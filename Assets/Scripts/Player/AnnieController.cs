using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnnieController : PlayerContainer {
	private int chargeCounter = 0;
	private int chargeThresh = 20;

	//AUDIO
	public AudioClip shootNoise;
	public AudioClip clickNoise;
	public AudioClip regularStep;
	public AudioClip splashStep;
	public AudioClip snowStep;

	//Shooting
	private Vector3 bulletSpawnPoint;
	public Transform cutsceneBullet;
	public Bullet bullet;
	public RaycastBullet raycastBullet;
	public Transform weapon;
	protected int energyCost = 2;
	private float absorbRate = 1f;
	private float absorbCounter = 0f;
	private float absorbCounterTime = 150f;
	public GameObject spread;
	private float range = 30f;
	private float angle = 15f;

	//Visual
	public ParticleSystem sunParticles;
	public ParticleSystem splashL;
	public ParticleSystem splashR;
	public ParticleSystem dustL;
	public ParticleSystem dustR;
	public Transform footPrintL;
	public Transform footPrintR;
	public Transform footPrintPrefab;
	public ParticleSystem gunBang;

	private bool solarCharging = false;

	private Vector3 targetpoint; //used for debugging

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

		if (playerInControl) {
			//updateInput();
			updateAnimations();
			//handleCombos();
			handleTargeting();
			checkForLensSwap();
			if(Time.timeScale!=0) absorb(1);
			handleSpecialAttack();
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
			if(Time.timeScale != 0) {
				zoomToEnemy();
			}
			else zoomToPlayer();
		}
		else {
			zoomToPlayer();
		}
	}

	//public override void updateAttackAnimSpeed() {
	//	animator.speed = 1;
	//}
	
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
		//ANIMATION EVENTS FOR VOICE ATING
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
		if(((GameData.annieWeaponConfig.SpecialAttack=="None" || !specialAttack)) && gameData.annieCurrentEnergy >= energyCost && bullet.CountSpawned() < GameData.annieWeaponConfig.combo) {
			bulletSpawnPoint = weapon.transform.position;
			//WHICH BULLET TO SPAWN
			//if(gameData.annieCurrentElem == GameData.elementalProperty.Sol) Instantiate(bullet, bulletSpawnPoint, transform.rotation);
			//else if(gameData.annieCurrentElem == GameData.elementalProperty.Fire) Instantiate(fireBullet, bulletSpawnPoint, transform.rotation);
			//else if(gameData.annieCurrentElem == GameData.elementalProperty.Earth) Instantiate(earthBullet, bulletSpawnPoint, transform.rotation);
			//else makeSound(clickNoise);
			if(GameData.annieWeaponConfig.speed > 4) raycastShoot();
			else bullet.Spawn(bulletSpawnPoint, transform.rotation);
			gameData.annieCurrentEnergy -= energyCost;
			makeSound(shootNoise);
		}
		else {
			makeSound(clickNoise);
		}
		
	}

	void raycastShoot() {
		//Currently ignores the combo stat altogether. Maxing speed is proably OP at this point.
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
		gunBang.Stop ();
		gunBang.Play ();
		for(int i=0; i<hitColliders.Length; i++) {
			bool canHitWall=true;
			Transform target = hitColliders[i].transform;
			Vector3 targetDir = (target.position - transform.position).normalized;
			if(!hitColliders[i].isTrigger && Vector3.Angle(this.transform.forward, targetDir) < angle) {
				float dist = Vector3.Distance(transform.position, target.position);
				RaycastHit hit;
				if(Physics.Raycast (transform.position, targetDir, out hit, dist)) {
					if(hit.collider.gameObject.tag == "Enemy") {
						canHitWall=false;
						hitEnemy(hit);
						if(checkForBossSegment(hit)) break;
					}
					else if(hit.collider.gameObject.tag == "Wall"&&canHitWall) {
						hitWall(hit);
						canHitWall=false;
					}
				}
			}
		}
	}

	void hitEnemy(RaycastHit hit) {
		int m = 1;
		EnemySegment b = null;
		//Ordinary Enemies
		EnemyClass enemy = hit.collider.GetComponent<EnemyClass>();
		//if it's a boss segment...
		if(enemy == null) { 
			b = hit.collider.GetComponent<EnemySegment>();
			b.hitWithSword();
			enemy = b.enemyParent;
			m = b.damageMultiplier;
		}

		int dmg = damageCalculator.getDamage(gameData.annieCurrentElem.ToString(), enemy.element, GameData.annieWeaponConfig.damage, 1);
		enemy.takeDamage (dmg*m, gameData.annieCurrentElem.ToString());
		enemy.knockback (transform.forward);

		//Sparkly Effects and Sound
		Instantiate (weaponHit, hit.transform.position, Quaternion.identity);
		enemy.superEffectiveSmoke (enemy.element, element);
	}

	void handleSpecialAttack() {
		if(GameData.annieWeaponConfig.SpecialAttack=="Spread") {
			spread.SetActive(specialAttack);
		}
	}

	void cutsceneShoot() {
		bulletSpawnPoint = weapon.transform.position;
		Instantiate(cutsceneBullet, bulletSpawnPoint, transform.rotation);
		makeSound(shootNoise);
	}

	public void makeStepNoise() {
		if(lightLevels.w.inSnow) makeSound(snowStep);
		else if(lightLevels.w.conditionName.Contains("rain") || lightLevels.w.conditionName.Contains("storm")&&!lightLevels.w.isIndoors) makeSound(splashStep);
		else makeSound(regularStep);
	}

	public void puffL() {
		if (!isIndoors) {
			if(lightLevels.w.inSnow) Instantiate (footPrintPrefab, footPrintL.position, transform.rotation);
			else if(lightLevels.w.conditionName.Contains("rain") || lightLevels.w.conditionName.Contains("storm")&&!lightLevels.w.isIndoors) splashL.Play();
			else dustL.Play();
		}
		else dustL.Play();
	}

	public void puffR() {
		if (!isIndoors) {
			if(lightLevels.w.inSnow) Instantiate (footPrintPrefab, footPrintR.position, transform.rotation);
			else if(lightLevels.w.conditionName.Contains("rain") || lightLevels.w.conditionName.Contains("storm")&&!lightLevels.w.isIndoors) splashR.Play();
			else dustR.Play();
		}
		else dustR.Play();
	}

	public float getEnergy() {
		return gameData.annieCurrentEnergy;
	}

	public float getEnergyCost() {
		return energyCost;
	}
}
