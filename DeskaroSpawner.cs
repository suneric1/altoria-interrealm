using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskaroSpawner : MonoBehaviour {

	public SpawnSpot[] spots;
	public GameObject deskaro;

	int deskaroCount = 0;
	int typeIndex = 0;

	// Use this for initialization
	void Start () {
		spots = FindObjectsOfType<SpawnSpot> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.frameCount % 60 == 0) {
			deskaroCount = GameObject.FindGameObjectsWithTag ("Enemy").Length;
			if (deskaroCount < 6) {
				int spotIndex = Random.Range (0, spots.Length);
				if (spots [spotIndex].VacantTime > 4) {
					Deskaro des = Instantiate (deskaro, spots [spotIndex].transform.position + Vector3.forward * 4, Quaternion.Euler (Vector3.zero)).GetComponent<Deskaro> ();
					spots [spotIndex].Occupied ();

					des.spawnSpot = spots [spotIndex];
					des.pathNodes = new Vector3[] {spots [spotIndex].transform.position + Vector3.forward * 4,
						spots [spotIndex].transform.position + Vector3.right * 4,
						spots [spotIndex].transform.position - Vector3.forward * 4,
						spots [spotIndex].transform.position - Vector3.right * 4
					};
					des.mask.maskType = (Altoria.PatternType)typeIndex;
					typeIndex++;
					if (typeIndex > 2)
						typeIndex = 0;
				}
			}
		}
	}
}
