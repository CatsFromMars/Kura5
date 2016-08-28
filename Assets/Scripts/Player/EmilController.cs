using UnityEngine;
using System.Collections;

public class EmilController : PlayerContainer {
	private int chargeCounter = 0;
	private int chargeThresh = 20;
	private int combo = 0;
	public Transform weaponHit;
	public AudioClip hitSound;
	public AudioClip wallHit;
	
	//AUDIO
	public AudioClip shootNoise;
	public AudioClip clickNoise;
	
	//VARIABLES REGARDING COMBAT
	public int strength = 10; //Base damage a weapon does
	public float meleeRange = 5.5f; //range of current weapon
	public float meleeAngle = 160f;
	public int meleeCost = 5;
	
	public Transform weapon;
	public MeleeWeaponTrail trail;

	private float burnRate = 1f;
	private float burnCounter = 0f;
	private float burnCounterTime = 20f;
	private float absorbRate = 1f;
	private float absorbCounter = 0f;
	private float absorbCounterTime = 150f;

	private float originalAgentSpeed;
	private float originalStoppingDistance;
	private float dashSpeed = 5f;
	private float dashStop = 6f;

	//"Sparkle" variables
	public Material bladeMat;
	public ParticleSystem darkMatter;
	public Animator smile;
	private int slashCounter = 0;
	private int smileThreshold = 21;
	private bool darkCharging = false;
	public ParticleSystem darkParticles;
	public GameObject darkenedFace;
	private bool isSmiling;
	
	void Start() {
		changeWeaponColor ();
		originalAgentSpeed = agent.speed;
		originalStoppingDistance = agent.stoppingDistance;
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
			checkForLensSwap();
			//handleCape();
			absorb(1);
			//handleSlasherSmileEffect();
			changeWeaponColor();
		}
	}

//	void handleSlasherSmileEffect() {
//		if(slashCounter >= smileThreshold) {
//			smile.gameObject.SetActive(true);
//			darkenedFace.gameObject.SetActive(true);
//		}
//		else if(slashCounter >= smileThreshold+5) {
//			smile.gameObject.SetActive(false);
//			darkenedFace.gameObject.SetActive(false);
//			slashCounter = 0;
//		}
//	}

//	void handleCape() {
//		bool capeWings = currentAnim (hash.rollState) || currentAnim(hash.hurtState);
//		if(capeWings) toggleBlendShape(75);
//		else if(mesh.GetBlendShapeWeight(0) >= 75) toggleBlendShape(0);
//	}

	void handleBurning() {
		if(lightLevels.sunlight > 0 && !lightLevels.w.isNightTime && !inCoffin && (!currentAnim(Animator.StringToHash("Base Layer.Switch")))) {
			if(!burning) {
				burning = true;
				animator.SetTrigger(Animator.StringToHash("Burn"));
			}
			burnCounterTime = 100 * 1/lightLevels.sunlight;
			takeSunDamage(burnRate);
			if(!smoke.isPlaying) smoke.Play();
		}
		else {
			burning = false;
			if(smoke.isPlaying) smoke.Stop();
		}
	}

	void takeSunDamage(float rate) {
		burnCounter += Time.timeScale;
		if(burnCounter >= burnCounterTime) {
			gameData.emilCurrentLife -= rate;
			burnCounter = 0f;
		}
		
		if(gameData.emilCurrentLife <= 0 && !dead) {
			playVoiceClip(dieVoices[Random.Range(0, dieVoices.Length)]);
			Die(); //KILL PLAYER IF GAME OVER.	
		}
	}

	void absorb(float rate) {
		if(lightLevels.darkness > 0) absorbCounter+=lightLevels.darkness.GetValue();
		else absorbCounter = 0;
		if(absorbCounter>absorbCounterTime) {
			gameData.emilCurrentEnergy += rate;
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
			if(parrying) {
				doParry();
			}
			else parryCounter = 0;
			targetEnemy();
			if(currentTarget != null) {
				if(lockOn == null) lockOn = Instantiate (lockOnUI, currentTarget.transform.position, Quaternion.identity) as Transform;
				else lockOn.transform.position = currentTarget.transform.position;
				zoomToEnemy();

				//Have Emil dash slash some baddies
				if(currentAnim(hash.comboState1)||currentAnim(hash.comboState2)||currentAnim(hash.comboState3)) {
					agent.speed = dashSpeed;
					agent.stoppingDistance = dashStop;
					agent.SetDestination(currentTarget.transform.position);
					//toggleBlendShape(100);
				}
				else {
					agent.ResetPath();
					agent.speed = originalAgentSpeed;
					agent.stoppingDistance = originalStoppingDistance;
					//toggleBlendShape(0);
				}
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
			parryCounter = 0;
			if(lockOn != null) {
				Destroy(lockOn.gameObject);
				zoomToPlayer();
			}
			if(!dead && agent.hasPath) {
				agent.ResetPath();
				agent.speed = originalAgentSpeed;
				agent.stoppingDistance = originalStoppingDistance;
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

		if(currentAnim(hash.comboState1) && gameData.emilCurrentElem != GameData.elementalProperty.Null) {
			if (gameData.emilCurrentEnergy > meleeCost) {
				gameData.emilCurrentEnergy -= meleeCost;
			}
			else gameData.emilCurrentElem = GameData.elementalProperty.Null;
		}
		changeWeaponColor();

		//Raycast
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeRange);

		for(int i=0; i<hitColliders.Length; i++) {
			Transform target = hitColliders[i].transform;
			Vector3 targetDir = (target.position - transform.position).normalized;
			if(!hitColliders[i].isTrigger && Vector3.Angle(transform.forward, targetDir) < meleeAngle/2) {
				float dist = Vector3.Distance(transform.position, target.position);
				RaycastHit hit;
				if(Physics.Raycast (transform.position, targetDir, out hit, dist)) {
					if(hit.collider.gameObject.tag == "Enemy") {
						hitEnemy(hit);
						if(checkForBossSegment(hit)) break;
					}
					else if(hit.collider.gameObject.tag == "Breakable") Smash(hit);
					else if(hit.collider.gameObject.tag == "Wall") hitWall(hit);
			
				}
			}
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
			bladeMat.SetColor("_OutlineColor", c);
		}
		else if(gameData.emilCurrentElem == GameData.elementalProperty.Frost) { 
			c = Color.cyan;
			bladeMat.SetColor("_Color", Color.black);
			bladeMat.SetColor("_OutlineColor", c);
		}
		else if(gameData.emilCurrentElem == GameData.elementalProperty.Cloud) { 
			c = Color.magenta;
			bladeMat.SetColor("_Color", Color.black);
			bladeMat.SetColor("_OutlineColor", c);
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


	bool checkForBossSegment(RaycastHit hit) {
		return hit.collider.GetComponent<EnemyClass>() != null;
	}
	
	void hitEnemy(RaycastHit hit) {
		//Ordinary Enemies
		EnemyClass enemy = hit.collider.GetComponent<EnemyClass>();
		//if it's a boss segment...
		if(enemy == null) enemy = hit.collider.transform.root.GetComponent<EnemyClass>();

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
			else {
				BossSegment boss = hit.collider.GetComponent<BossSegment>();
				if(boss != null && gameData.emilCurrentElem == GameData.elementalProperty.Dark) boss.bossParent.ShadowSeal();
			}
		}
		//Sparkly Effects and Sound
		makeSound(hitSound);
		Instantiate (weaponHit, hit.transform.position, Quaternion.identity);
		enemy.superEffectiveSmoke (enemy.element, element);
		//Blood? Blood.
		Transform blood = hit.collider.transform.FindChild ("Blood");
		if(blood!=null) blood.GetComponent<ParticleSystem>().Play();

		//finally, boost the smile counter
		slashCounter++;
	}
	
	public void Charge() {
		darkCharging = Time.timeScale != 0 && lightLevels.darkness > 0 && gameData.emilCurrentEnergy < gameData.emilMaxEnergy && currentAnim(hash.chargeState);
		chargeCounter+=(lightLevels.darkness.GetValue()*Mathf.RoundToInt(Time.timeScale));
		if(Time.timeScale != 0 && lightLevels.darkness > 0 && gameData.emilCurrentEnergy < gameData.emilMaxEnergy && !audio.isPlaying) makeSound(chargingSound);
		if(chargeCounter > chargeThresh) {
			chargeCounter = 0;
			if(lightLevels.darkness > 0 && gameData.emilCurrentEnergy < gameData.emilMaxEnergy) {
				darkMatter.Emit (1);
				gameData.emilCurrentEnergy += 1;
			}
		}
	}

	public void voiceIfCharging(AudioClip clip) {
		//ANIMATION EVENTS FOR VOICE ACTING
		if(Time.timeScale != 0 && lightLevels.darkness > 0 && gameData.emilCurrentEnergy < gameData.emilMaxEnergy) {
			voice.volume = 0.9f;
			voice.pitch = 1f;
			voice.clip = clip;
			voice.Play();
		}
	}

	override protected void getHurt(int damage, Vector3 knockbackDir) {
		gameData.emilCurrentLife -= damage;
		
		if(gameData.emilCurrentLife <= 0) {
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

	public float getEnergy() {
		return gameData.emilCurrentEnergy;
	}
}
