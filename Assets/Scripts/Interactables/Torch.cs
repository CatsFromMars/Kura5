using UnityEngine;
using System.Collections;

public class Torch : Activatable {
	private GameObject fire;

	void Awake() {
		fire = transform.FindChild("Fire").gameObject;
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "Bullet") {
			if(collision.gameObject.GetComponent<Bullet>().element == "Fire") {
				fire.SetActive(true);
				audio.Play();
				Activate();
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "EnemyWeapon") {
			if(other.gameObject.GetComponent<WeaponData>().element == "Fire") {
				fire.SetActive(true);
				audio.Play();
				Activate();
			}
		}
	}
}
