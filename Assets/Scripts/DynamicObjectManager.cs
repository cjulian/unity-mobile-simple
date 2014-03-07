using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Sets Dynamic objects to active once they get close to entering the main camera's field of view.
// Deactivates Dynamic objects once they've passed out of view.
// Dynamic objects are objects with the tag "Dynamic"
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

		dynamicObjectsA = GameObject.FindGameObjectsWithTag("Dynamic");
		dynamicObjects = new List<GameObject>(dynamicObjectsA);
		HideOffscreenObjects();

		InvokeRepeating ("TestObjects", 0, enableInterval);
	}


	void HideOffscreenObjects() {
		foreach(GameObject obj in dynamicObjects) {
			if (obj.transform.position.x - g.mainCamera.transform.position.x > g.getCameraOrthWidth() + enableThreshold) {
				obj.SetActive(false);
			}
		}
	}


	void TestObjects() {
		Debug.Log("Checked!");


		for(int i = 0; i < dynamicObjects.Count; i++) {
			if (dynamicObjects[i].transform.position.x - g.mainCamera.transform.position.x < -(g.getCameraOrthWidth() + enableThreshold/2)) {
				dynamicObjects[i].SetActive(false);
				dynamicObjects.RemoveAt(i);
				i--;

			} else if (!dynamicObjects[i].activeInHierarchy && dynamicObjects[i].transform.position.x - g.mainCamera.transform.position.x < g.getCameraOrthWidth() + enableThreshold) {
				dynamicObjects[i].SetActive(true);
			}
		}
	}
}
