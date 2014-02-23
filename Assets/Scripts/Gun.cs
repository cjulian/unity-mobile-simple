using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Camera mainCamera;
	public GameObject bullet;
	public float bulletSpeed = 10.0f;


	private Vector3 targetPoint;
	private Quaternion targetRotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Shoot(SimpleTouch t) {
		if (t.touchPhase == TouchPhase.Began && mainCamera != null) {
			targetPoint = mainCamera.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, mainCamera.transform.position.z * -1.0f));
			targetRotation = Quaternion.LookRotation(targetPoint - this.transform.position, Vector3.forward);

			GameObject bullet1 = GameObject.Instantiate(bullet, this.transform.position, targetRotation) as GameObject;
			bullet1.rigidbody.velocity = new Vector3(targetPoint.x - this.transform.position.x, targetPoint.y - this.transform.position.y, 0).normalized * bulletSpeed;

			Debug.Log (this.transform.position);
			Debug.Log (targetPoint);
			Debug.Log (bullet1.transform.rotation);
		}
	}
}
