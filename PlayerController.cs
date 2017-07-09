using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PlayerController : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public PostProcessingProfile postProcessingProf;
	public bool PlayerMoving { get; private set; }

	Rigidbody playerRgbd;
	float speed = 6f;
	float preLIndexTrigger = 0;
	bool allowRotation = true;

	void Start(){
		playerRgbd = GetComponent<Rigidbody> ();
		preLIndexTrigger = Input.GetAxisRaw ("LIndexTrigger");
		PlayerMoving = false;
	}

	void Update()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");
		PlayerMoving = x != 0 || z != 0;
//		Vector3 camForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
//		Vector3 camRight = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);
		if (Input.GetAxisRaw ("LIndexTrigger") == 1) {
			if (allowRotation) {
				if (x > 0.9f) {
//					transform.Rotate (new Vector3 (0, 45, 0));
					StartCoroutine(Rotate(new Vector3 (0, 45, 0)));
					allowRotation = false;
				} else if (x < -0.9f) {
					//					transform.Rotate (new Vector3 (0, -45, 0));
					StartCoroutine(Rotate(new Vector3 (0, -45, 0)));
					allowRotation = false;
				}
			}
		} else {
			Vector3 movement = (transform.forward * Input.GetAxisRaw ("Vertical")).normalized + (transform.right * Input.GetAxisRaw ("Horizontal")).normalized;
			playerRgbd.MovePosition (transform.position + movement.normalized * Time.deltaTime * speed);
		}
		if (x == 0)
			allowRotation = true;
		preLIndexTrigger = Input.GetAxisRaw ("LIndexTrigger");
	}

	IEnumerator Rotate(Vector3 rot){
		postProcessingProf.motionBlur.enabled = true;
		Vector3 targetEuler = transform.localEulerAngles + rot;
		float t = 0;
		while (t < 0.1f) {
			t += Time.deltaTime;
			transform.Rotate (rot * Time.deltaTime / 0.1f);
			yield return null;
		}
		transform.localEulerAngles = targetEuler;
		postProcessingProf.motionBlur.enabled = false;
	}
}
