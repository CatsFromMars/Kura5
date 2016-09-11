using UnityEngine;
using System.Collections;

public class BossSegment : MonoBehaviour {

	//Allows for multiple hitboxes on a boss
	public BossEnemy bossParent;
	public Transform blinker;
	private bool isInvincible = false;
	public int damageMultiplier = 1;

	void Awake() {
		//Find Parent Enemy Component
		bossParent = transform.root.GetComponent<BossEnemy>();
	}

	void OnCollisionEnter(Collision collision) {
		
		if (collision.collider.gameObject.tag == "Bullet") {
			//What happens if Annie shoots at this?
			Bullet bullet = collision.collider.gameObject.GetComponent<Bullet>();
			hitWithBullet(bullet);
		}
		
	}

	public virtual void hitWithBullet(Bullet bullet) {
		//What happens if Annie shoots at this?
		bossParent.hitCounter -= 1;
		if(bossParent.hitCounter < 0) bossParent.hitCounter = 0;
		int dmg = bossParent.damageCalculator.getDamage(bullet.element, bossParent.element, bullet.damage, 1);
		bossParent.takeDamage(dmg, bullet.element, false);
		StartCoroutine(flashWhite());
		//bossParent.superEffectiveSmoke(bossParent.element, bullet.element);
	}
	
	public virtual void hitWithSword() {
		//what happens if Emil hacks at this?
		bossParent.hitCounter -= 1;
		if(bossParent.hitCounter < 0) bossParent.hitCounter = 0;
		StartCoroutine(flashWhite());
		return;
	}

	public virtual void shadowSeal(SafeInt darkness) {
		//what happens if Emil shadow seals this?
		return;
	}

	protected IEnumerator flashWhite () {
		blinker.active = true;
		isInvincible = true;
		yield return new WaitForSeconds(0.2f);
		blinker.active = false;
		isInvincible = false;
	}

}
