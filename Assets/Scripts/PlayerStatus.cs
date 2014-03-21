using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	public int health = 100;
	private int groundedLayerMask;


	// Use this for initialization
	void Start () {
		int platformLayerMask = 1 << LayerMask.NameToLayer("Platform");
		int enemyPlatformLayerMask = 1 << LayerMask.NameToLayer("EnemyPlatform");
		int destructibleLayerMask = 1 << LayerMask.NameToLayer("Destructible");
		groundedLayerMask = platformLayerMask | enemyPlatformLayerMask | destructibleLayerMask;	
	}


	// Update is called once per frame
	void Update () {
	
	}


	// Check if the player's feet are on the ground
	public bool GetGroundedState() {
		Vector3 colliderRight = this.collider.bounds.center;
		colliderRight.x += collider.bounds.size.x / 2f - 0.2f;
		colliderRight.y += collider.bounds.size.y / -2f + 0.2f;
		
		Vector3 colliderLeft = this.collider.bounds.center;
		colliderLeft.x += collider.bounds.size.x / -2;
		colliderLeft.y += collider.bounds.size.y / -2f + 0.2f;
		
		RaycastHit hit;
		if (Physics.CapsuleCast(colliderRight, colliderLeft, 0.1f, Vector3.down, out hit, 0.21f, groundedLayerMask)) {
			if (hit.normal.normalized.y > 0.9) {
				return true;
			}
		}

		// Debug.DrawLine(colliderLeft + new Vector3(0, 0, -10f), colliderRight + new Vector3(0, 0, -10f) , Color.red);		
		return false;
	}
	
	
	// Check if the player's righward movement is blocked by any objects
	public bool GetBlockedState() {
		Vector3 colliderTop = this.collider.bounds.center;
		colliderTop.x += collider.bounds.size.x / 2 - 0.3f;
		colliderTop.y += collider.bounds.size.y / 2 - 0.15f;
		
		Vector3 colliderBottom = this.collider.bounds.center;
		colliderBottom.x += collider.bounds.size.x / 2 - 0.3f;
		colliderBottom.y += -1 * collider.bounds.size.y / 2 + 0.15f;
		
		RaycastHit hit;
		if (Physics.CapsuleCast(colliderBottom, colliderTop, 0.1f, Vector3.right, out hit, 0.31f, groundedLayerMask)) {
			return true;
		}

		// Debug.DrawLine(colliderBottom + new Vector3(0,0,-10f), colliderTop + new Vector3(0,0,-10f), Color.red);
		return false;
	}
}
