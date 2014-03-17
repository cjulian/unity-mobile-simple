using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public Animator anim;
	public GameManager gameManager;

	// Jump Button Texture
	public GUITexture jumpButton;

	private SimpleTouch[] touch;
	private bool grounded = true;

	// left-to-right movement speed
	public float velX = 5f;

	// JUMP related vars
	public float jumpVel = 20;
	public bool canGlide = false;
	public float glideVel = -1;
	public int maxAirJumps = 1;
	private int numAirJumps = 0;

	private int groundedLayerMask;
	public Arm armScript;


	void Update () {
		this.SetVelX(velX);
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
	}


	// Jump logic.  Decide if player should jump, double jump or glide.
	void Jump(SimpleTouch t) {
		switch (t.touchPhase){
			case TouchPhase.Began:
				if (grounded || numAirJumps < maxAirJumps) {
					SetVelY(jumpVel);
					
					if (!grounded) {
						numAirJumps++;
					}
				}
				break;
				
				
			case TouchPhase.Stationary:
			case TouchPhase.Moved:
				if (canGlide && this.rigidbody.velocity.y <= glideVel) {
					SetVelY(glideVel);
				}
				break;
				
				
			case TouchPhase.Ended:
				if (this.rigidbody.velocity.y > 0) {
					SetVelY(this.rigidbody.velocity.y * 0.4f);
				}
				break;
				
				
			default:				
				break;
		}			
	}


	void OnCollisionStay(Collision c) {
		GroundCollisionCheck(c);
	}
	void OnCollisionEnter(Collision c) {
		GroundCollisionCheck(c);
	}
	void OnCollisionExit() {
		grounded = false;
	}


	// Check if the player is grounded
	void GroundCollisionCheck(Collision c) {
		float playerColliderBottomY = this.collider.bounds.center.y - this.collider.bounds.size.y / 2;

		foreach(ContactPoint cp in c.contacts) {

			// If the collision point is less than 0.1 world units above the bottom of the player collider...
			// i.e. if the collision point is on the bottom of the collider
			if (cp.point.y - playerColliderBottomY < 0.1f) {

				// If that bottom collision point is relatively flat...
				if (cp.normal.normalized.y > 0.6f) {
					grounded = true;
					break;
				}
			}
		}
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