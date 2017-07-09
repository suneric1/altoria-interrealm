using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {

	Image bg;
	public Altoria.ItemType itemType;
	public Vector3 posOffset;
	public Vector3 rotOffset;
	public event Action OnHighlight;
	public event Action OnUnhighlight;
	public bool Empty{ get; private set; }

	// Use this for initialization
	void Start () {
		bg = GetComponent<Image> ();
		if (transform.childCount > 0 && transform.GetChild (0).GetComponent<Grabbable> ())
			Empty = false;
		else
			Empty = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Occupied(){
		Empty = false;
	}

	public void Released(){
		Empty = true;
	}

	public void Unhighlight(){
//		print ("unhighlighted");
		if(OnUnhighlight!=null) OnUnhighlight();
		if (bg)
			bg.color = new Color (bg.color.r, bg.color.g, bg.color.g, 0.75f);
	}

	public void Highlight(){
//		print ("highlighted");
		if(OnHighlight!=null) OnHighlight();
		if(bg)
			bg.color = new Color (bg.color.r, bg.color.g, bg.color.g, 1);
	}
}
