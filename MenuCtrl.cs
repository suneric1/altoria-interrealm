using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCtrl : MonoBehaviour {

	public InventorySystem inventorySystem;
	public SpatialUI alma;
	public GameObject ui;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp ("ButtonX")) {
			inventorySystem.Toggle ();
			if (alma.isOpen)
				alma.Close ();
		}
		if (Input.GetButtonUp ("ButtonY")) {
			alma.Toggle ();
			if (inventorySystem.isOpen)
				inventorySystem.Close ();
		}
		if (Input.GetButtonUp ("ButtonMenu")) {
			ui.SetActive (!ui.activeSelf);
		}
	}
}
