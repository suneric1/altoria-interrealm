using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPower : MonoBehaviour {

	public bool emitted = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (emitted && other.GetComponent<Exposable>()!=null && other.GetComponent<Exposable>().Exposed && !other.GetComponent<Exposable>().OutOfRealm) {
			other.GetComponent<Rigidbody> ().AddForce (GetComponent<Rigidbody> ().velocity.normalized*400);
			other.GetComponent<Exposable> ().PushOutRealm ();
		}
	}
}
