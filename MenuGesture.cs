using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGesture : MonoBehaviour {

	Animator anim;
	int Idle = Animator.StringToHash("Idle");
	int Point = Animator.StringToHash("Point");
	int GrabLarge = Animator.StringToHash("GrabLarge");
	int GrabSmall = Animator.StringToHash("GrabSmall");
	int GrabStickUp = Animator.StringToHash("GrabStickUp");
	int GrabStickFront = Animator.StringToHash("GrabStickFront");
	int ThumbUp = Animator.StringToHash("ThumbUp");
	int Fist = Animator.StringToHash("Fist");
	int Gun = Animator.StringToHash("Gun");
	int GunShoot = Animator.StringToHash("GunShoot");
	int PushButton = Animator.StringToHash("PushButton");
	int Spread = Animator.StringToHash("Spread");
	int MiddleFinger = Animator.StringToHash("MiddleFinger");
	int Peace = Animator.StringToHash("Peace");
	int OK = Animator.StringToHash("OK");
	int Phone = Animator.StringToHash("Phone");
	int Rock = Animator.StringToHash("Rock");
	int Natural = Animator.StringToHash("Natural");
	int Number3 = Animator.StringToHash("Number3");
	int Number4 = Animator.StringToHash("Number4");
	int Number3V2 = Animator.StringToHash("Number3V2");
	int PressTriggerViveController = Animator.StringToHash("PressTriggerViveController");

	float lIndex, rIndex, lGrip, rGrip;
	bool rThumbTouched, lThumbTouched, rIndexTouched, lIndexTouched;
	MenuController ctrl;
	public string side;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		ctrl = GetComponent<MenuController> ();
//		side = ctrl.Side;
	}

	// Update is called once per frame
	void Update () {
//		print (side);
		lIndex = Input.GetAxisRaw ("LIndexTrigger");
		rIndex = Input.GetAxisRaw ("RIndexTrigger");
		lGrip = Input.GetAxisRaw ("LHandTrigger");
		rGrip = Input.GetAxisRaw ("RHandTrigger");
		rThumbTouched = Input.GetButton ("TouchA") || Input.GetButton ("TouchB") || Input.GetButton ("RThumbRest");
		lThumbTouched = Input.GetButton ("TouchX") || Input.GetButton ("TouchY") || Input.GetButton ("LThumbRest");
		rIndexTouched = Input.GetButton ("RIndexTouch");
		lIndexTouched = Input.GetButton ("LIndexTouch");

		if (side == "R") {
			if (rGrip == 1) {
				if (rThumbTouched) {
					if (rIndexTouched)
						anim.SetTrigger (Fist);
					else
						anim.SetTrigger (Point);
				} else {
					if (rIndexTouched)
						anim.SetTrigger (ThumbUp);
					else
						anim.SetTrigger (Gun);
				}
			} else
				anim.SetTrigger (Natural);
		} 
		else if (side == "L") {
			if (lGrip == 1) {
				if (lThumbTouched) {
					if (lIndexTouched)
						anim.SetTrigger (Fist);
					else
						anim.SetTrigger (Point);
				} else {
					if (lIndexTouched)
						anim.SetTrigger (ThumbUp);
					else
						anim.SetTrigger (Gun);
				}
			} else
				anim.SetTrigger (Natural);
		}
	}
}
