using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialUI : MonoBehaviour {

	PlayerController playerCtrl;
	public bool isOpen{ get; private set; }

	Transform cam;
	Coroutine transition;

	// Use this for initialization
	void Start () {
		playerCtrl = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		cam = Camera.main.transform;
		isOpen = false;
	}

	// Update is called once per frame
	void Update () {
//		if (!isOpen) {
//		} else if (playerCtrl.PlayerMoving)
//			Close ();
	}

	public void Toggle(){
		if (isOpen) {
			Close ();
		} else {
			Open ();
		}
	}

	public void Close(){
		isOpen = false;
		if (transition != null)
			StopCoroutine (transition);
		transition = StartCoroutine (Transition (cam.position - Vector3.up - cam.forward, Vector3.zero));
	}

	public void Open(){
		isOpen = true;
		transform.rotation = Quaternion.Euler(Vector3.up*cam.eulerAngles.y);
		transform.position = cam.position - Vector3.up - cam.forward;
		transform.localScale = Vector3.zero;
		if (transition != null)
			StopCoroutine (transition);
		transition = StartCoroutine (Transition (cam.position + new Vector3 (cam.forward.x, 0, cam.forward.z).normalized * 2.5f, Vector3.one * 0.01f));
	}

	IEnumerator Transition(Vector3 pos, Vector3 scale){
		if (!isOpen)
			Time.timeScale = 1;
		float t = 0f;
		while(t < 0.5f){
			transform.localPosition = Vector3.Lerp(transform.localPosition, pos, t);
			transform.localScale = Vector3.Lerp(transform.localScale, scale, t);
			t += Time.deltaTime;
			yield return null;
		}
		transform.localPosition = pos;
		transform.localScale = scale;
		transition = null;
		if (isOpen)
			Time.timeScale = 0;
	}
}
