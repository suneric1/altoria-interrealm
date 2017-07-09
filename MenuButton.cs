using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {

	public int btnType=0;
	Coroutine transition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Enter(){
		if (btnType == 0)
			SceneManager.LoadScene (1);
		else
			Application.Quit ();
	}

	public void Highlight(){
		GetComponent<Image> ().color = new Color (GetComponent<Image> ().color.r, GetComponent<Image> ().color.g, GetComponent<Image> ().color.b, 0.7f);
		if (transition != null)
			StopCoroutine (transition);
		transition = StartCoroutine (Scale (Vector3.one * 1.1f));
	}

	public void Unhighlight(){
		GetComponent<Image> ().color = new Color (GetComponent<Image> ().color.r, GetComponent<Image> ().color.g, GetComponent<Image> ().color.b, 1f);
		if (transition != null)
			StopCoroutine (transition);
		transition = StartCoroutine (Scale (Vector3.one));
	}

	IEnumerator Scale(Vector3 scale){
		float t = 0f;
		while(t < 0.5f){
			transform.localScale = Vector3.Lerp(transform.localScale, scale, t);
			t += Time.deltaTime;
			yield return null;
		}
		transform.localScale = scale;
		transition = null;
	}
}
