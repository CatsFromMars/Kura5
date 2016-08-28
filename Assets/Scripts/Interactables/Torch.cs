using UnityEngine;
using System.Collections;

public class Torch : Activatable {
	private GameObject fire;
	private GameObject[] o;

	void Awake() {
		fire = transform.FindChild("Fire").gameObject;
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "Bullet") {
			if(collision.gameObject.GetComponent<Bullet>().element == "Fire") {
				fire.SetActive(true);
				brightenRoom();
				audio.Play();
				Activate();
			}
		}
	}

	public void douse() {
		fire.SetActive(false);
		darkenRoom();
		audio.Play();
		Deactivate();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "EnemyWeapon") {
			if(other.gameObject.GetComponent<WeaponData>().element == "Fire") {
				light();
			}
		}
	}

	public void light() {
		fire.SetActive(true);
		brightenRoom();
		audio.Play();
		Activate();
	}

	void brightenRoom() {
		//Disables all GameObjects tagged as Occlusion
		o = GameObject.FindGameObjectsWithTag ("Occlusion");
		foreach (GameObject i in o) i.SetActive (false);
	}

	void darkenRoom() {
		//if(o!=null) foreach (GameObject i in o) if(i!=null) i.SetActive (true);
	}
}
