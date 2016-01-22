using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
	GameObject bombFlash;
	public Transform explosion;

	private Vector3 direction;
	public float velocity = 20f;
	private bool onGround = false;

	void Awake() {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (player != null) {
			Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y-1f, player.transform.position.z);
			transform.LookAt(pos);
		}
		direction = transform.forward;
	}
	
	void FixedUpdate() {
		if(!onGround)transform.position += direction * velocity * Time.deltaTime;
	}

	// Use this for initialization
	void Start () {
		bombFlash = transform.FindChild ("BombFlash").gameObject;
		StartCoroutine(goOff());
	}
	
	// Update is called once per frame
	IEnumerator goOff() {
		yield return new WaitForSeconds(1f);
		StartCoroutine(flash(0.3f));
		yield return new WaitForSeconds(1f);
		StopCoroutine(flash (0.3f));
		StartCoroutine(flash(0.1f));
		yield return new WaitForSeconds(1f);
		Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy (transform.gameObject);
	}

	IEnumerator flash(float speed) {
		while(true){
			yield return new WaitForSeconds(speed);
			bombFlash.SetActive(true);
			yield return new WaitForSeconds(speed);
			bombFlash.SetActive(false);
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Floor") {
			onGround = true;
			audio.Play();
		}
	}
}
