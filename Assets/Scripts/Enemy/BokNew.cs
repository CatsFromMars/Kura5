using UnityEngine;
using System.Collections;

public class BokNew : PatrolEnemy {
	public Transform projectileSpawner;
	public Transform projectile;
	
	// Update is called once per frame
	void Update () {
		handleBurning ();
		updateAnimations ();
		if(frozen) Freeze();
		else if(stunned) Stunned(); //Stunned autotransitions to seek
		else if(pausing) Pause();
		else if(playerLastSighting != resetPlayerPosition) Seek(); //Seek autotransitions to attack or confused
		else Patrol(); //Transitions to Pause
		//Attack autotransitions to pause
		manageMovement();
	}

	void ShootProjectile() {
		//ADD AS ANIMATION EVENT
		Vector3 projectileSpawnPoint = projectileSpawner.transform.position;
		Instantiate(projectile, projectileSpawnPoint, transform.rotation);
	}
}
