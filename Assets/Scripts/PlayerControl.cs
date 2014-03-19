using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public Animator anim;
	public GameManager gameManager;

	// Jump Button Texture
	public GUITexture jumpButton;

	private SimpleTouch[] touch;
	private bool grounded = true;
	private bool blocked = false;

	// left-to-right movement speed
	public float walkSpeed = 5f;

	// JUMP related vars
	public float jumpVel = 20;
	public bool canGlide = false;
	public float glideVel = -1;
	public int maxAirJumps = 1;
	private int numAirJumps = 0;

	private float velX = 0;
	private float velY = 0;


	private int groundedLayerMask;
	public Arm armScript;


	void Start() {
		int platformLayerMask = 1 << LayerMask.NameToLayer("Platform");
		int enemyPlatformLayerMask = 1 << LayerMask.NameToLayer("EnemyPlatform");
		int destructibleLayerMask = 1 << LayerMask.NameToLayer("Destructible");
		groundedLayerMask = platformLayerMask | enemyPlatformLayerMask | destructibleLayerMask;
	}


	void Update () {
		blocked = GetBlockedState();
		if (!blocked) {
			velX = walkSpeed;
		} else {
			velX = 0;
		}

		velY = this.rigidbody.velocity.y;

		this.rigidbody.AddForce(Vector3.right * walkSpeed, ForceMode.Impulse);

		grounded = GetGroundedState();
		anim.SetBool("Grounded", grounded);

		if (grounded) {
			numAirJumps = 0;
		}

		touch = gameManager.GetTouchInput();
		if (touch != null) {
			bool jumped = false;
			bool aimed = false;

			foreach (SimpleTouch t in touch) {

				// If touch was on jump button, jump.  Otherwise, shoot.
				if (jumpButton != null && jumpButton.HitTest(t.position)) {
					if (!jumped) {
						Jump(t);
						jumped = true;
					}
				} else {
					if (!aimed && armScript != null) {
						armScript.aim(t);
						aimed = true;
					}
				}
			}
		}	

		this.rigidbody.velocity = new Vector3(velX, velY, 0);
	}


	// Jump logic.  Decide if player should jump, double jump or glide.
	void Jump(SimpleTouch t) {
		switch (t.touchPhase){
			case TouchPhase.Began:
				if (grounded || numAirJumps < maxAirJumps) {
					velY = jumpVel;
					
					if (!grounded) {
						numAirJumps++;
					}
				}
				break;
				
				
			case TouchPhase.Stationary:
			case TouchPhase.Moved:
				if (canGlide && this.rigidbody.velocity.y <= glideVel) {
					velY = glideVel;
				}
				break;
				
				
			case TouchPhase.Ended:
				if (this.rigidbody.velocity.y > 0) {
					velY = this.rigidbody.velocity.y * 0.4f;
				}
				break;
				
				
			default:				
				break;
		}			
	}


	// cast a capsule below the player's feet a very short distance and return true if it hits anything.
	bool GetGroundedState() {
//		float colliderBottomY = this.collider.bounds.center.y - this.collider.bounds.size.y / 2;

		Vector3 colliderRight = this.collider.bounds.center;
		colliderRight.x += collider.bounds.size.x / 2f - 0.2f;
		colliderRight.y += collider.bounds.size.y / -2f + 0.2f;

		Vector3 colliderLeft = this.collider.bounds.center;
		colliderLeft.x += collider.bounds.size.x / -2;
		colliderLeft.y += collider.bounds.size.y / -2f + 0.2f;

//		Debug.DrawLine(colliderLeft + new Vector3(0, 0, -10f), colliderRight + new Vector3(0, 0, -10f) , Color.red);

		RaycastHit hit;
		if (Physics.CapsuleCast(colliderRight, colliderLeft, 0.1f, Vector3.down, out hit, 0.21f, groundedLayerMask)) {
			if (hit.normal.normalized.y > 0.9) {
				return true;
			}
		}

		return false;
	}


	// cast a capsule to the player's right a very short distance and return true if it hits anything.
	bool GetBlockedState() {
//		float colliderRightX = this.collider.bounds.center.x + this.collider.bounds.size.x / 2;

		Vector3 colliderTop = this.collider.bounds.center;
		colliderTop.x += collider.bounds.size.x / 2 - 0.3f;
		colliderTop.y += collider.bounds.size.y / 2;

		Vector3 colliderBottom = this.collider.bounds.center;
		colliderBottom.x += collider.bounds.size.x / 2 - 0.3f;
		colliderBottom.y += -1 * collider.bounds.size.y / 2 + 0.15f;

//		Debug.DrawLine(colliderBottom + new Vector3(0,0,-10f), colliderTop + new Vector3(0,0,-10f), Color.red);

		RaycastHit hit;
		if (Physics.CapsuleCast(colliderBottom, colliderTop, 0.1f, Vector3.right, out hit, 0.31f, groundedLayerMask)) {
			return true;
		}

//		Debug.Log ("not blocked");
		return false;
	}


	// Convenience functions for setting individual components of player velocity
	void SetVelX(float velX) {
		this.rigidbody.velocity = new Vector3(velX, this.rigidbody.velocity.y, this.rigidbody.velocity.z);
	}
	void SetVelY(float velY) {
		this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, velY, this.rigidbody.velocity.z);
	}
	void SetVelZ(float velZ) {
		this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, this.rigidbody.velocity.y, velZ);
	}
}