using UnityEngine;
using System.Collections;

public class AnnieController : PlayerContainer {
	private int chargeCounter = 0;
	private int chargeThresh = 7;

	//AUDIO
	public AudioClip shootNoise;
	public AudioClip clickNoise;

	//VARIABLES REGARDING SHOOTING/COMBAT
	private Vector3 bulletSpawnPoint;
	public Transform bullet;
	public Transform fireBullet;
	public Transform earthBullet;
	public Transform weapon;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (playerInControl) {
			updateInput();
			updateAnimations();
			//handleCombos();
			handleTargeting();
		}
	}

	void handleTargeting() {
		if(targeting) {
			//Targeting Mode
			//if(parrying) {
			//	doParry();
			//}
			//else parryCounter = 0;
			targetEnemy();
			if(lockOn == null) lockOn = Instantiate (lockOnUI, currentTarget.transform.position, Quaternion.identity) as Transform;
			else lockOn.transform.position = currentTarget.transform.position;
		}
		else {
			//NON-Targeting Mode
			untargetEnemy();
			if (currentAnim(hash.hurtState)) knockBack(transform.forward*-3);
			else if(charging) Charge();
			//parryCounter = 0;
			if(lockOn != null) {
				Destroy(lockOn.gameObject);
			}
		}
	}

	public void Charge() {
		chargeCounter+=lightLevels.sunlight;
		if(chargeCounter > chargeThresh) {
			chargeCounter = 0;
			if(lightLevels.sunlight > 0) gameData.annieCurrentEnergy += 1;
		}
	}

	override protected void getHurt(int damage, Vector3 knockbackDir) {
		gameData.annieCurrentLife -= damage;
		
		if(gameData.annieCurrentLife <= 0) Die();
		else {
			//Play knockback anim
			animator.SetTrigger(hash.hurtTrigger);
		}
	}

	void Shoot() {
		if(gameData.annieCurrentEnergy >= 5) {
			bulletSpawnPoint = weapon.transform.position;
			//WHICH BULLET TO SPAWN
			if(gameData.annieCurrentElem == GameData.elementalProperty.Sol) Instantiate(bullet, bulletSpawnPoint, transform.rotation);
			else if(gameData.annieCurrentElem == GameData.elementalProperty.Fire) Instantiate(fireBullet, bulletSpawnPoint, transform.rotation);
			else if(gameData.annieCurrentElem == GameData.elementalProperty.Earth) Instantiate(earthBullet, bulletSpawnPoint, transform.rotation);
			else makeSound(clickNoise);
			gameData.annieCurrentEnergy -= 5; //REMEMBER TO CHANGE THIS! FOR TESTING PURPOSES ONLY!
			makeSound(shootNoise);
		}
		else {
			makeSound(clickNoise);
		}
		
	}
}
