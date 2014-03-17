using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {
	
	public int healthMax = 1;
	protected int healthCurr;
	protected GameManager gm;
	protected ParticleSystem ps;


	// - Initialize current health to max.
	// - Initialize particle system.
	public virtual void Start () {
		healthCurr = healthMax;

		if (gm == null) {
			gm = (GameManager) GameObject.FindObjectOfType<GameManager>();
			ps = gm.mainParticleSystem;
		}
	}


	// Update is called once per frame
	public void Update () {
		if (healthCurr <= 0) {
			Die();
		}
	}
	

	// Calculate damage and remaining health.
	public virtual void Hit(int damage, Vector3 point, Vector3 normal) {

		// Health
		healthCurr -= damage;

		// Particle
		if (ps != null) {
			ps.transform.position = point;
			ps.transform.rotation = Quaternion.LookRotation(normal);
			ps.Emit(3);
		}
		
		// Colour
		renderer.material.SetColor("_Color", new Color(1f, 0.9f, 0.9f));
		StartCoroutine(DefaultColor());
	}
	

	// Handle the "death" of this object
	protected virtual void Die() {
		this.gameObject.SetActive(false);
	}


	// Helper function to return this object to default colour after 0.1 seconds
	protected IEnumerator DefaultColor() {
		yield return new WaitForSeconds(0.1f);
		renderer.material.SetColor("_Color", Color.white);
	}
}