using UnityEngine;
using System.Collections;

public class BossSegment : EnemySegment {

	//Allows for multiple hitboxes on a boss
	public BossEnemy bossParent;
	//public Transform blinker;
	//private bool isInvincible = false;
	//public int damageMultiplier = 1;
	//protected Animator animator;

	void Awake() {
		if(bossParent == null) bossParent = transform.root.GetComponent<BossEnemy>();
		enemyParent = bossParent;
		Debug.Log (enemyParent.name);
	}

	public override void hitWithBullet(Bullet bullet) {
		//What happens if Annie shoots at this?
		bossParent.hitCounter -= 1;
		if(bossParent.hitCounter < 0) bossParent.hitCounter = 0;
		int dmg = bossParent.damageCalculator.getDamage(bullet.element, bossParent.element, bullet.damage, 1);
		bossParent.takeDamage(dmg, bullet.element, false);
		StartCoroutine(flashWhite());
		//bossParent.superEffectiveSmoke(bossParent.element, bullet.element);
	}

	public override void hitWithSword() {
		//what happens if Emil hacks at this?
		bossParent.hitCounter -= 1;
		if(bossParent.hitCounter < 0) bossParent.hitCounter = 0;
		StartCoroutine(flashWhite());
		return;
	}

}
