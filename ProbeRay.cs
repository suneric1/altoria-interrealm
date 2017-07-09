using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeRay : MonoBehaviour {

	public LineRenderer line;
	public LineRenderer line2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		line2.SetPosition (0, line2.transform.position);
//		line2.SetPosition (0, line2.transform.position);
		int closestIndex = -1;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		if (enemies.Length > 0) {
			closestIndex = 0;
			for (int i = 1; i < enemies.Length; i++) {
				if (Vector3.SqrMagnitude (transform.position - enemies [i].transform.position) < Vector3.SqrMagnitude (transform.position - enemies [closestIndex].transform.position)) {
					closestIndex = i;
				}
			}
//			line2.SetPosition (1, line2.transform.position + (enemies [closestIndex].transform.position-line2.transform.position).normalized*2);
			Quaternion from = Quaternion.Euler(line2.transform.forward.normalized);
			Quaternion to = Quaternion.Euler ((enemies [closestIndex].transform.position - line2.transform.parent.position).normalized);
			float angle = Quaternion.Angle (from, to);
//			line2.SetPosition (1, line2.transform.position + to * line2.transform.forward * 2);
			line2.SetPosition (1, line2.transform.position + Vector3.RotateTowards(line2.transform.forward.normalized, enemies [closestIndex].transform.position - line2.transform.parent.position, angle/2, 2)*2);
			//			line.transform.localRotation = Quaternion.RotateTowards (from, to, 20);
		}
	}

}
