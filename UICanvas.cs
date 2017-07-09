using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour {

	public Transform cam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, cam.position + cam.forward * 2f, Time.deltaTime*5f);
		transform.LookAt (cam);
		transform.Rotate (Vector3.up * 180);
	}
}
