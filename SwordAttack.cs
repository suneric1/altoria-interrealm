using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour {

	public int swordDamage = 5;
	public AudioSource swingAttack;
	public AudioSource swingStone;
	public AudioSource unsheath;

	EquipmentHolder holder;
	Rigidbody rgbd;
	OVRHapticsClip clip;

	// Use this for initialization
	void Start () {
		rgbd = GetComponent<Rigidbody> ();
		clip = new OVRHapticsClip (10);
		for (int i = 0; i < 10; i++) {
			clip.Samples [i] = i % 2 == 0 ? (byte)0 : (byte)150;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable(){
		GetComponent<Grabbable> ().OnGrab += OnGrab;
		GetComponent<Grabbable> ().OnDrop += OnDrop;
	}

	void OnDisable(){
		GetComponent<Grabbable> ().OnGrab -= OnGrab;
		GetComponent<Grabbable> ().OnDrop -= OnDrop;
	}

	void OnGrab(){
		unsheath.Play();
	}

	void OnDrop(){
	}

	void OnTriggerEnter(Collider other){
		if (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude > 2.5f || OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude > 2.5f) {
			if (other.CompareTag ("Enemy") && other.GetComponent<Exposable>().OutOfRealm && !other.GetComponent<Killable> ().isDead) {
//			print ("hey!!");
//				other.GetComponent<Animator> ().SetTrigger ("Hurt");
				other.GetComponent<Killable> ().TakeDamage (swordDamage);

				swingAttack.Play ();
				OVRHaptics.RightChannel.Preempt (new OVRHapticsClip (clip.Samples, clip.Samples.Length));
			} else if (other.GetComponent<JadeSource> ()) {
				swingStone.Play ();
				other.GetComponent<JadeSource> ().Hit (other.ClosestPoint(transform.position));
			} else if (other.CompareTag("Environment")) {
				swingStone.Play ();
			}
		}
	}
}
