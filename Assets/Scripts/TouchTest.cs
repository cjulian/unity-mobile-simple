using UnityEngine;
using System.Collections;

public class TouchTest : MonoBehaviour {

	public GUIText label;

	private float tX = 0;
	private float tY = 0;
	private float tZ = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		string debugText = "";
		bool touched = false;

		if (Input.touchCount > 0) {
			Touch t = Input.GetTouch(0);

			if (t.phase == TouchPhase.Began) {
				tX = t.position.x;
				tY = t.position.y;
			}

			debugText = "touch: ";
			touched = true;
			
		} else if (Input.GetMouseButtonDown(0)) {

			tX = Input.mousePosition.x;
			tY = Input.mousePosition.y;
			tZ = Input.mousePosition.z;

			debugText = "mouse: ";
			touched = true;
		}

		debugText += tX + ", " + tY + ", " + tZ;

		if (label != null) {
		    if (touched) {
				label.text = debugText;
			}
		}
	}
}

