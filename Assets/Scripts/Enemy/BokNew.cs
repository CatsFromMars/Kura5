using UnityEngine;
using System.Collections;

public class BokNew : PatrolEnemy {
	public Transform projectileSpawner;
	public Transform projectile;
	public SkinnedMeshRenderer r;
	public Material[] Frost;
	public Material[] Fire;
	public Material[] Cloud;
	public Material[] Earth;
	public Material[] Dark;
	private bool alreadyChangedElement;
	
	// Update is called once per frame
	void Update () {
		if(!dead) {
			handleBurning ();
			updateAnimations ();
			if(frozen) Freeze();
			else if(stunned) Stunned(); //Stunned autotransitions to seek
			else if(pausing) Pause();
			else if(playerLastSighting != resetPlayerPosition) Seek(); //Seek autotransitions to attack or confused
			else Patrol(); //Transitions to Pause
			//Attack autotransitions to pause
			manageMovement();

			//Swap element according to weather
//			if(!alreadyChangedElement && changesElementWithWeather && !selfOnScreen()) {
//				swapElement();
//				alreadyChangedElement = true;
//			}
//			else if (selfOnScreen()) alreadyChangedElement = false;
		}
	}

	void ShootProjectile() {
		//ADD AS ANIMATION EVENT
		Vector3 projectileSpawnPoint = projectileSpawner.transform.position;
		Instantiate(projectile, projectileSpawnPoint, transform.rotation);
	}

	public override void changeColor() {
		r = body.GetComponent<SkinnedMeshRenderer>();
		Material[] mats = r.materials;
		if (element == "Fire") {
			mats[2] = Fire[0];
			mats[0] = Fire[1];
		}
		if (element == "Frost") {
			mats[2] = Frost[0];
			mats[0] = Frost[1];
		}
		if (element == "Cloud") {
			mats[2] = Cloud[0];
			mats[0] = Cloud[1];
		}
		if (element == "Earth") {
			mats[2] = Earth[0];
			mats[0] = Earth[1];
		}
		if (element == "Dark") {
			mats[2] = Dark[0];
			mats[0] = Dark[1];
			mats [5].color = Color.black;
			mats [3].color = Color.yellow;
		}
		else {
			mats [5].color = Color.white;
			mats [3].color = Color.red;
		}
		r.materials = mats;
	}
}
