using UnityEngine;
using System.Collections;

//A simple projectile that shoots in a straight direcrion.

public class BokProjectile : MonoBehaviour {
	//ACTIVE BULLET TIME
	public float elapsedTime;
	
	//HOW FAST SHOULD THE BULLET GO?
	private Vector3 direction;
	public float velocity = 100f;
	
	//BE SURE TO CHANGE THIS LATER. MAKE IT NOT PUBLIC.
	public Transform hitEffect;
	public Transform hitAirEffect;
	public Transform goo;

	void Awake() {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (player != null) {
			//Vector3 pos = new Vector3(player.transform.position.x, -1f, player.transform.position.z);
			transform.LookAt(player.transform.position);
		}
		direction = transform.forward;
	}
	
	void FixedUpdate() {
		transform.position += direction * velocity * Time.deltaTime;
		elapsedTime++;
	}


	void OnTriggerEnter(Collider other) {

		if (other.gameObject.tag == "Floor") {
			//WHAT HAPPENS IF A BULLET HITS A WALL/FLOOR?
			Instantiate(hitEffect, transform.position, Quaternion.identity);
			Instantiate(goo, transform.position, Quaternion.identity);
			Destroy (this.gameObject);

		}

		else if (other.gameObject.tag == "Player") {
			//Parry reflect!
			PlayerContainer player = other.GetComponent<PlayerContainer>();
			if(player.currentAnim(Animator.StringToHash("Targeting.Block"))) {
				direction *= -1;
			}
			else {
				Instantiate(hitAirEffect, transform.position, Quaternion.identity);
				Destroy (this.gameObject);
			}
		}

		else if (other.gameObject.tag == "Wall") {
			Instantiate(hitAirEffect, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
		}

		else if (other.gameObject.tag == "Enemy") {
			if(direction == transform.forward*-1) {
				Instantiate(hitAirEffect, transform.position, Quaternion.identity);
				Destroy (this.gameObject);
			}
		}
	}
}
