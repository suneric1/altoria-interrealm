using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeShatter : MonoBehaviour {

	Transform player;
	public GameObject JadePower;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		for (int i = 1; i < transform.childCount; i++) {
			transform.GetChild (i).GetComponent<Rigidbody> ().AddForce ((player.position - transform.position)*100);
			Physics.IgnoreCollision (transform.GetChild (i).GetComponent<MeshCollider> (), player.GetComponent<CapsuleCollider> ());
		}
		StartCoroutine (FadeOut ());
		GameObject jd = Instantiate (JadePower, transform.position, JadePower.transform.rotation);
		Destroy (jd, 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator FadeOut(){
		int a = 255;
		float t = 0;
		float duration = 1f;

		Material[] mats = new Material[transform.childCount];
		for (int i = 0; i < mats.Length; i++) {
			mats [i] =transform.GetChild (i).GetComponent<MeshRenderer>().material;
		}

		Color32[] colors = new Color32[mats.Length];
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = mats [i].color;
		}

		yield return new WaitForSeconds (2);

		while (t < duration) {
			a-=(int)(255*Time.deltaTime/duration);
			if (a < 0)
				a = 0;
			for (int i = 0; i < colors.Length; i++) {
				colors [i] = new Color32 (colors [i].r, colors [i].g, colors [i].b, (byte)a);
				mats [i].color = colors [i];
			}
			t += Time.deltaTime;
			yield return null;
		}
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = new Color32 (colors [i].r, colors [i].g, colors [i].b, 0);
			mats [i].color = colors [i];
		}
		Destroy (gameObject);
	}
}
