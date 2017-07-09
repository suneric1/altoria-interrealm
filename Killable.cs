using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Killable : MonoBehaviour {

	public ParticleSystem hurtEffect;
	public int maxHealth = 50;
	public Renderer[] renderers;

//	public delegate void Action();
	public event Action OnDeath;
	public event Action OnFaint;

	int currentHealth;
	public bool isDead { get; set; }
	public bool isCapacitated { get; set; }
	Animator anim;
	Rigidbody rgbd;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		isDead = false;
		anim = GetComponent<Animator> ();
		rgbd = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {

	}

	public int HP { get{ return currentHealth; }}

	public void TakeDamage(int damage){
//		print ("EnemyDamaged");
		currentHealth -= damage;
		hurtEffect.Play ();
		if (currentHealth <= 0) {
			if (!isCapacitated) {
				currentHealth = 1;
				isCapacitated = true;
				GetComponent<Animator> ().SetTrigger ("Faint");

				FindObjectOfType<GameStateManager> ().deskaroFaint = true;

				if (OnFaint != null)
					OnFaint ();
			} else {
				currentHealth = 0;
				isDead = true;
				rgbd.isKinematic = true;

				if (OnDeath != null)
					OnDeath ();

				GetComponent<Animator> ().SetTrigger ("Die");
				StartCoroutine (FadeOut ());
			}
		}
	}

	public IEnumerator FadeOut(){
		//		print ("Out!");
		int a = 255;
		float t = 0;
		float duration = 3f;
		Color32[] colors = new Color32[renderers.Length];
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = renderers [i].material.color;
		}
		while (t < duration) {
			a = (int)(a - (255 * Time.deltaTime / duration));
			if (a < 0)
				a = 0;
			for (int i = 0; i < colors.Length; i++) {
				colors [i] = new Color32 (colors [i].r, colors [i].g, colors [i].b, (byte)a);
				if(renderers [i])
					renderers [i].material.color = colors [i];
			}
			t += Time.deltaTime;
			yield return null;
		}
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = new Color32 (colors [i].r, colors [i].g, colors [i].b, 0);
			if(renderers [i])
				renderers [i].material.color = colors [i];
		}
		Destroy (gameObject);
	}
}
