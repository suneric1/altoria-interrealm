using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public int maskCount = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Camera.main.transform.position;
		Vector3 targetRotation = new Vector3 (transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
		transform.localRotation = Quaternion.Lerp (Quaternion.Euler (transform.localEulerAngles), Quaternion.Euler (targetRotation), Time.deltaTime * 5f);
	}
}
