using UnityEngine;
using System.Collections;

public class Mummy : PatrolEnemy {
	public Transform projectileSpawner;
	public Transform projectile;
	bool playerNoise;
	
	// Update is called once per frame
	void Update () {
		playerNoise = trackingPlayer&&(playerAnimator.GetCurrentAnimatorStateInfo(0).nameHash == hash.runningState);
		if(playerNoise) playerLastSighting = player.transform.position;

		if(!dead) {
			updateAnimations();
			animator.SetBool(Animator.StringToHash("PlayerClose"), false); //change later
			if(frozen) Freeze();
			else if(stunned) Stunned(); //Stunned autotransitions to seek
			else if(pausing) Pause();
			else if(playerLastSighting != resetPlayerPosition) Seek(); //Seek autotransitions to attack or confused
			else Patrol(); //Transitions to Pause
			manageMovement();
		}
	}

	public override void Seek() {
		//decide: attack or chase?
		chasing = true;
		if(!playerNoise) pauseTimer = pauseWaitTime-1;
		if(playerInSight&&playerNoise) Attack();
		else Chase();
	}

	void spawnEffectAtFeet(Transform effect) {
		Vector3 pos = new Vector3(transform.position.x, transform.position.y - 3.6f, transform.position.z);
		Instantiate(effect, pos, transform.rotation);
	}

	void ShootProjectile() {
		//ADD AS ANIMATION EVENT
		Vector3 projectileSpawnPoint = projectileSpawner.transform.position;
		Instantiate(projectile, projectileSpawnPoint, transform.rotation);
	}
}
