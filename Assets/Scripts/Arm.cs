using UnityEngine;
using System.Collections;

public class Arm : MonoBehaviour {

	public GameManager gameManager;
	public Gun weaponScript;

	private Vector3 targetPoint;
	private Quaternion targetRotation;


	// Rotates the character's arm to point towards where the user has touched and fires the current weapon.
	public void aim(SimpleTouch t) {
		if (gameManager != null && (t.touchPhase == TouchPhase.Began || t.touchPhase == TouchPhase.Moved || t.touchPhase == TouchPhase.Stationary)) {
			targetPoint = gameManager.mainCamera.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, (gameManager.mainCamera.transform.position.z * -1.0f) + this.transform.position.z));
			targetRotation = Quaternion.LookRotation(targetPoint - this.transform.position, Vector3.forward);
			this.transform.rotation = targetRotation;

			if (weaponScript != null) {
				weaponScript.Shoot(targetPoint, targetRotation, this.transform.position);
			}
		}	
	}
}
