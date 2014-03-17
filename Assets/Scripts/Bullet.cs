using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private Renderer bulletRenderer;
	private int damage = 0;


	public void SetDamage(int d) {
		damage = d;
	}

	void Start () {
		if (bulletRenderer == null) {
			Renderer thisRenderer = bulletRenderer = this.GetComponent<MeshRenderer>();
			Renderer childRenderer = this.GetComponentInChildren<MeshRenderer>();

			bulletRenderer = thisRenderer != null ? thisRenderer : childRenderer;
		}
	}


	void Update () {
		// deactivate bullet if it's not visible
		if (!bulletRenderer.isVisible) {
			this.gameObject.SetActive(false);
		}

		// re-orient bullet to point in direction of travel (eg. arrow orientation)
		this.transform.rotation = Quaternion.LookRotation(this.rigidbody.velocity, Vector3.back);
	}


	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.layer == LayerMask.NameToLayer("Destructible") ||
		    collision.gameObject.layer == LayerMask.NameToLayer("Enemy") ||
		    collision.gameObject.layer == LayerMask.NameToLayer("EnemyPlatform"))
		{
			Destructible d = collision.gameObject.GetComponent<Destructible>();
			if (d != null) {
				d.Hit(damage, collision.contacts[0].point, collision.contacts[0].normal);
			}
		}

		// make this bullet disappear
		this.gameObject.SetActive(false);
	}
}
