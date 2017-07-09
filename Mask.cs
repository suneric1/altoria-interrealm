using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour {

	//	Inventory inventory;
	public Killable deskaro;
	public Grabbable grabbable;
	public Altoria.PatternType maskType;

	public bool detached = false;
	public bool purified = false;
	float timeInWater = 0;
	bool inWater = false;
	Renderer rend;
	Material maskMat_0, maskMat_1, maskMat_2, maskMat_3;

	// Use this for initialization
	void Start(){
//		inventory = GameObject.FindObjectOfType<Inventory> ();
		rend = GetComponent<Renderer>();
		grabbable.itemType = Altoria.ItemType.Mask;
		maskMat_0 = Resources.Load ("Materials/MaskBlank") as Material;
		maskMat_1 = Resources.Load ("Materials/Mask_1") as Material;
		maskMat_2 = Resources.Load ("Materials/Mask_2") as Material;
		maskMat_3 = Resources.Load ("Materials/Mask_3") as Material;
	}

	void OnEnable () {
		grabbable.OnGrab += OnGrab;
		grabbable.OnDrop += OnDrop;
	}
	
	// Update is called once per frame
	void Update () {
		if (!purified) {
			if (inWater) {
				timeInWater += Time.deltaTime / 2;
				if (timeInWater > 1) {
					timeInWater = 1;
					purified = true;
				}
				print ("inwater");
			} else {
				timeInWater -= Time.deltaTime / 2;
				if (timeInWater < 0)
					timeInWater = 0;
			}
			rend.material.SetFloat ("_Blend1", timeInWater);
		}
	}

	void OnGrab(){
		//		mobAnimator.enabled = false;
		deskaro.OnDeath += OnDeath;
		if (grabbable.allowDrop) {
			GetComponent<BoxCollider> ().isTrigger = true;
		}
	}

	void OnDrop(){
		//		mobAnimator.enabled = true;
		deskaro.OnDeath -= OnDeath;
		if (grabbable.allowDrop) {
			GetComponent<BoxCollider> ().isTrigger = false;
		}
	}

	void OnDeath(){
		FindObjectOfType<GameStateManager> ().maskOne = true;
		FindObjectOfType<GameStateManager> ().maskCount++;
		if (FindObjectOfType<GameStateManager> ().maskCount > 2)
			FindObjectOfType<GameStateManager> ().maskThree = true;

		detached = true;
		switch (maskType) {
		case Altoria.PatternType.SunMoon:
			GetComponent<Renderer> ().material = maskMat_1;
			break;
		case Altoria.PatternType.TimeSpace:
			GetComponent<Renderer> ().material = maskMat_2;
			break;
		case Altoria.PatternType.War:
			GetComponent<Renderer> ().material = maskMat_3;
			break;
		}
		GameObject maskInstance = Instantiate (gameObject, transform.parent);
		grabbable.grabber.GrabbedObject = maskInstance.GetComponent<Grabbable> ();
		maskInstance.GetComponent<Grabbable> ().dropBack = false;
		maskInstance.GetComponent<Grabbable> ().allowDrop = true;
		maskInstance.GetComponent<Grabbable> ().isSlotItem = true;
		Destroy (gameObject);
	}

	void OnTriggerEnter(Collider col){
		if (col.name == "WaterCollider" && !grabbable.inSlot) {
			inWater = true;
		}
	}

	void OnTriggerExit(Collider col){
		if (col.name == "WaterCollider") {
			inWater = false;
		}
	}
}
