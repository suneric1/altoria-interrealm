using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour {

    public Vector3 grabPositionOffset;
    public Vector3 grabRotationOffset;
    public bool allowOffhandGrab;
	public Controller grabber{ get; set;}
	public bool allowDrop = true;
	public bool dropBack;
	public bool isEquipmentItem;
	public Transform equipmentHolder;
	public enum GrabGesture {GrabLarge, HoldSword};
	public GrabGesture gesture;
	public bool isSlotItem;
	public Transform slot;
	public Vector3 slotOffset;
	public Vector3 slotRotation;

	public Altoria.ItemType itemType{ get; set; }
	public Transform orgParent{ get; private set; }
	public Vector3 orgPos{ get; private set; }
	public Vector3 orgRot{ get; private set; }

//	public delegate void Action();
	public event Action OnGrab;
	public event Action OnDrop;
	public event Action OnEquipmentOut;
	public event Action OnEquipmentBack;
	public event Action OnSlotOut;
	public event Action OnSlotIn;

	Material originMaterial;
	bool highlighted = false;
	bool aroundHolder = true;
	bool inHolder;
	bool moving = false;
	public bool inSlot = false;
	Coroutine movingCoroutine;
	Transform player;
	Transform inventorySystem;

    // Use this for initialization
    void Start () {

		inventorySystem = GameObject.Find ("InventorySystem").transform; 

		if (GetComponent<MeshRenderer> ())
			originMaterial = GetComponent<MeshRenderer> ().material;
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		Physics.IgnoreCollision (player.GetComponent<Collider> (), GetComponent<Collider> ());
		if (isEquipmentItem)
			inHolder = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string GetGesture(){
		switch (gesture) {
		case GrabGesture.GrabLarge:
			return "GrabLarge";
		case GrabGesture.HoldSword:
			return "PressTriggerViveController";
		default:
			return "GrabLarge";
		}
	}

	public void Highlight(){
		if (!grabber && GetComponent<shaderGlow> () && !highlighted) {
			GetComponent<shaderGlow> ().lightOn ();
			highlighted = true;
		}
	}

	public void Unhighlight(){
		if (!grabber && GetComponent<shaderGlow> () && highlighted) {
			GetComponent<shaderGlow> ().lightOff ();
			highlighted = false;
		}
	}

	public bool GrabbedBy(Controller controller){
		if (moving)
			return false;
		if (OnGrab != null)
			OnGrab ();
		if (isEquipmentItem && inHolder) {
			inHolder = false;
			equipmentHolder.GetComponent<EquipmentHolder> ().Show ();
			if (OnEquipmentOut != null)
				OnEquipmentOut ();
		}
		if (grabber) {
			grabber.GrabbedObject = null;
		}
		if (dropBack) {
			if (!grabber) {
				orgParent = transform.parent;
				orgPos = transform.localPosition;
				orgRot = transform.localEulerAngles;
			}
		}
		if (slot && inSlot) {
			slot.GetComponent<ItemSlot> ().Released ();
		}
		inSlot = false;
		grabber = controller;
		GetComponent<Rigidbody> ().isKinematic = true;
		transform.parent = controller.transform;
		if (movingCoroutine != null)
			StopCoroutine (movingCoroutine);
		movingCoroutine = StartCoroutine (MoveTo (grabPositionOffset, grabRotationOffset));
		return true;
	}

	public bool DroppedBy(Controller controller){
		if (dropBack) {
			if (OnDrop != null) OnDrop ();
			grabber = null;
			transform.parent = orgParent;
			if (movingCoroutine != null) {
				StopCoroutine (movingCoroutine);
				moving = false;
			}
			movingCoroutine = StartCoroutine (MoveTo (orgPos, orgRot));
			return true;
		} else if (isEquipmentItem && aroundHolder) {
			if (OnDrop != null) OnDrop ();
			if (movingCoroutine != null) {
				StopCoroutine (movingCoroutine);
				moving = false;
			}
			grabber = null;
			BackToEquipment ();
			return true;
		}  else if (isSlotItem && slot) {
			if (OnDrop != null) OnDrop ();
			if (movingCoroutine != null) {
				StopCoroutine (movingCoroutine);
				moving = false;
			}
			GetComponent<Collider> ().isTrigger = true;
			grabber = null;
			MoveToSlot ();
			return true;
		} else if (allowDrop) {
			if (OnDrop != null) OnDrop ();
			if (movingCoroutine != null) {
				StopCoroutine (movingCoroutine);
				moving = false;
			}
			grabber = null;
			GetComponent<Rigidbody> ().isKinematic = false;
			GetComponent<Rigidbody> ().velocity = (player.rotation * OVRInput.GetLocalControllerVelocity (controller.GetComponent<Controller> ().controller)) * 2;

			transform.parent = null;
			return true;
		}
		return false;
	}

	public void MoveToSlot(float delay = 0){
		if (OnSlotIn != null)
			OnSlotIn ();
		ItemSlot itemSlot = slot.GetComponent<ItemSlot> ();
		itemSlot.Unhighlight ();
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		GetComponent<Rigidbody>().isKinematic = true;
		transform.parent = slot;
		if (inventorySystem.localScale == Vector3.zero && slot.parent.parent == inventorySystem) {
			transform.localScale *= 100;
		}
		itemSlot.Occupied ();
		inSlot = true;
		if (movingCoroutine != null)
			StopCoroutine (movingCoroutine);
		if(itemSlot.itemType == Altoria.ItemType.Default)
			movingCoroutine = StartCoroutine (MoveTo (slotOffset, slotRotation, delay));
		else if(itemSlot.itemType == itemType)
			movingCoroutine = StartCoroutine (MoveTo (itemSlot.posOffset, itemSlot.rotOffset, delay));
	}

	public void BackToEquipment(float delay = 0){
		if (OnEquipmentBack != null)
			OnEquipmentBack ();
		equipmentHolder.GetComponent<EquipmentHolder> ().Hide ();
		inHolder = true;
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
		GetComponent<Rigidbody>().isKinematic = true;
		transform.parent = equipmentHolder.parent;
		if (movingCoroutine != null)
			StopCoroutine (movingCoroutine);
		movingCoroutine = StartCoroutine (MoveTo (equipmentHolder.localPosition, equipmentHolder.localEulerAngles, delay));
	}

	IEnumerator MoveTo(Vector3 pos, Vector3 rot, float delay = 0){
		moving = true;
		float t = 0f;
		if (delay > 0)
			yield return new WaitForSeconds (delay);
		while(t < 0.5f){
//			print ("moving");
			transform.localPosition = Vector3.Lerp(transform.localPosition, pos, t);
			transform.localRotation = Quaternion.RotateTowards (transform.localRotation, Quaternion.Euler (rot), Time.deltaTime * 720);
			t += Time.deltaTime;
			yield return null;
		}
		transform.localPosition = pos;
		transform.localRotation = Quaternion.Euler (rot);
		moving = false;
	}

	void OnTriggerEnter(Collider other){
		if (grabber != null && isEquipmentItem && other.transform == equipmentHolder) {
			aroundHolder = true;
			equipmentHolder.GetComponent<EquipmentHolder> ().Highlight ();
		}
	}

	void OnTriggerExit(Collider other){
		if (grabber != null && isEquipmentItem && other.transform == equipmentHolder) {
			aroundHolder = false;
			equipmentHolder.GetComponent<EquipmentHolder> ().Unhighlight ();
		}
	}
}
