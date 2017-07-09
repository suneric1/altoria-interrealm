using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mob : MonoBehaviour {

    Vector3[] stops = new Vector3[4];
	NavMeshAgent nav;
	Transform player;
	Animator anim;
	float timer = 0f;
	float attackInterval = 2f;
	bool playerInRange = false;
	PlayerHealth playerHealth;

	public bool maskOff{ get; set;}
	public int damage = 10;

	// Use this for initialization
	void Start () {
        stops[0] = transform.position;
        stops[1] = transform.position + new Vector3(8, 0, 0);
        stops[2] = transform.position + new Vector3(8, 0, -8);
        stops[3] = transform.position + new Vector3(0, 0, -8);
//        StartCoroutine(Wander());

		maskOff = false;
		nav = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
    }
	
	// Update is called once per frame
	void Update () {
		if (GameObject.FindGameObjectWithTag ("Player")) {
			player = GameObject.FindGameObjectWithTag ("Player").transform;
			nav.SetDestination (player.position);

			timer += Time.deltaTime;

			if (!maskOff && playerInRange && timer > attackInterval)
				Attack ();
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			playerInRange = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player"))
			playerInRange = false;
	}

	void Attack(){
		anim.SetTrigger ("Attack");
		timer = 0f;
	}

	public void TakePlayerDamage(){
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		playerHealth.TakeDamage (damage);
	}

    IEnumerator Wander()
    {
        int stopIndex = 1;
        int preStopIndex = 0;
        float t = 0;
        transform.LookAt(stops[stopIndex]);
        while (true)
        {
            t += 2*Time.deltaTime;
            if(Vector3.Distance(transform.position, stops[stopIndex]) > 0)
                transform.position = Vector3.Lerp(stops[preStopIndex], stops[stopIndex], t/Vector3.Distance(stops[preStopIndex], stops[stopIndex]));
            else
            {
                t = 0;
                preStopIndex = stopIndex;
                stopIndex++;
                if (stopIndex > 3) stopIndex = 0;
                transform.LookAt(stops[stopIndex]);
            }
            yield return null;
        }
    }
}
