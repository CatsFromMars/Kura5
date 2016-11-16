using UnityEngine;
using System.Collections;

//A simple projectile that shoots in a straight direcrion.

public class Bullet : MonoBehaviour {
	//BULET STAT VARS
	public string element = "Sol";
	public int damage = 5;
	public float velocity = 100f;
	public float stunTime = 0.5f;
	public float size = 1f;

	//public SphereCollider col;
	public Transform elec;
	private bool hit = false;
	public float aliveTime = 1f;
	
	//ACTIVE BULLET TIME
	private float elapsedTime;
	//BE SURE TO CHANGE THIS LATER. MAKE IT NOT PUBLIC.
	public Transform hitEffect;

	void Start() {
		transform.localScale = new Vector3 (size,size,size);
	}
	
	void Update() {
		//if(!GetComponent<MeshRenderer>().renderer.isVisible) Destroy(this.gameObject);
		if(Time.timeScale == 0) {
			transform.position += transform.forward * velocity * Time.unscaledDeltaTime;
			elapsedTime+=Time.unscaledDeltaTime;
			if(elapsedTime >= aliveTime) Destroy (this.gameObject);
		}
	}
	
	void FixedUpdate() {
		//Debug.Log (elapsedTime);
		transform.position += transform.forward * velocity * Time.deltaTime;
		elapsedTime+=(Time.deltaTime);
		if(elapsedTime >= aliveTime) Destroy (this.gameObject);
	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag=="Invisible") GetComponent<SphereCollider>().enabled=false;
		else if(!hit) {
			hit = true;
			if(collision.collider.tag == "Wall") {
				Instantiate(Resources.Load("Effects/Sound") as GameObject, transform.position, Quaternion.identity);
			}
			
			Instantiate(hitEffect, transform.position, Quaternion.identity);
			if(elec!=null)elec.parent = null;
			Destroy (this.gameObject);
		}
	}
}