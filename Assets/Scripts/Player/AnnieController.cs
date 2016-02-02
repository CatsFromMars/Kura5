using UnityEngine;
using System.Collections;

public class AnnieController : PlayerContainer {
	private int chargeCounter = 0;
	private int chargeThresh = 20;

	//AUDIO
	public AudioClip shootNoise;
	public AudioClip clickNoise;

	//VARIABLES REGARDING SHOOTING/COMBAT
	private Vector3 bulletSpawnPoint;
	public Transform bullet;
	public Transform fireBullet;
	public Transform earthBullet;
	public Transform weapon;

	//Visual
	public ParticleSystem sunParticles;
	public ParticleSystem dustL;
	public ParticleSystem dustR;

	private bool solarCharging = false;

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

		if (currentAnim(hash.hurtState)) {
			targeting = false;
			knockBack(currentKnockbackDir);
		}
		else if(charging&&!targeting) Charge();


		if(targeting) {
			//Targeting Mode
			//if(parrying) {
			//	doParry();
			//}
			//else parryCounter = 0;
			targetEnemy();
			if(currentTarget != null) {
				if(lockOn == null) lockOn = Instantiate (lockOnUI, currentTarget.transform.position, Quaternion.identity) as Transform;
				else lockOn.transform.position = currentTarget.transform.position;
				zoomToEnemy();
			}
			else { 
				untargetEnemy();
				if(lockOn != null) Destroy(lockOn.gameObject);
				zoomToPlayer();
			}
		}
		else {
			//NON-Targeting Mode
			untargetEnemy();
			//parryCounter = 0;
			if(lockOn != null) {
				Destroy(lockOn.gameObject);
				zoomToPlayer();
			}
		}
	}

	public void Charge() {
		solarCharging = Time.timeScale != 0 && !lightLevels.w.isNightTime && lightLevels.sunlight > 0 && gameData.annieCurrentEnergy < gameData.annieMaxEnergy && currentAnim(hash.chargeState);
		if(!lightLevels.w.isNightTime) {
			if(solarCharging&&!audio.isPlaying) makeSound(chargingSound);
			chargeCounter+=(lightLevels.sunlight * Mathf.RoundToInt(Time.timeScale));
			if(chargeCounter > chargeThresh) {
				chargeCounter = 0;
				if(lightLevels.sunlight > 0 && gameData.annieCurrentEnergy < gameData.annieMaxEnergy) {
					sunParticles.Emit (1);
					gameData.annieCurrentEnergy += 1;
				}
			}
		}
	}

	public void voiceIfCharging(AudioClip clip) {
		//ANIMATION EVENTS FOR VOICE ACTING
		if(Time.timeScale != 0 && !lightLevels.w.isNightTime && lightLevels.sunlight > 0 && gameData.annieCurrentEnergy < gameData.annieMaxEnergy) {
			voice.volume = 1;
			voice.pitch = 1f;
			voice.clip = clip;
			voice.Play();
		}
	}

	override protected void getHurt(int damage, Vector3 knockbackDir) {
		gameData.annieCurrentLife -= damage;
		
		if(gameData.annieCurrentLife <= 0) {
			Die();
			playVoiceClip(dieVoices[Random.Range(0, dieVoices.Length)]);
		}
		else {
			//Play knockback anim
			animator.SetTrigger(hash.hurtTrigger);
			//Play hurt voice
			playVoiceClip(hurtVoices[Random.Range(0, hurtVoices.Length)]);
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

	public void puffL() {
		//Animation event: has dust scatter where annie walks!
		dustL.Emit (2);
	}

	public void puffR() {
		//Animation event: has dust scatter where annie walks!
		dustR.Emit (2);
	}
}
