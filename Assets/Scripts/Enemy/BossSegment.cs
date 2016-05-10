using UnityEngine;
using System.Collections;

public class BossSegment : MonoBehaviour {

	//Allows for multiple hitboxes on a boss
	public BossEnemy bossParent;

	void Awake() {
		//Find Parent Enemy Component
		bossParent = transform.root.GetComponent<BossEnemy>();
		if(bossParent == null) Debug.LogError("No enemy parent found!");
	}

	void OnCollisionEnter(Collision collision) {
		
		if (collision.collider.gameObject.tag == "Bullet") {
			Bullet bullet = collision.collider.gameObject.GetComponent<Bullet>();
			int dmg = bossParent.damageCalculator.getDamage(bullet.element, bossParent.element, bullet.damage, 1);
			bossParent.takeDamage(dmg, bullet.element);
			bossParent.superEffectiveSmoke(bossParent.element, bullet.element);
		}
		
	}
	
	
}
