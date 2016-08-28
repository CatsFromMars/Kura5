using UnityEngine;
using System.Collections;

public class Mummy : PatrolEnemy {
	public int burnedSpeed = 10;
	bool onFire = false;
	public Transform projectileSpawner;
	public Transform projectile;
	bool playerNoise;
	public WeaponData weapon;
	private bool listeningForPlayer = false;
	public GameObject fireEffect;
	private int fireCounter = 6;
	private GameObject[] occluders;
	
	// Update is called once per frame
	void Update () {

		//Lights ablaze if his with fire attack
		if(mostRecentAttackElem == "Fire") {
			if(!onFire) {
				occluders = GameObject.FindGameObjectsWithTag("Occlusion");
				brightenRoom();
				StartCoroutine(BurnStart());
				mostRecentAttackElem = "Null";
			}
		}

		float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
		playerNoise = trackingPlayer&&(playerAnimator.GetCurrentAnimatorStateInfo(0).nameHash == hash.runningState);
		canSee = playerNoise;
		animator.SetBool(Animator.StringToHash("PlayerClose"), (distanceFromPlayer<5f));
		if(playerNoise) playerLastSighting = player.transform.position;

		if(!dead) {
			updateAnimations();
			if(onFire) Burn();
			else if(frozen) Freeze();
			else if(stunned) Stunned(); //Stunned autotransitions to seek
			else if(pausing) Pause();
			else if(distanceFromPlayer<3.5f) Attack();
			else if(playerLastSighting != resetPlayerPosition) Seek(); //Seek autotransitions to attack or confused
			else Patrol(); //Transitions to Pause
			manageMovement();

			if(animator.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Bite")) weapon.damage = strength;
			else weapon.damage = 7;

			if(listeningForPlayer && playerNoise && !onFire) {
				listeningForPlayer = false;
				animator.SetTrigger (Animator.StringToHash("PlayerSpotted"));
			}
		}
	}

	void Burn() {
		//Run around ON FIYAH!
		Patrol();
		agent.speed = burnedSpeed;
		patrolWaitTime = 10;
	}
	void brightenRoom() {
		//Disables all GameObjects tagged as Occlusion
		foreach (GameObject i in occluders) i.SetActive (false);
	}

	void darkenRoom() {
		foreach (GameObject i in occluders) i.SetActive (true);
	}

	IEnumerator BurnStart() {
		onFire = true;
		fireEffect.SetActive (true);
		animator.SetBool (Animator.StringToHash("OnFire"), true);
		weapon.element = "Fire";
		for (int i=0; i<fireCounter; i++) {
			yield return new WaitForSeconds(2);
			takeDamage(3, "Fire");
		}

		patrolWaitTime = 10;
		onFire = false;
		fireEffect.SetActive (false);
		animator.SetBool (Animator.StringToHash("OnFire"), false);
		patrolWaitTime = 100;
		weapon.element = element;
		mostRecentAttackElem = "Null";
		darkenRoom ();
	}

	public override void Seek() {
		//decide: attack or chase?
		sightRange = col.radius;
		chasing = true;
		if(!playerNoise) pauseTimer = pauseWaitTime-1;
		if(playerInSight&&playerNoise&&canSee) Attack();
		else Chase();
	}

	void OnTriggerEnter (Collider other) {
		//Player can be silent and still trigger caution. Fix this bug
		if(other.gameObject.tag == "Player" && !trackingPlayer) {
			//animator.SetTrigger (Animator.StringToHash("PlayerSpotted"));
			listeningForPlayer = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "Player") {
			trackingPlayer = false;
			//audio.enabled = false;
			playerInSight = false;
			listeningForPlayer = false;
		}
	}
	
	
	void spawnEffectAtFeet(Transform effect) {
		Vector3 pos = new Vector3(transform.position.x, transform.position.y - 3.6f, transform.position.z);
		Instantiate(effect, pos, transform.rotation);
	}

	void Bite() {

	}

	void ShootProjectile() {
		//ADD AS ANIMATION EVENT
		Vector3 projectileSpawnPoint = projectileSpawner.transform.position;
		Instantiate(projectile, projectileSpawnPoint, transform.rotation);
	}
}
