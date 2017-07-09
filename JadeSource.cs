using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeSource : MonoBehaviour {

	public int HP = 5;
	public int jadeAmount;
	public GameObject shatter;
	public ParticleSystem particle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Hit(Vector3 pos){
		HP--;
		particle.transform.position = pos;
		particle.Play ();
		if (HP <= 0)
			Crack ();
	}

	public void Crack(){
		FindObjectOfType<GameStateManager> ().jadeCollected = true;
		Instantiate (shatter, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
