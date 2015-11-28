using UnityEngine;
using System.Collections;

public class Ivy : EnemyClass {

	private bool canRegrow = true; //Ivy can regrow if not hit by a dark attack
	private float distanceFromPlayer = 0f;
	private float range = 7f;
	private int attackTimer = 0;
	private int attackWaitTime = 30;
	private int knockback = 7;
	public bool hitWithDarkAttack = false;

	void Update() {
		if(animator.GetCurrentAnimatorStateInfo(0).nameHash == hash.dyingState) checkForDeath();
		quickLook ();
		GetPlayerDistance ();
		DecideAttack ();
	}

	void DecideAttack() {
		if(distanceFromPlayer <= range) {
			animator.SetBool(hash.attackBool, false);
			if(attackTimer >= attackWaitTime) {
				animator.SetBool(hash.attackBool, true);
				attackTimer = 0;
			}
			else attackTimer++;
		}
		else animator.SetBool(hash.attackBool, false);
	}

	void Attack() {
		if(distanceFromPlayer <= range) {
			player.GetComponent<PlayerContainer>().hitPlayer(strength, "Sol", knockback*transform.forward);
		}

	}

	void OnTriggerEnter (Collider other) {
		if(other.gameObject.tag == "Player") player = other.transform;
	}

	void checkForDeath() {
		if(hitWithDarkAttack) killSelf();
		else Regenerate();
	}

	void Regenerate() {
		//Set death bools to false
		animator.SetBool (hash.deadBool, false);
		//Regenerate HP
		currentLife = maxLife;
		dead = false;
		dying = false;
	}

	public void killSelf() {
		//To be called by Emil's controller. Insta kill self if hit by a dark elem attack
		//Set HP to zero 
		//Death
		DestroySelf ();
	}

	void GetPlayerDistance () {
		if (player != null) {
			distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
		}
	}
}
