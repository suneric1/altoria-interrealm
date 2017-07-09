using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public int maxHealth = 100;
	public Image damageEffect;

	float recoverTimer;
	int currentHealth;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		recoverTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (recoverTimer > 10 && currentHealth < 100 && currentHealth > 0) {
			currentHealth += 20;
			if (currentHealth > 100)
				currentHealth = 100;
			damageEffect.color = new Color (1, 0, 0, Mathf.Floor((maxHealth - currentHealth)/20f)*20 / 400f);
//			print (currentHealth);
			recoverTimer = 0;
		}
		recoverTimer += Time.deltaTime;
	}

	public int HP { get{ return currentHealth; }}

	public void TakeDamage(int damage){
		currentHealth -= damage;
		if (currentHealth <= 0) {
			currentHealth = 0;
//			SceneManager.LoadScene (Application.loadedLevel);
		}
		damageEffect.color = new Color (1, 0, 0, Mathf.Floor((maxHealth - currentHealth)/20f)*20 / 400f);
//		print (currentHealth);
	}
}
