using UnityEngine;
using System.Collections;

public class Torch : Activatable {
	private GameObject fire;
	private GameObject[] o;
	public bool setFlag;
	public string flag;
	private Flags flags;

	void Awake() {
		flags = GetUtil.getFlags();
		if(setFlag) flags.AddOtherFlag(flag);
		fire = transform.FindChild("Fire").gameObject;
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "Bullet") {
			if(collision.gameObject.GetComponent<Bullet>().element == "Fire") {
				if(setFlag) {
					Debug.Log("Set flag at: "+flag);
					flags.SetOther(flag);
				}
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
				Debug.Log("Is falg settable? "+setFlag);
				if(setFlag) {
					Debug.Log("Set flag at: "+flag);
					flags.SetOther(flag);
				}
				light();
			}
		}
	}

	public void light() {
		if(fire==null) fire = transform.FindChild("Fire").gameObject;
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
