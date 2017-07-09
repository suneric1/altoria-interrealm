using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpot : MonoBehaviour {

	public bool hasSpawned = false;
	public float VacantTime {get;set;}

	// Use this for initialization
	void Start () {
		VacantTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasSpawned) {
			VacantTime += Time.deltaTime;
		}
	}

	public void Occupied(){
		VacantTime = 0;
		hasSpawned = true;
	}

	public void Released(){
		hasSpawned = false;
	}
}
