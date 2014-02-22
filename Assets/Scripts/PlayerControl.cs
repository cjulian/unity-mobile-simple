using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public Camera mainCamera;

	// label for debugging
	public GUIText label; 

	// Jump Button
	public GUITexture jumpButton;

	// store input
	private SimpleTouch[] touch;

	// check for grounded
	private bool grounded = false;

	// JUMP related vars
	public float jumpVel = 20;
	public bool canGlide = false;
	public float glideVel = -1;
	public int maxAirJumps = 1;
	private int numAirJumps = 0;

	// Ground layer
	private int groundLayer = 8;

	// WEAPONS
	public GameObject weapon;
	private Gun gun;

	// TEST hand 
	public GameObject hand;

	// Use this for initialization
	void Start () {
		if (weapon != null) {
			gun = weapon.GetComponent<Gun>();
		}
	}


	// Update is called once per frame
	void Update () {

		// check if grounded
		grounded = GetGroundedState();
		label.text = grounded.ToString();
		if (grounded) {
			numAirJumps = 0;
		}

		// check for input
		bool jumped = false;
		bool aimed = false;
		touch = GetTouchInput ();

		if (touch != null) {
			foreach (SimpleTouch t in touch) {

				// If touch was on jump button...
				if (jumpButton != null && jumpButton.HitTest(t.position)) {

					// Jump logic.
					if (!jumped) {
						jumped = true;

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

				// Not the jump button, so shoot
				} else {
					if (hand != null && mainCamera != null && !aimed) {
//						aimed = true;
//						Vector3 handPos = mainCamera.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, mainCamera.transform.position.z * -1));
//
//						handPos.Set(handPos.x, handPos.y, hand.transform.position.z);
//						hand.transform.position = handPos;

						if (weapon != null && gun != null) {
							gun.Shoot(t);
						}
					}
				}
			}
		}
	}

	// TODO: limit raycast to only ground objects (check tag)
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


	// Functions for setting individual components of player velocity
	void SetVelX(float velX) {
		this.rigidbody.velocity = new Vector3(velX, this.rigidbody.velocity.y, this.rigidbody.velocity.z);
	}
	void SetVelY(float velY) {
		this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, velY, this.rigidbody.velocity.z);
	}
	void SetVelZ(float velZ) {
		this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, this.rigidbody.velocity.y, velZ);
	}


	// Return touch input if touchscreen or convert mouse input to touch input
	SimpleTouch[] GetTouchInput()
	{
		SimpleTouch[] touches = null;

		// Check for touch input...
		if (Input.touchCount > 0) {
			touches = new SimpleTouch[Input.touchCount];

			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch t = Input.GetTouch (i);
				touches[i] = new SimpleTouch
				{
					position = new Vector3(t.position.x, t.position.y, 0),
					touchPhase = t.phase
				};
			}


		// If no touch input check for mouse input
		} else {
			bool touched = false;
			TouchPhase phase = TouchPhase.Canceled;

			if (Input.GetMouseButtonDown(0)) {
				phase = TouchPhase.Began;
				touched = true;

			} else if (Input.GetMouseButton(0)) {
				phase = TouchPhase.Stationary;
				touched = true;

			} else if (Input.GetMouseButtonUp(0)) {
				phase = TouchPhase.Ended;
				touched = true;
			}

			if (touched) {
				touches = new SimpleTouch[1];
				touches[0] = new SimpleTouch 
				{
					position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0),
					touchPhase = phase
				};
			}		
		}

		return touches;
	}
}

public class SimpleTouch {
	public TouchPhase touchPhase;
	public Vector3 position;
}
