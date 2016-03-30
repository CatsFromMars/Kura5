using UnityEngine;
using System.Collections;

public class AnnieController : PlayerContainer {
	private int chargeCounter = 0;
	private int chargeThresh = 20;

	//AUDIO
	public AudioClip shootNoise;
	public AudioClip clickNoise;
	public AudioClip regularStep;
	public AudioClip snowStep;

	//VARIABLES REGARDING SHOOTING/COMBAT
	private Vector3 bulletSpawnPoint;
	public Transform bullet;
	public Transform fireBullet;
	public Transform earthBullet;
	public Transform weapon;
	protected int energyCost = 2;
	private float absorbRate = 1f;
	private float absorbCounter = 0f;
	private float absorbCounterTime = 150f;

	//Visual
	public ParticleSystem sunParticles;
	public ParticleSystem dustL;
	public ParticleSystem dustR;
	public Transform footPrintL;
	public Transform footPrintR;
	public Transform footPrintPrefab;

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

			absorb(1);
		}
	}

	void absorb(int rate) {

		if(lightLevels.sunlight > 0 && !lightLevels.w.isNightTime) absorbCounter+=lightLevels.sunlight.GetValue();
		if(absorbCounter>absorbCounterTime) {
			gameData.annieCurrentEnergy += rate;
			absorbCounter = 0;
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
			chargeCounter+=(lightLevels.sunlight.GetValue() * Mathf.RoundToInt(Time.timeScale));
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
		if(gameData.annieCurrentEnergy >= energyCost) {
			bulletSpawnPoint = weapon.transform.position;
			//WHICH BULLET TO SPAWN
			if(gameData.annieCurrentElem == GameData.elementalProperty.Sol) Instantiate(bullet, bulletSpawnPoint, transform.rotation);
			else if(gameData.annieCurrentElem == GameData.elementalProperty.Fire) Instantiate(fireBullet, bulletSpawnPoint, transform.rotation);
			else if(gameData.annieCurrentElem == GameData.elementalProperty.Earth) Instantiate(earthBullet, bulletSpawnPoint, transform.rotation);
			else makeSound(clickNoise);
			gameData.annieCurrentEnergy -= energyCost; //REMEMBER TO CHANGE THIS! FOR TESTING PURPOSES ONLY!
			makeSound(shootNoise);
		}
		else {
			makeSound(clickNoise);
		}
		
	}

	public void makeStepNoise() {
		if(lightLevels.w.conditionName.Contains("Snow") && !isIndoors && inSnow) makeSound(snowStep);
		else makeSound(regularStep);
	}

	public void puffL() {
		//Animation event: has annie's feet emit particle effects depending on weather
		if(lightLevels.w.conditionName.Contains("Snow") && !isIndoors && inSnow) Instantiate (footPrintPrefab, footPrintR.position, transform.rotation);
		else dustL.Emit (2);
	}

	public void puffR() {
		//Animation event: has annie's feet emit particle effects depending on weather
		if(lightLevels.w.conditionName.Contains("Snow") && !isIndoors && inSnow) Instantiate (footPrintPrefab, footPrintL.position, transform.rotation);
		else dustL.Emit (2);
	}
}
