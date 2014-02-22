using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public GameObject bullet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Shoot(SimpleTouch t) {
		if (t.touchPhase == TouchPhase.Began) {
			Debug.Log ("shoot");
		}
	}
}
