using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour {

	public GameObject maskHolder;
	public Altoria.PatternType patternType;
	ItemSlot slot;

	// Use this for initialization
	void Start () {
		slot = GetComponent<ItemSlot> ();
		slot.OnHighlight += OnHighlight;
		slot.OnUnhighlight += OnUnhighlight;
	}

	void OnHighlight ()
	{
		maskHolder.SetActive (true);
	}

	void OnUnhighlight ()
	{
		maskHolder.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
