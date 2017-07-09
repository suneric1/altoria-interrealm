using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentHolder : MonoBehaviour {

	bool highlighted = false;

	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Show(){
		GetComponent<MeshRenderer> ().enabled = true;
	}

	public void Hide(){
		print ("hide!!");
		GetComponent<MeshRenderer> ().enabled = false;
	}

	public void Highlight(){
		if (GetComponent<shaderGlow> () && !highlighted) {
			GetComponent<shaderGlow> ().lightOn ();
			highlighted = true;
		}
	}

	public void Unhighlight(){
		if (GetComponent<shaderGlow> () && highlighted) {
			GetComponent<shaderGlow> ().lightOff ();
			highlighted = false;
		}
	}
}
