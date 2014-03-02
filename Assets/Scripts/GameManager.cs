using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Camera mainCamera;
	public GUIText debugLabel; 

	private SimpleTouch[] touch;


	// Update is called once per frame
	void Update () {
		touch = _GetTouchInput();
	}


	public SimpleTouch[] GetTouchInput() {
		return touch;
	}


	// Return touch input if touchscreen or convert mouse input to touch input
	private SimpleTouch[] _GetTouchInput()
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
