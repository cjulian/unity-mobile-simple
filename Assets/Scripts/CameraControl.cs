using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject player = null;
	float offsetX = 0;
	float offsetY = 0;

	// Use this for initialization
	void Start () {
		offsetX = this.transform.position.x;
		offsetY = this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3(player.transform.position.x + offsetX, offsetY, this.transform.position.z);
	}
}
