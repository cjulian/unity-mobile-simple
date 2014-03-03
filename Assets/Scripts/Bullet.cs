using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public Renderer bulletRenderer;
	public int damage = 0;


	// Use this for initialization
	void Start () {
		if (bulletRenderer == null) {
			Renderer thisRenderer = bulletRenderer = this.GetComponent<MeshRenderer>();
			Renderer childRenderer = this.GetComponentInChildren<MeshRenderer>();

			bulletRenderer = thisRenderer != null ? thisRenderer : childRenderer;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!bulletRenderer.isVisible) {
			this.gameObject.SetActive(false);
		}
	}

	void OnCollisionEnter(Collision collision) {
//		Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal * 2.0f, Color.white, 0.24f);
		collision.gameObject.GetComponent<Enemy>().Hit(damage);
		this.gameObject.SetActive(false);
	}
}
