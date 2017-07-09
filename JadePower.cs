using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadePower : MonoBehaviour {

	Transform orb;

	ParticleSystem ps;
	ParticleSystem.Particle[] particles;
	bool powerCharged = false;

	// Use this for initialization
	void Start () {
		orb = GameObject.Find ("ProbeOrb").transform;
		ps = GetComponent<ParticleSystem> ();
		particles = new ParticleSystem.Particle[ps.main.maxParticles];
		ps.GetParticles (particles);
	}
	
	// Update is called once per frame
	void Update () {
		ps.GetParticles (particles);
		for (int i = 0; i < ps.particleCount; i++) {
			if ((orb.position - particles [i].position).sqrMagnitude > 0.001f) {
				particles [i].velocity += (orb.position - particles [i].position).normalized / (orb.position - particles [i].position).sqrMagnitude;
				particles [i].velocity = particles [i].velocity.normalized * 2.5f;
			} else {
				particles [i].velocity = particles [i].velocity.normalized * 0.1f;
				if (!powerCharged) {
					orb.GetComponent<ProbeOrb> ().UpdateJade (4);
					powerCharged = true;
				}
			}
		}
		ps.SetParticles (particles, ps.particleCount);
	}
}
