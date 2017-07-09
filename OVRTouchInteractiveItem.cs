using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRTouchInteractiveItem : MonoBehaviour {

    public Controller LController;
    public Controller RController;
    public delegate void Action();
    public event Action OnGrab;
    public event Action OnDrop;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Grab()
    {
        if (OnGrab != null) OnGrab();
    }

    public void Drop()
    {
        if (OnDrop != null) OnDrop();
    }
}
