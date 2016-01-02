using UnityEngine;
using System.Collections;

public class EmilController : PlayerContainer {
	private int chargeCounter = 0;
	private int chargeThresh = 20;
	private int combo = 0;
	private float meleeRange = 5.5f;
	public Transform weaponHit;
	public AudioClip hitSound;
	public AudioClip wallHit;
	
	//AUDIO
	public AudioClip shootNoise;
	public AudioClip clickNoise;
	
	//VARIABLES REGARDING SHOOTING/COMBAT
	public Transform weapon;
	public int strength = 10;
	public MeleeWeaponTrail trail;
	private float burnRate = 1f;
	private float burnCounter = 0f;
	private float burnCounterTime = 20f;


	//"Sparkle" variables
	public Material bladeMat;
	public ParticleSystem darkMatter;
	
	void Start() {
		changeWeaponColor ();
	}
	// Update is called once per frame
	void Update () {
		if (playerInControl) {
			updateInput();
			updateAnimations();
			handleCombos();
			handleTargeting();
			handleParticeEffects();
			handleBurning();
			//handleCape();
		}
	}

	void handleCape() {
		bool capeWings = currentAnim (hash.rollState) || currentAnim(hash.hurtState);
		if(capeWings) toggleBlendShape(75);
		else if(mesh.GetBlendShapeWeight(0) >= 75) toggleBlendShape(0);
	}

	void handleBurning() {
		if(lightLevels.sunlight > 0 && !lightLevels.w.isNightTime) {
			burnCounterTime = 100 * 1/lightLevels.sunlight;
			takeSunDamage(burnRate);
			if(!smoke.isPlaying) smoke.Play();
		}
		else if (smoke.isPlaying) smoke.Stop();
	}

	void takeSunDamage(float rate) {
		burnCounter++;
		if(burnCounter >= burnCounterTime) {
			gameData.emilCurrentLife -= rate;
			burnCounter = 0f;
		}
		
		if(gameData.emilCurrentLife <= 0) Die(); //KILL PLAYER IF GAME OVER.	
	}

	void handleTargeting() {
		if(targeting) {
			//Targeting Mode
			if(parrying) {
				doParry();
			}
			else parryCounter = 0;
			targetEnemy();
			if(currentTarget != null) {
				if(lockOn == null) lockOn = Instantiate (lockOnUI, currentTarget.transform.position, Quaternion.identity) as Transform;
				else lockOn.transform.position = currentTarget.transform.position;
			}
		}
		else {
			//NON-Targeting Mode
			untargetEnemy();
			if (currentAnim(hash.hurtState)) knockBack(currentKnockbackDir);
			else if(charging) Charge();
			parryCounter = 0;
			if(lockOn != null) {
				Destroy(lockOn.gameObject);
			}
		}
	}

	void handleParticeEffects() {
		//Sword Slash Effect
		if(combo > 0 || currentAnim(hash.comboState1)) trail.Emit = true;
		else trail.Emit = false;
	}

	void handleCombos() {
		//HANDLING ATTACK COMBOS
		if(Input.GetButtonDown("Attack") && currentAnim(hash.comboState1)) combo=1;
		else if(Input.GetButtonDown("Attack") && currentAnim(hash.comboState2)) combo=2;
		else if(currentAnim(hash.holdWeaponState) || currentAnim(hash.idleState) || currentAnim(hash.targetState)) combo = 0;
		animator.SetInteger (hash.comboInt, combo);
	}

	void Slash() {
		if (gameData.emilCurrentEnergy > 5) {
			gameData.emilCurrentEnergy -= 5;
			element = gameData.emilCurrentElem.ToString();
		}
		else gameData.emilCurrentElem = GameData.elementalProperty.Null;

		changeWeaponColor();

		//Animation Event for swordplay
		RaycastHit hitCenter;
		RaycastHit hitRight;
		RaycastHit hitLeft;
		//Forward
		if (Physics.Raycast (transform.position, transform.forward, out hitCenter, meleeRange)) {
			if(hitCenter.collider.gameObject.tag == "Enemy") hitEnemy(hitCenter);
			else if(hitCenter.collider.gameObject.tag == "Breakable") Smash (hitCenter);
			else if(hitCenter.collider.gameObject.tag == "Wall") hitWall(hitCenter);
			return;
		}
		//Left
		if (Physics.Raycast (transform.position, Quaternion.AngleAxis(15, transform.up) * transform.forward, out hitRight, meleeRange)) {
			if(hitRight.collider.gameObject.tag == "Enemy") hitEnemy(hitRight);
			else if(hitRight.collider.gameObject.tag == "Breakable") Smash (hitRight);
			else if(hitRight.collider.gameObject.tag == "Wall") hitWall(hitRight);
			return;
		}
		if (Physics.Raycast (transform.position, Quaternion.AngleAxis(45, transform.up) * transform.forward, out hitRight, meleeRange)) {
			if(hitRight.collider.gameObject.tag == "Enemy") hitEnemy(hitRight);
			else if(hitRight.collider.gameObject.tag == "Breakable") Smash (hitRight);
			return;
		}
		//Right
		if (Physics.Raycast (transform.position,  Quaternion.AngleAxis(-15, transform.up) * transform.forward, out hitLeft, meleeRange)) {
			if(hitLeft.collider.gameObject.tag == "Enemy") hitEnemy(hitLeft);
			else if(hitLeft.collider.gameObject.tag == "Breakable") Smash (hitLeft);
			else if(hitLeft.collider.gameObject.tag == "Wall") hitWall(hitLeft);
			return;
		}
		if (Physics.Raycast (transform.position,  Quaternion.AngleAxis(-45, transform.up) * transform.forward, out hitLeft, meleeRange)) {
			if(hitLeft.collider.gameObject.tag == "Enemy") hitEnemy(hitLeft);
			else if(hitLeft.collider.gameObject.tag == "Breakable") Smash (hitLeft);
			return;
		}
	}

	void CircleSlash() {
		//For Emil's final combo
	}

	void changeWeaponColor() {
		//Do this on element change later on. For now, do it during slash
		Color c;
		if(gameData.emilCurrentElem == GameData.elementalProperty.Dark) { 
			c = Color.red;
			bladeMat.SetColor("_Color", Color.black);
			bladeMat.SetColor("_OutlineColor", Color.red);
		}
		else {
			c = Color.white;
			bladeMat.SetColor("_Color", Color.gray);
			bladeMat.SetColor("_OutlineColor", Color.black);
		}
		trail._material.SetColor("_TintColor", c);
	}

	void Smash(RaycastHit hit) {
		GameObject breakable = hit.collider.gameObject;
		if(breakable != null) breakable.GetComponent<Breakable>().Shatter();
	}

	void hitWall(RaycastHit hit) {
		makeSound (wallHit);
		Instantiate (soundEffect, hit.point, Quaternion.identity);
	}
	
	void hitEnemy(RaycastHit hit) {
		//Ordinary Enemies
		EnemyClass enemy = hit.collider.GetComponent<EnemyClass>();

		//Insta-kill Ivy
		Ivy ivy = hit.collider.GetComponent<Ivy>();
		if(ivy != null && gameData.emilCurrentElem == GameData.elementalProperty.Dark) ivy.hitWithDarkAttack = true;

		int dmg = damageCalculator.getDamage(gameData.emilCurrentElem.ToString(), enemy.element, strength, 1);
		enemy.takeDamage (dmg, gameData.emilCurrentElem.ToString());
		enemy.knockback (transform.forward);

		//Shadow Stun Code
		if(lightLevels.darkness > 0) {
			PatrolEnemy patrol = hit.collider.GetComponent<PatrolEnemy>();
			if(patrol != null && gameData.emilCurrentElem == GameData.elementalProperty.Dark) patrol.Freeze();
		}
		//Sparkly Effects and Sound
		makeSound(hitSound);
		Instantiate (weaponHit, hit.transform.position, Quaternion.identity);
		enemy.superEffectiveSmoke (enemy.element, element);
		//Blood? Blood.
		Transform blood = hit.collider.transform.FindChild ("Blood");
		if(blood!=null) blood.GetComponent<ParticleSystem>().Play();
	}
	
	public void Charge() {
		chargeCounter+=lightLevels.darkness*Mathf.RoundToInt(Time.timeScale);
		if(Time.timeScale != 0 && lightLevels.darkness > 0 && gameData.emilCurrentEnergy < gameData.emilMaxEnergy && !audio.isPlaying) makeSound(chargingSound);
		if(chargeCounter > chargeThresh) {
			chargeCounter = 0;
			if(lightLevels.darkness > 0 && gameData.emilCurrentEnergy < gameData.emilMaxEnergy) {
				darkMatter.Emit (1);
				gameData.emilCurrentEnergy += 1;
			}
		}
	}

	override protected void getHurt(int damage, Vector3 knockbackDir) {
		gameData.emilCurrentLife -= damage;
		
		if(gameData.emilCurrentLife <= 0) Die();
		else {
			//Play knockback anim
			animator.SetTrigger(hash.hurtTrigger);
		}
	}
}
