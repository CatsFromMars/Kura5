using UnityEngine;
using System.Collections;

public class EnemySegment : MonoBehaviour {

	//Allows for multiple hitboxes on a boss
	public EnemyClass enemyParent;
	public Transform blinker;
	private bool isInvincible = false;
	public int damageMultiplier = 1;
	protected Animator animator;

	void Awake() {
		initialize ();
	}

	public virtual void initialize() {
		//Find Parent Enemy Component
		if(enemyParent==null) enemyParent = transform.root.GetComponent<EnemyClass>();
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
		enemyParent.hitCounter -= 1;
		if(enemyParent.hitCounter < 0) enemyParent.hitCounter = 0;
		int dmg = enemyParent.damageCalculator.getDamage(bullet.element, enemyParent.element, bullet.damage, 1);
		enemyParent.takeDamage(dmg, bullet.element, false);
		StartCoroutine(flashWhite());
		//enemyParent.superEffectiveSmoke(enemyParent.element, bullet.element);
	}
	
	public virtual void hitWithSword() {
		//what happens if Emil hacks at this?
		enemyParent.hitCounter -= 1;
		if(enemyParent.hitCounter < 0) enemyParent.hitCounter = 0;
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

	public bool currentAnim(int hash) {
		return animator.GetCurrentAnimatorStateInfo(0).nameHash == hash;
	}

}
