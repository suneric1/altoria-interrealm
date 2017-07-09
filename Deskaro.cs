using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Deskaro : MonoBehaviour {

	public Vector3[] pathNodes;
	public int damage = 10;
	public Mask mask;

	public SpawnSpot spawnSpot{ get; set; }

	Exposable exposable;
	Killable killable;

	float findPathTimer = 0;
	int nodeIndex = 0;
	CapsuleCollider col;

	Transform player;
	PlayerHealth playerHealth;
	bool playerInRange = false;
	float attackTimer = 0f;
	float attackInterval = 2f;

	void Awake () {
		exposable = GetComponent<Exposable> ();
		killable = GetComponent<Killable> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerHealth = player.GetComponent<PlayerHealth> ();
		col = GetComponent<CapsuleCollider> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!exposable.OutOfRealm && findPathTimer > 5) {
			findPathTimer = 0;
			if (nodeIndex < pathNodes.Length) {
				GetComponent<NavMeshAgent> ().SetDestination (pathNodes [nodeIndex]);
				nodeIndex++;
				if (nodeIndex >= pathNodes.Length)
					nodeIndex = 0;
			}
		}
		findPathTimer += Time.deltaTime;
		if (exposable.OutOfRealm && !killable.isDead && !killable.isCapacitated) {
//			print (playerInRange);
			GetComponent<NavMeshAgent> ().SetDestination (player.position);
			attackTimer += Time.deltaTime;
			if (playerInRange && attackTimer > attackInterval)
				Attack ();
		}
	}

	void OnEnable(){
		killable.OnFaint += OnFaint;
		killable.OnDeath += OnDeath;
	}

	void OnDisable(){
		killable.OnFaint -= OnFaint;
		killable.OnDeath -= OnDeath;
	}

	void OnDeath(){
		FindObjectOfType<GameStateManager> ().maskLost = true;

		if (spawnSpot)
			spawnSpot.Released ();
		if (mask != null && mask.GetComponent<Grabbable>().grabber == null) {
			mask.GetComponent<Rigidbody> ().isKinematic = false;
			mask.GetComponent<Collider> ().isTrigger = false;
		}
	}

	void Attack(){
		GetComponent<Animator>().SetTrigger ("Attack");
		StartCoroutine (TakePlayerDamage (0.8f));
		attackTimer = 0f;
	}

	IEnumerator TakePlayerDamage(float delay = 0){
		yield return new WaitForSeconds (delay);
		playerHealth.TakeDamage (damage);
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			playerInRange = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
			playerInRange = false;
		}
	}

	void OnFaint(){
		mask.GetComponent<Grabbable> ().enabled = true;
	}
}
