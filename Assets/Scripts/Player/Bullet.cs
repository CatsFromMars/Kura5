using UnityEngine;
using System.Collections;

//A simple projectile that shoots in a straight direcrion.

public class Bullet : MonoBehaviour {
	//BULET STAT VARS
	public int damage = 5;
	public string element = "Sol";
	public float velocity = 100f;
	public Transform elec;

	//ACTIVE BULLET TIME
	private float elapsedTime;
	//BE SURE TO CHANGE THIS LATER. MAKE IT NOT PUBLIC.
	public Transform hitEffect;

	void Update() {
		if(!GetComponent<MeshRenderer>().renderer.isVisible) Destroy(this.gameObject);
	}

	void FixedUpdate() {
		transform.position += transform.forward * velocity * Time.deltaTime;
		elapsedTime++;
	}

	void OnCollisionEnter(Collision collision) {

		if(collision.collider.tag == "Wall") {
			Instantiate(Resources.Load("Effects/Sound") as GameObject, transform.position, Quaternion.identity);
		}

		Instantiate(hitEffect, transform.position, Quaternion.identity);
		if(elec!=null)elec.parent = null;
		Destroy (this.gameObject);

	}
}
