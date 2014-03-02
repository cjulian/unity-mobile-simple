using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public Renderer bulletRenderer;

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
}
