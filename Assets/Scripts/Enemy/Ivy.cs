using UnityEngine;
using System.Collections;

public class Ivy : EnemyClass {

	private bool canRegrow = true; //Ivy can regrow if not hit by a dark attack
	private float distanceFromPlayer = 0f;
	private float range = 8f;
	private int attackTimer = 0;
	private int attackWaitTime = 50;
	private int knockback = 7;
	public bool hitWithDarkAttack = false;
	private Quaternion originalRot;
	private bool inRange = false;

	void Start() {
		originalRot = transform.rotation;
	}

	void Update() {
		if(animator.GetCurrentAnimatorStateInfo(0).nameHash == hash.dyingState) checkForDeath();

		if(animator.GetCurrentAnimatorStateInfo(0).nameHash == hash.attackState) {
			Vector3 target = new Vector3(player.position.x, this.transform.position.y, player.position.z);
			transform.LookAt(target);
		}
		else {
			transform.rotation = Quaternion.Slerp(transform.rotation, originalRot, Time.deltaTime * 3);
		}

		GetPlayerDistance ();
		DecideAttack ();
		if(hitWithDarkAttack) currentLife=0;
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
		else {
			animator.SetBool(hash.attackBool, false);
			attackTimer = 0;
		}
	}

	void Attack() {
		attackTimer = 0;
		if(distanceFromPlayer <= range) {
			player.GetComponent<PlayerContainer>().hitPlayer(strength, "Sol", knockback*transform.forward);
		}

	}

	void OnTriggerEnter (Collider other) {
		if(other.gameObject.tag == "Player") player = other.transform;
	}

	void checkForDeath() {
		if(hitWithDarkAttack) {
			killSelf();
		}
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
