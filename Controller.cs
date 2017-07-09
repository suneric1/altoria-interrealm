using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public OVRInput.Controller controller;
	public float grabRadius;
	public LayerMask grabMask;
	public LayerMask slotMask;
	public Grabbable GrabbedObject { get; set;}
	public Transform line;
	public Transform bag;
    //public delegate void Action();
    //public event Action OnGrab;

	//    private string side;
	InventorySystem inventorySystem;
	private Grabbable grabbable;
	private ItemSlot itemSlot;
	private bool grabbing;
	private bool aroundBag = false;

	public string Side { get; private set;}

    void Start()
	{
		if (controller == OVRInput.Controller.LTouch) Side = "L";
		else Side = "R";
		inventorySystem = GameObject.Find ("InventorySystem").GetComponent<InventorySystem> (); 
    }

    // Update is called once per frame
    void Update () {
        transform.localPosition = OVRInput.GetLocalControllerPosition(controller);
		transform.localRotation = OVRInput.GetLocalControllerRotation(controller);

		if (GrabbedObject == null)
			DetectGrabbable ();
		else if (GrabbedObject.isSlotItem) {
			DetectSlot ();
			DetectBag ();
		}

        if (!grabbing && Input.GetAxis(Side + "HandTrigger") == 1)
        {
            grabbing = true;
			if(grabbable != null && grabbable.enabled)
				grabbable.Unhighlight ();
            if (GrabbedObject == null)
                GrabObject();
			else if(GrabbedObject.allowOffhandGrab)
                DropObject();
        }
        if (grabbing && Input.GetAxis(Side + "HandTrigger") < 1)
        {
            grabbing = false;
			if (GrabbedObject != null && !GrabbedObject.allowOffhandGrab)
            {
                DropObject();
            }
        }
	}

	void DetectBag(){
		aroundBag = false;

		RaycastHit[] hits = Physics.SphereCastAll(line.position, grabRadius, line.forward, 0.1f);

		if (hits.Length > 0) {
			for (int i = 0; i < hits.Length; i++) {
				if (hits [i].transform == bag) {
					aroundBag = true;
//					print ("found bag");
				}
			}
		}
	}

	void DetectSlot(){
		RaycastHit hit;

		if (Physics.Raycast (new Ray (line.position, line.forward), out hit, 10f, slotMask)) {
			if (!itemSlot || itemSlot && itemSlot.transform != hit.transform) {
				print ("found new slot");
				if(itemSlot && itemSlot.Empty)
					itemSlot.Unhighlight ();
				itemSlot = hit.transform.GetComponent<ItemSlot> ();
				if (itemSlot && itemSlot.Empty) {
					itemSlot.Highlight ();
				}
			}
		} else if (itemSlot) {
			itemSlot.Unhighlight ();
			itemSlot = null;
		}

		Debug.DrawRay (line.position, line.forward, Color.red);
	}

	void DetectGrabbable(){
		RaycastHit hit;
		RaycastHit[] hits = Physics.SphereCastAll(line.position, grabRadius, line.forward, 0f, grabMask);

		if (hits.Length > 0)
		{
			int closestHit = 0;
			for(int i = 1; i < hits.Length; i++)
			{
				//				print(hits[i].distance);
				if (hits[i].distance < hits[closestHit].distance) closestHit = i;
			}

			if(grabbable == null || grabbable != null && grabbable.transform != hits[closestHit].transform)
			{
				if (grabbable != null && grabbable.enabled)
					grabbable.Unhighlight ();
				grabbable = hits[closestHit].transform.GetComponent<Grabbable> ();
				if (grabbable != null && grabbable.enabled) 
					grabbable.Highlight ();
			}
		} else if (Physics.Raycast (new Ray (line.position, line.forward), out hit, 10f, grabMask)) {
			if (grabbable == null || grabbable != null && grabbable.transform != hit.transform) {
				if (grabbable != null && grabbable.enabled)
					grabbable.Unhighlight ();
				grabbable = hit.transform.GetComponent<Grabbable> ();
				if (grabbable != null && grabbable.enabled) {
					grabbable.Highlight ();
					//						print ("hovered!");
				}
			}
		} else if (grabbable != null && grabbable.enabled) {
			grabbable.Unhighlight ();
			grabbable = null;
		}
//		print (grabbable.transform);

		Debug.DrawRay (line.position, line.forward, Color.red);
	}

    public void GrabObject()
    {
		if (grabbable != null && grabbable.enabled) {
//			print ("grab!");
			if (grabbable.GrabbedBy (this)) {
				GrabbedObject = grabbable;
				grabbable = null;
			} else
				print ("grab failed");
		}
    }

    public void DropObject()
    {
//		print("drop!");
		if (GrabbedObject.isSlotItem) {
			if (itemSlot && itemSlot.Empty)
				GrabbedObject.slot = itemSlot.transform;
			else if (aroundBag) {
				Transform emptySlot = inventorySystem.GetEmptySlot ();
				GrabbedObject.slot = emptySlot;
				if (emptySlot != null)
					bag.GetComponent<AudioSource> ().Play ();
			} else
				GrabbedObject.slot = null;
		}
		if(GrabbedObject.DroppedBy (this))
        	GrabbedObject = null;
    }
}
