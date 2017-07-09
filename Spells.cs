using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour {

	public ParticleSystem gather;
	public ParticleSystem power;
	public Controller rController;
	public Transform hand;

	bool gathering = false;
	float powerValue = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (rController.GrabbedObject == null) {
			if (Input.GetAxisRaw ("Submit") == 1 && !gathering) {
				gathering = true;
				powerValue = 0;
				gather.Play ();
				power.Play ();
			} else if (Input.GetAxisRaw ("Submit") == 0 && gathering) {
				gathering = false;
				gather.Stop ();
				power.Stop ();
				if (powerValue >= 0.3f) {
					powerValue = 0;
					EmitPush (-hand.up * 3000);
				}
			}
		}

		if (gathering) {
			powerValue += 0.6f * Time.deltaTime;
			if (powerValue > 0.3f)
				powerValue = 0.3f;
			ParticleSystem.MainModule pm = power.main;
			pm.startSize = powerValue;
		}
	}

	void EmitPush(Vector3 force){
		GameObject push = Instantiate (power.gameObject, null);
		push.GetComponent<PushPower> ().enabled = true;
		push.GetComponent<PushPower> ().emitted = true;
		push.transform.position = power.transform.position;
		push.GetComponent<Rigidbody> ().isKinematic = false;
		push.GetComponent<Rigidbody> ().AddForce (force);
		ParticleSystem.MainModule pm = push.GetComponent<ParticleSystem> ().main;
		pm.simulationSpace = ParticleSystemSimulationSpace.World;
		push.GetComponent<ParticleSystem> ().Play ();
		ParticleSystem.SubEmittersModule ps = push.GetComponent<ParticleSystem> ().subEmitters;
		ps.enabled = true;
		Destroy (push, 5);
	}
}
