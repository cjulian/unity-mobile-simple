using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	// label for debugging
	public GUIText label; 

	// store input
	private SimpleTouch touch;

	// check for grounded
	private bool grounded = false;

	// JUMP related vars
	public float jumpVel = 20;
	public bool canGlide = false;
	public float glideVel = -1;
	public int maxAirJumps = 1;
	private int numAirJumps = 0;


	// Use this for initialization
	void Start () {
		
	}


	// Update is called once per frame
	void Update () {
		touch = GetTouchInput ();
		grounded = GetGroundedState();
		label.text = grounded.ToString();

		if (grounded) {
			numAirJumps = 0;
		}

		// JUMP
		switch (touch.touchPhase){
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

	// TODO: limit raycast to only ground objects (check tag)
	bool GetGroundedState()	{
		bool grounded = false;
		Vector3 pos = this.rigidbody.collider.bounds.center;
		float height = this.rigidbody.collider.bounds.size.y;
		float leftX = pos.x - this.rigidbody.collider.bounds.size.x / 2;
		float rightX = pos.x + this.rigidbody.collider.bounds.size.x / 2;

		RaycastHit hit;

		// test left, right and center of collider
		if (Physics.Raycast(new Vector3(leftX,pos.y, pos.z), -Vector3.up, out hit, height/2) ||
		    Physics.Raycast(new Vector3(pos.x,pos.y, pos.z), -Vector3.up, out hit, height/2) ||
		    Physics.Raycast(new Vector3(rightX,pos.y, pos.z), -Vector3.up, out hit, height/2))
		{
			grounded = true;
		}

		return grounded;
	}

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
	SimpleTouch GetTouchInput()
	{
		bool touched = false;
		float tx = 0;
		float ty = 0;
		TouchPhase phase = TouchPhase.Canceled;

		// Check for touch input...
		if (Input.touchCount > 0) {
			Touch t = Input.GetTouch(0);
			
			tx = t.position.x;
			ty = t.position.y;
			phase = t.phase;
			touched = true;

		// If no touch input check for mouse input
		} else if (Input.GetMouseButtonDown(0)) {
			tx = Input.mousePosition.x;
			ty = Input.mousePosition.y;
			phase = TouchPhase.Began;
			touched = true;

		} else if (Input.GetMouseButton(0)) {
			tx = Input.mousePosition.x;
			ty = Input.mousePosition.y;
			phase = TouchPhase.Stationary;
			touched = true;

		} else if (Input.GetMouseButtonUp(0)) {
			tx = Input.mousePosition.x;
			ty = Input.mousePosition.y;
			phase = TouchPhase.Ended;
			touched = true;
		}

		if (touched) {
			if (label != null) {
				label.text = phase.ToString() + ": " + tx + ", " + ty;
			}
		}

		return new SimpleTouch {
			touchPhase = phase,
			position = new Vector2(tx, ty)
		};
	}
}

public class SimpleTouch {
	public TouchPhase touchPhase;
	public Vector2 position;
}
