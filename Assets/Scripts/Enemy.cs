using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int healthMax = 100;
	public int healthCurr = 100;
	public ParticleSystem ps;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (healthCurr <= 0) {
			this.gameObject.SetActive(false);
		}
	}

	public void Hit(int damage, Vector3 point, Vector3 normal) {
		if (ps != null) {
			ps.transform.position = point;
			ps.transform.rotation = Quaternion.LookRotation(normal);
			ps.Emit(3);
		}

		healthCurr -= damage;
		renderer.material.SetColor("_Color", new Color(1f, 0.9f, 0.9f));
		StartCoroutine(DefaultColor());
	}

	private IEnumerator DefaultColor() {
		yield return new WaitForSeconds(0.1f);
		renderer.material.SetColor("_Color", Color.white);
	}
}
