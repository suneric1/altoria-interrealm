using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Temple : MonoBehaviour {

	public ParticleSystem power;
	public Statue[] statues;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.frameCount % 60 == 0) {
			int correctMask = 0;
			for (int i = 0; i < statues.Length; i++) {
				if (statues [i].transform.childCount > 0 && statues [i].transform.GetChild (0).GetComponent<Mask> ().maskType == statues [i].patternType)
					correctMask++;
			}
			if (correctMask >= 3)
				StartCoroutine (OpenGate ());
		}
	}

	IEnumerator OpenGate(){
		FindObjectOfType<GameStateManager> ().doorOpen = true;
		GetComponent<Animator> ().SetTrigger ("DoorOpen");
		power.Play ();
		yield return new WaitForSeconds (15);
		SceneManager.LoadScene (0);
	}
}
