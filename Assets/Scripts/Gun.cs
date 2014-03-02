using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public GUIText clipGUI;
	public GameObject arm;
	public GameObject bulletPrefab;
	public float bulletSpeed = 20.0f;
	private GameObject[] bullets;

	public int bulletCacheSize = 10;
	private int bulletCacheIndex = 0;

	public int bulletClipSize = 10;
	private int bulletClipIndex = 0;

	public float shotDelay = 0.15f;
	private bool shotDelayOver = true;

	public float reloadDelay = 1.0f;
	private bool reloadDelayOver = true;


	// Use this for initialization
	void Start () {
		bullets = new GameObject[bulletCacheSize];

		for (int i = 0; i < bulletCacheSize; i++) {
			bullets[i] = GameObject.Instantiate(bulletPrefab) as GameObject;
		}
	}


	// Update is called once per frame
	void Update () {
		UpdateClipGUI();
	}


	// Shoot the gun's bullets at the targetPoint, rotating the bullets into the proper orientation.
	// Manages firing rate and reload time and also tracks number of bullets remaining in clip.
	public void Shoot(Vector3 targetPoint, Quaternion targetRotation) {
		if (shotDelayOver && reloadDelayOver) {
			bullets[bulletCacheIndex].SetActive(true);
			bullets[bulletCacheIndex].transform.position = this.transform.position;
			bullets[bulletCacheIndex].transform.rotation = targetRotation;
			bullets[bulletCacheIndex].rigidbody.velocity = new Vector3(targetPoint.x - arm.transform.position.x, targetPoint.y - arm.transform.position.y, 0).normalized * bulletSpeed;
			bulletCacheIndex = (bulletCacheIndex + 1) % bulletCacheSize;
			shotDelayOver = false;
			StartCoroutine(StartShotDelay());
			
			bulletClipIndex = (bulletClipIndex + 1);
			if (bulletClipIndex == bulletClipSize) {
				reloadDelayOver = false;
				StartCoroutine(StartReloadDelay());
			}
			bulletClipIndex %= bulletClipSize;
		}
	}


	// Update the GUI that displays remaining bullets in clip
	private void UpdateClipGUI() {
		if (clipGUI != null) {
			int GUIBulletIndex = bulletClipSize - bulletClipIndex;
			
			if (GUIBulletIndex == bulletClipSize) {
				GUIBulletIndex = reloadDelayOver ? bulletClipSize : 0;
			}
			
			clipGUI.text = GUIBulletIndex + " / " + bulletClipSize;
		}	
	}


	private IEnumerator StartShotDelay () {
		yield return new WaitForSeconds(shotDelay);
		shotDelayOver = true;
	}


	private IEnumerator StartReloadDelay () {
		yield return new WaitForSeconds(reloadDelay);
		reloadDelayOver = true;
		shotDelayOver = true;
	}
}
