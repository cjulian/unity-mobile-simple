using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Tracks dynamic objects and enables/disables them as they move into/out of camera view.
// Dynamic objects are objects tagged "Dynamic"
public class DynamicObjectManager : MonoBehaviour {

	public GameManager g;
	private GameObject[] dynamicObjectsA;
	private List<GameObject> dynamicObjects;
	public float enableThreshold = 5f;
	public float enableInterval = 1f;


	void Start() {
		if (g == null) {
			g = GameObject.Find("_GameManager").GetComponentInChildren<GameManager>();
		}

		// Create a list of all dynamic objects
		dynamicObjectsA = GameObject.FindGameObjectsWithTag("Dynamic");
		dynamicObjects = new List<GameObject>(dynamicObjectsA);

		HideOffscreenObjects();
		InvokeRepeating ("TestObjects", 0, enableInterval);
	}


	// Disable objects outside of the camera's field of view
	void HideOffscreenObjects() {
		float camOrthWidth = g.getCameraOrthWidth();
		float camRightThreshold = camOrthWidth + enableThreshold;
		float camLeftThreshold =  -(camOrthWidth + enableThreshold/2);

		foreach(GameObject obj in dynamicObjects) {
			float objPositionRelativeToCam = obj.transform.position.x - g.mainCamera.transform.position.x;

			if (obj.activeInHierarchy && (objPositionRelativeToCam > camRightThreshold || objPositionRelativeToCam < camLeftThreshold)) {
				obj.SetActive(false);
			}
		}
	}


	// Enable objects that move into the camera's field of view from the right.
	// Disable objects that move past the camera's field of view to the left
	// Disabled objects are removed them from list of dynamic objects and never checked again.
	void TestObjects() {
		float camOrthWidth = g.getCameraOrthWidth();
		float camRightThreshold = camOrthWidth + enableThreshold;
		float camLeftThreshold =  -(camOrthWidth + enableThreshold/2);

		for(int i = 0; i < dynamicObjects.Count; i++) {
			float objPositionRelativeToCam = dynamicObjects[i].transform.position.x - g.mainCamera.transform.position.x;

			if (objPositionRelativeToCam < camLeftThreshold) {
				dynamicObjects[i].SetActive(false);
				dynamicObjects.RemoveAt(i);
				i--;

			} else if (!dynamicObjects[i].activeInHierarchy && objPositionRelativeToCam < camRightThreshold) {
				dynamicObjects[i].SetActive(true);
			}
		}
	}
}
