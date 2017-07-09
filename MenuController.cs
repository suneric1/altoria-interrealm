using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	public OVRInput.Controller controller;
	public LayerMask menuMask;
	public Transform line;

	private bool grabbing;
	MenuButton btn;

	public string Side { get; private set;}

	void Start()
	{
		if (controller == OVRInput.Controller.LTouch) Side = "L";
		else Side = "R";
	}

	// Update is called once per frame
	void Update () {
		transform.localPosition = OVRInput.GetLocalControllerPosition(controller);
		transform.localRotation = OVRInput.GetLocalControllerRotation(controller);

		if (!grabbing && Input.GetAxis(Side + "HandTrigger") == 1)
		{
			grabbing = true;
			if (btn)
				btn.Enter ();
		}
		if (grabbing && Input.GetAxis(Side + "HandTrigger") < 1)
		{
			grabbing = false;
		}
		DetectMenu ();
	}

	void DetectMenu(){
		RaycastHit hit;

		if (Physics.Raycast (new Ray (line.position, line.forward), out hit, 100f, menuMask)) {
			if (!btn || btn && btn.transform != hit.transform) {
				print ("found new button");
				if(btn)
					btn.Unhighlight ();
				btn = hit.transform.GetComponent<MenuButton> ();
				if (btn) {
					btn.Highlight ();
				}
			}
		} else if (btn) {
			btn.Unhighlight ();
			btn = null;
		}

		Debug.DrawRay (line.position, line.forward, Color.red);
	}
}