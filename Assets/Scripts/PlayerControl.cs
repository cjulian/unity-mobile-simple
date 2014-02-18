using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	// label for debugging
	public GUIText label; 

	// store input
	private SimpleTouch touch;

	// JUMP related vars
	public float jumpVel = 20;
	public bool canGlide = false;
	public float glideVel = -1;
	public int maxNumJumps = 2;
	private int numJumps = 0;


	// Use this for initialization
	void Start () {
		
	}


	// Update is called once per frame
	void Update () {
		touch = GetTouchInput ();

		// JUMP
		switch (touch.touchPhase){
			case TouchPhase.Began:
				if (numJumps < maxNumJumps - 1) {
					this.rigidbody.velocity = Vector3.up * jumpVel;
					numJumps++;
				}
				break;


			case TouchPhase.Stationary:
			case TouchPhase.Moved:
				if (canGlide && this.rigidbody.velocity.y <= glideVel) {
					this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, glideVel, this.rigidbody.velocity.z);
				}
				break;


			case TouchPhase.Ended:
				if (this.rigidbody.velocity.y > 0) {
					//this.rigidbody.velocity = Vector3.zero;
					this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, this.rigidbody.velocity.y * 0.2f, this.rigidbody.velocity.z);
				}
				break;


			default:				
				break;
		}


		if (this.transform.position.y <= 0.5) {
			this.transform.position = new Vector3(this.transform.position.x, 0.5f, this.transform.position.z);
			numJumps = 0;
		}
	}


	// Return touch input if touchscreen or convert mouse input to touch input
	SimpleTouch GetTouchInput() {
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
