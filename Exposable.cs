using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exposable : MonoBehaviour {

	public bool Exposed { get; set; }
	public bool OutOfRealm { get; set; }
	public Renderer[] renderers;
	public Collider col;

	Coroutine fading;

	// Use this for initialization
	void Start () {
		Exposed = false;
		OutOfRealm = false;
		col.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.F))
			StartCoroutine (FadeIn ());
		if (Input.GetKeyDown (KeyCode.T))
			StartCoroutine (FadeOut ());
		if (Input.GetKeyDown (KeyCode.R))
			StartCoroutine (Reveal ());
		
	}

	public void PushOutRealm(){
		if (GetComponent<Deskaro> ())
			FindObjectOfType<GameStateManager> ().deskaroOut = true;
		
		OutOfRealm = true;
		StartCoroutine (Reveal ());
	}

	public void Expose(){
		if (GetComponent<Deskaro> ())
			FindObjectOfType<GameStateManager> ().deskaroExposed = true;

		Exposed = true;
		col.isTrigger = false;
		if (fading != null)
			StopCoroutine (fading);
		fading = StartCoroutine (FadeIn ());
	}

	public void Hide(){
		Exposed = false;
		col.isTrigger = true;
		if (fading != null)
			StopCoroutine (fading);
		fading = StartCoroutine (FadeOut ());
	}

	void OnTriggerEnter(Collider other){
		if (!OutOfRealm && other.CompareTag ("Realm")) {
			Expose ();
		}
	}

	void OnTriggerExit(Collider other){
		if (!OutOfRealm && other.CompareTag ("Realm")) {
			Hide ();
		}
	}

	public IEnumerator FadeIn(){
//		print ("In!");
		int a = 0;
		float t = 0;
		float duration = 0.2f;
		Color32[] colors = new Color32[renderers.Length];
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = renderers [i].material.color;
		}
		while (t < duration) {
			a=(int)(a+60*Time.deltaTime/duration);
			if (a > 60)
				a = 60;
			for (int i = 0; i < colors.Length; i++) {
				colors [i] = new Color32 (colors [i].r, colors [i].g, colors [i].b, (byte)a);
				renderers [i].material.color = colors [i];
			}
			t += Time.deltaTime;
			yield return null;
		}
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = new Color32 (colors [i].r, colors [i].g, colors [i].b, 60);
			renderers [i].material.color = colors [i];
		}
	}

	public IEnumerator FadeOut(){
//		print ("Out!");
		int a = 60;
		float t = 0;
		float duration = 0.2f;
		Color32[] colors = new Color32[renderers.Length];
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = renderers [i].material.color;
		}
		while (t < duration) {
			a=(int)(a-60*Time.deltaTime/duration);
			if (a < 0)
				a = 0;
			for (int i = 0; i < colors.Length; i++) {
				colors [i] = new Color32 (colors [i].r, colors [i].g, colors [i].b, (byte)a);
				renderers [i].material.color = colors [i];
			}
			t += Time.deltaTime;
			yield return null;
		}
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = new Color32 (colors [i].r, colors [i].g, colors [i].b, 0);
			renderers [i].material.color = colors [i];
		}
	}

	IEnumerator Reveal(){
		int a = 60;
		float t = 0;
		float duration = 0.5f;
		Color32[] colors = new Color32[renderers.Length];
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = renderers [i].material.color;
		}
		while (t < duration) {
			a=(int)(a+195*Time.deltaTime/duration);
			if (a > 255)
				a = 255;
			for (int i = 0; i < colors.Length; i++) {
				colors [i] = new Color32 (colors [i].r, colors [i].g, colors [i].b, (byte)a);
				renderers [i].material.color = colors [i];
			}
			t += Time.deltaTime;
			yield return null;
		}
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = new Color32 (colors [i].r, colors [i].g, colors [i].b, 255);
			renderers [i].material.color = colors [i];
		}
		col.isTrigger = false;
	}
}
