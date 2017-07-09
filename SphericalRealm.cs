using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalRealm : MonoBehaviour {

	ParticleSystem ps;
	ParticleSystem.Particle[] particles;
	List<LineRenderer> lineRenderers = new List<LineRenderer> ();
	float lifeTimer = 0;
	bool effective = true;
	ProbeOrb orb;

	public int maxLineCount = 100;
	public LineRenderer lineTemplate;

	// Use this for initialization
	void Start () {
		orb = GameObject.Find ("ProbeOrb").GetComponent<ProbeOrb> ();
		ps = GetComponent<ParticleSystem> ();
		particles = new ParticleSystem.Particle[ps.main.maxParticles];
//		if (ps.main.simulationSpace == ParticleSystemSimulationSpace.Local) {
//			lineTemplate.useWorldSpace = false;
//		} else if (ps.main.simulationSpace == ParticleSystemSimulationSpace.World) {
//			lineTemplate.useWorldSpace = true;
//		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up * Time.deltaTime);
		lifeTimer += Time.deltaTime;
		if (effective && lifeTimer > 5) {
			orb.RealmCreated = false;
			orb.GetComponent<Grabbable> ().BackToEquipment ();
			RealmDisappear ();
		}

		DrawLines ();
	}

	public void RealmDisappear(){
		effective = false;
		GetComponent<Animator> ().SetTrigger ("disappear");
	}

	void Destroy(){
		GameObject.Destroy (gameObject);
	}

	void DrawLines(){

		int particleCount = ps.particleCount;
		ps.GetParticles (particles);

		for (int i = 0; i < lineRenderers.Count; i++) {
			lineRenderers [i].enabled = false;
		}

		//		for (int i = 0; i < particleCount; i++) {
		//			if (Vector3.SqrMagnitude (particles [i].position - transform.position) > 9) {
		//				particles [i].position = transform.position + (particles [i].position - transform.position).normalized * 3;
		//				print (Vector3.SqrMagnitude (particles [i].position - transform.position));
		//			}
		//		}

		int lrIndex = 0;
		for (int i = 0; i < particleCount; i++) {
			for (int j = i + 1; j < particleCount; j++) {
				if (lrIndex < maxLineCount && Vector3.Distance (particles [i].position, particles [j].position) < 0.8f) {
					if (lrIndex < lineRenderers.Count) {
						lineRenderers [lrIndex].SetPosition (0, particles [i].position);
						lineRenderers [lrIndex].SetPosition (1, particles [j].position);
						lineRenderers [lrIndex].enabled = true;
					} else {
						LineRenderer lr = Instantiate (lineTemplate, transform);
						lr.SetPosition (0, particles [i].position);
						lr.SetPosition (1, particles [j].position);
						lr.startColor = particles [i].GetCurrentColor (ps);
						lr.endColor = particles [j].GetCurrentColor (ps);
						lineRenderers.Add (lr);
					}
					lrIndex++;
				}
			}
		}
	}
}
