using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	// External objects
	public GameManager gameManager;
	public Animator anim;
	public GUITexture jumpButton;
	private PlayerStatus playerStatus;
	private Arm armScript;

	private SimpleTouch[] touch;
	public float walkSpeed = 5f;
	public float jumpVel = 20;
	public bool canGlide = false;
	public float glideVel = -1;
	public int maxAirJumps = 1;
	private int numAirJumps = 0;
	private float velX = 0;
	private float velY = 0;
	private bool grounded = false;
	public float knockBackDelay = 1f;
	public float recoveryDelay = 1.1f;
	public float invincibleDelay = 1.5f;
	bool invincible = false;
	HitState hitState = HitState.normal;


	void Start() {
		playerStatus = this.GetComponent<PlayerStatus>();
		armScript = this.GetComponentInChildren<Arm>();
	}


	void Update () {
		grounded = playerStatus.GetGroundedState();
		anim.SetBool("Grounded", grounded);

		if (grounded) {
			numAirJumps = 0;
		}

		// default velocity
		velX = playerStatus.GetBlockedState() == true ? 0 : walkSpeed;
		velY = this.rigidbody.velocity.y;

		// velocity can be overridden by player input or if player is hit
		ManageInput();
		ManageHitState();

		this.rigidbody.velocity = new Vector3(velX, velY, 0);
	}


	// Detect input and jump or shoot accordingly.
	void ManageInput() 
	{
		touch = gameManager.GetTouchInput();
		if (touch != null) {
			bool jumped = false;
			bool aimed = false;
			
			foreach (SimpleTouch t in touch) {
				if (jumpButton != null && jumpButton.HitTest(t.position)) {
					if (!jumped) {
						Jump(t);
						jumped = true;
					}
				} else {
					if (!aimed && armScript != null && hitState != HitState.knockback) {
						armScript.aim(t);
						aimed = true;
					}
				}
			}
		}	
	}


	// Decide if player should jump, double jump or glide and
	// then set Y velocity accordingly
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


	void OnTriggerEnter(Collider c) {
		if (!invincible) {
			hitState = HitState.hit;
		}
	}


	void ManageHitState() {
		switch(hitState) {

		// Start knockback.
		// Player invincibility starts.
		case HitState.hit:
			velX = -8f;
			velY = 10f;
			invincible = true;
			hitState = HitState.knockback;
			StartCoroutine(KnockBackFromHit());
			StartCoroutine(RecoverFromHit());
			StartCoroutine(InvincibilityTimer());
			this.collider.material.dynamicFriction = 1.0f;
			break;
		
		// Continue moving back, even if grounded
		case HitState.knockback:
			velX = this.rigidbody.velocity.x;
			velY = this.rigidbody.velocity.y;
			break;

		// Continue moving back until grounded.
		// Once grounded, do not move forward until recovered.
		case HitState.recovering:
			velX = grounded ? 0 : velX = this.rigidbody.velocity.x;
			velY = grounded ? 0 : this.rigidbody.velocity.y;
			break;

		// Once grounded return player to normal state.
		case HitState.recovered:
			this.collider.material.dynamicFriction = 0;
			if (grounded) {
				hitState = HitState.normal;
			}
			break;

		case HitState.normal:
		default:
			break;
		}
	}


	private IEnumerator KnockBackFromHit () {
		yield return new WaitForSeconds(knockBackDelay);
		hitState = HitState.recovering;
	}


	private IEnumerator RecoverFromHit () {
		yield return new WaitForSeconds(recoveryDelay);
		hitState = HitState.recovered;
	}


	private IEnumerator InvincibilityTimer () {
		yield return new WaitForSeconds(invincibleDelay);
		invincible = false;
	}
}

public enum HitState {
	normal,
	hit,
	knockback,
	recovering,
	recovered
}