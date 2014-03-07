using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public Animator anim;
	public GameManager gameManager;

	// Jump Button
	public GUITexture jumpButton;

	// store input
	private SimpleTouch[] touch;

	// check for grounded
	private bool grounded = false;

	// walking speed
	public float velX = 5f;

	// JUMP related vars
	public float jumpVel = 20;
	public bool canGlide = false;
	public float glideVel = -1;
	public int maxAirJumps = 1;
	private int numAirJumps = 0;

	// Ground layer
	private int groundLayer;

	// ARM
	public Arm armScript; // arm script


	void Awake() {
		groundLayer = LayerMask.NameToLayer("Ground");
	}

	// Update is called once per frame
	void Update () {
		this.SetVelX(velX);
//		this.rigidbody.AddForce(Vector3.right * velX/10, ForceMode.VelocityChange);

		grounded = GetGroundedState();
		anim.SetBool("Grounded", grounded);

		if (grounded) {
			numAirJumps = 0;
		}

		touch = gameManager.GetTouchInput();
		if (touch != null) {
			bool jumped = false;

			foreach (SimpleTouch t in touch) {

				// If touch was on jump button, jump.  Otherwise, shoot.
				if (jumpButton != null && jumpButton.HitTest(t.position)) {
					if (!jumped) {
						Jump(t);
						jumped = true;
					}
				} else {
					if (armScript != null) {
						armScript.aim(t);
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
					SetVelY(this.rigidbody.velocity.y * 0.2f);
				}
				break;
				
				
			default:				
				break;
		}			
	}


	// Returns true if player is on the ground
	bool GetGroundedState()	{
		bool grounded = false;
		Vector3 pos = this.rigidbody.collider.bounds.center;
		float height = this.rigidbody.collider.bounds.size.y;
		float leftX = pos.x - this.rigidbody.collider.bounds.size.x / 2;
		float rightX = pos.x + this.rigidbody.collider.bounds.size.x / 2;

		RaycastHit hit;	

		// test left, right and center of collider
		if (Physics.Raycast(new Vector3(leftX, pos.y, pos.z), -Vector3.up, out hit, height/2, 1 << groundLayer) ||
		    Physics.Raycast(new Vector3(pos.x, pos.y, pos.z), -Vector3.up, out hit, height/2, 1 << groundLayer) ||
		    Physics.Raycast(new Vector3(rightX, pos.y, pos.z), -Vector3.up, out hit, height/2, 1 << groundLayer))
		{
			grounded = true;
		}

		return grounded;
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