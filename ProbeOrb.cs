using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeOrb : MonoBehaviour {

	public GameObject realmTemplate;
	public bool RealmCreated { get; set;}
	Material shellMaterial;
	Material lightMaterial;

	SphericalRealm realm;
	float dropTimer = 0;
	bool dropped = false;
	Material[] mats;

	int numOfJade = 0;

	// Use this for initialization
	void Start () {
		RealmCreated = false;
		Physics.IgnoreCollision (GetComponent<SphereCollider> (), GameObject.FindGameObjectWithTag ("Player").GetComponent<CapsuleCollider> ());
		mats = GetComponent<MeshRenderer> ().materials;

		shellMaterial = Resources.Load ("Materials/Orbshell", typeof(Material)) as Material;
		lightMaterial = Resources.Load ("Materials/GreenLight", typeof(Material)) as Material;
	}
	
	// Update is called once per frame
	void Update () {
		if (dropped) {
			dropTimer += Time.deltaTime;
			if (dropTimer > 3) {
				dropped = false;
				dropTimer = 0;
				GetComponent<Grabbable> ().BackToEquipment ();
			}
		}

		if (Input.GetKeyDown (KeyCode.N)) {
			UpdateJade (-1);
		}
		if (Input.GetKeyDown (KeyCode.M)) {
			UpdateJade (1);
		}
	}

	public IEnumerator ShowUp(){
		float t = 0;
		Vector3 orgPos = transform.position;
		Transform player = GameObject.FindGameObjectWithTag ("Player").transform;
		Vector3 target = Camera.main.transform.position + player.forward*0.4f - player.up*0.3f;
		while (t < 2) {
			t += Time.deltaTime;
			transform.position = Vector3.Lerp (orgPos, target, t / 2);
			yield return null;
		}
	}

	public void UpdateJade(int num){
		numOfJade += num;
		if (numOfJade < 0)
			numOfJade = 0;
		else if (numOfJade > 4)
			numOfJade = 4;
		
		mats = GetComponent<MeshRenderer> ().materials;
		if (numOfJade == 0) {
			for (int i = 1; i < 6; i++) {
				print ("update!");
				mats[i] = shellMaterial;
			}
			transform.GetChild (0).gameObject.SetActive (false);
		} else {
			for (int i = 1; i < 6; i++) {
				if (i < numOfJade + 2) {
//					print ("update!");
					mats[i] = lightMaterial;
				} else {
//					print ("update!");
					mats[i] = shellMaterial;
				}
			}
			transform.GetChild (0).gameObject.SetActive (true);
		}
		GetComponent<MeshRenderer> ().materials = mats;
	}

	void OnEnable(){
		GetComponent<Grabbable> ().OnGrab += OnGrab;
		GetComponent<Grabbable> ().OnDrop += OnDrop;
		GetComponent<Grabbable> ().OnEquipmentBack += OnEquipmentBack;
		GetComponent<Grabbable> ().OnEquipmentOut += OnEquipmentOut;
	}

	void OnDisable(){
		GetComponent<Grabbable> ().OnGrab -= OnGrab;
		GetComponent<Grabbable> ().OnDrop -= OnDrop;
		GetComponent<Grabbable> ().OnEquipmentBack -= OnEquipmentBack;
		GetComponent<Grabbable> ().OnEquipmentOut -= OnEquipmentOut;
	}

	void OnGrab(){
		FindObjectOfType<GameStateManager> ().orbReceived = true;

		dropped = false;
		dropTimer = 0;
		if (RealmCreated) {
			DestroyRealm ();
		}
		GetComponent<SphereCollider> ().isTrigger = true;
	}

	void OnDrop(){
		dropped = true;
		GetComponent<SphereCollider> ().isTrigger = false;
	}

	void OnEquipmentBack(){
		dropped = false;
		GetComponent<SphereCollider> ().isTrigger = true;
	}

	void OnEquipmentOut(){
	}

	void OnCollisionEnter(Collision col){
		if (!RealmCreated && col.transform.CompareTag("Environment")) {
			dropped = false;
			if (numOfJade > 0) {
				CreateRealm ();
				UpdateJade (-1);
			} else {
				GetComponent<Rigidbody> ().velocity = Vector3.zero;
				GetComponent<Rigidbody> ().isKinematic = true;
				GetComponent<SphereCollider> ().isTrigger = true;
				GetComponent<Grabbable> ().BackToEquipment (1);
			}
		}
	}

	void CreateRealm(){
		FindObjectOfType<GameStateManager> ().realmOpened = true;

		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		GetComponent<Rigidbody> ().isKinematic = true;
		GetComponent<SphereCollider> ().isTrigger = true;
		realm = Instantiate (realmTemplate, transform.position, Quaternion.Euler(Vector3.zero)).GetComponent<SphericalRealm>();
		RealmCreated = true;
	}

	void DestroyRealm(){
		realm.RealmDisappear ();
		RealmCreated = false;
	}
}
