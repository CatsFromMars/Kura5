using UnityEngine;
using System.Collections;

public class UndeadCoffin : MonoBehaviour {
	//Used for Wadjet Boss
	//If Debris GO touches coffin, crack that sucker open and spawn an Undead!
	public Transform coffinHitEffect;
	public Transform enemyPrefab;

	void OnTriggerEnter(Collider other) {
		if(other.name.Contains("Debris")) {
			Instantiate(coffinHitEffect, transform.position, Quaternion.identity);
			Instantiate(enemyPrefab, transform.position, Quaternion.identity);
			Destroy(other.gameObject);
			Destroy(this.gameObject);
		}
	}
}
