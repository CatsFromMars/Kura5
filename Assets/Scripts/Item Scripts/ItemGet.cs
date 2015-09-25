using UnityEngine;
using System.Collections;

public class ItemGet : MonoBehaviour {
	
	public int itemID;
	Inventory inventory;
	GameObject globalData;
	bool itemCollected = false;
	public AudioClip thumpSound; //NOISE MADE WHEN IT FALLS ON GROUND
	bool fellOnGround = false;

	// Use this for initialization
	void Awake () {
		globalData = GameObject.FindGameObjectWithTag("GameController");
		inventory = globalData.GetComponent<Inventory>();
		rigidbody.AddForce(Vector3.up * rigidbody.mass*500);
	}

	void OnTriggerStay(Collider other) {
		if(Input.GetButtonUp("Confirm") && other.gameObject.tag == "Player") {
			if(!itemCollected) inventory.AddItem(itemID);
			itemCollected = true;
			Instantiate (Resources.Load("Effects/Pickup"), transform.position, Quaternion.identity);
			Destroy(this);
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.relativeVelocity.magnitude > 0 && !fellOnGround) {
			fellOnGround = true;
			audio.clip = thumpSound;
			audio.Play();
		}
		
	}

}
