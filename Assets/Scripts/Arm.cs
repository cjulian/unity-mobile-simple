using UnityEngine;
using System.Collections;

public class Arm : MonoBehaviour {

	public Camera mainCamera;

	private Vector3 targetPoint;
	private Quaternion targetRotation;

	// Use this for initialization
	void Start () {
	
	}
	
	public void aim(SimpleTouch t) {
		if (mainCamera != null && t.touchPhase == TouchPhase.Began || t.touchPhase == TouchPhase.Moved || t.touchPhase == TouchPhase.Stationary) {
			targetPoint = mainCamera.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, (mainCamera.transform.position.z * -1.0f) + this.transform.position.z));
			targetRotation = Quaternion.LookRotation(targetPoint - this.transform.position, Vector3.back);
			this.transform.rotation = targetRotation;
		}	
	}
}
