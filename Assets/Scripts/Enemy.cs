using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int healthMax = 100;
	public int healthCurr = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (healthCurr <= 0) {
			this.gameObject.SetActive(false);
		}
	}

	public void Hit(int damage) {
		healthCurr -= damage;
	}
}
