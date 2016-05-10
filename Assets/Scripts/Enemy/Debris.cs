using UnityEngine;
using System.Collections;

public class Debris : MonoBehaviour {
	private bool bitsSpawned;
	public Transform hitEffect;


	void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "Floor") {
			//WHAT HAPPENS IF A BULLET HITS A WALL/FLOOR?
			if(!bitsSpawned) {
				Instantiate(hitEffect, transform.position, Quaternion.identity);
				bitsSpawned = true;
			}
			Destroy (this.gameObject);
			
		}
	}
}
