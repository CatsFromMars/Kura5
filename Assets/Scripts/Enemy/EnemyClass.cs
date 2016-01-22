using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyClass : MonoBehaviour {
	//LOOT VARIABLES
	public Transform commonLoot;
	public Transform rareLoot;
	//STAT VARIABLES
	public float maxLife;
	public float currentLife;
	public int strength;
	protected bool frozen;
	protected float freezeTimer; //THIS HANDLES EMIL'S SPELLS
	protected float freezeWaitTime = 400f;
	protected bool canSee = true;
	protected bool canHear = true;
	protected string enemyName;
	public EnemyType type;
	public string element;
	protected Shader regularShader;
	protected Shader blinkShader;
	protected Transform blinker;
	protected Slider lifeBar;

	//DETECTION VARIABLES
	protected bool trackingPlayer = false;
	protected bool playerDetected = false;
	protected bool caution = false;
	protected float fieldOfViewAngle = 40f;
	protected float sightRange = 13.0f;
	protected Vector3 lastSighting;
	protected Vector3 playerPos;
	protected SphereCollider col;
	protected Transform player;
	protected GameObject playerObject;
	protected Animator playerAnimator;
	protected Vector3 previousSighting; //PLAYER SIGHED IN PREVIOUS FRAME
	//DETECTION VARIABLES CARRIED FROM PATROL
	protected Vector3 playerLastSighting = new Vector3(1000,1000,1000); //Default pos
	protected Vector3 resetPlayerPosition = new Vector3(1000,1000,1000); //Default pos
	protected bool playerInSight;
	
	//NAVIGATION VARIABLES
	protected NavMeshAgent agent;

	//DATA VARIABLES
	protected AudioSource audio;
	protected Rigidbody rigidbody;
	protected GameObject globalData;
	protected GameData gameData;
	protected Animator animator;
	protected HashIDs hash;
	public LightLevels lightLevels;
	public DamageCalculator damageCalculator;
	
	//ACTION VARIABLES
	//protected bool attacking;
	protected bool dying;
	protected bool dead;
	public bool stunned = false;
	//protected bool chasing;
	protected bool hurtCaution;
	protected bool okayToAttack = true;
	protected bool isBeingTargeted;
	protected int hitCounter = 0;
	protected bool isInvincible = false;
	protected string mostRecentAttackElem = "Null";

	//ENVIRONMENT VARIABLES
	public SunDetector sunDetector;
	protected Transform sunCollider; //HANDLES SUNLIGHT INTERACTION
	protected bool inSunlight = false;
	protected bool inShadow = false;
	protected float burnRate = 1f;
	protected float burnCounter = 0f;
	protected float burnCounterTime = 5f;

	public Transform body; //THE ACTUAL MESH OF AN ENEMY
	protected Material[] materials;
	protected Color[] colors;

	//VISUAL VARIABLES
	public Transform shadowStunEffect;
	public ParticleSystem shadowStunParticles;

	public enum EnemyType {
		Undead,
		Phantom,
		Beast,
		Robot
	}

	void Awake () {
		blinker = transform.FindChild ("Flash");
		if(blinker == null) Debug.LogError ("EnemyFlash Projector Not Found!");
		rigidbody = GetComponent<Rigidbody> ();
		globalData = GameObject.FindGameObjectWithTag("GameController");
		damageCalculator = globalData.GetComponent<DamageCalculator>();
		animator = GetComponent<Animator>();
		hash = globalData.GetComponent<HashIDs>();
		gameData = globalData.GetComponent<GameData>();
		agent = GetComponent<NavMeshAgent>();
		blinkShader = Shader.Find("Reflective/Bumped Diffuse");
		lightLevels = GameObject.FindGameObjectWithTag("LightLevels").GetComponent<LightLevels>();
		audio = GetComponent<AudioSource>();
		lifeBar = GameObject.Find ("EnemyLife").GetComponent<Slider>();

		//GET PLAYER
		playerObject = GameObject.FindWithTag("Player"); //Get player container
		if(playerObject != null) {
			player = playerObject.transform;
			playerAnimator = player.GetComponent<Animator>();
		}
		col = GetComponent<SphereCollider>();

		if (body != null) {
			materials = body.GetComponent<SkinnedMeshRenderer>().materials;
			colors = new Color[materials.Length];
			storeColors();
			regularShader = materials[0].shader;
		}
	}

	protected void storeColors() {
		for(int i=0; i<materials.Length; i++) {
			colors[i] = materials[i].color;
		}
	}

	//COLLISION FUNCTIONS.
	void OnCollisionEnter(Collision collision) {

		if (collision.collider.gameObject.tag == "Bullet") {
			Bullet bullet = collision.collider.gameObject.GetComponent<Bullet>();
			int dmg = damageCalculator.getDamage(bullet.element, element, bullet.damage, 1);
			takeDamage(dmg, bullet.element);
			superEffectiveSmoke(element, bullet.element);
		}

		if (collision.collider.gameObject.tag == "EnemyWeapon") {
			WeaponData weapon = collision.collider.gameObject.GetComponent<WeaponData>();
			int dmg = damageCalculator.getDamage(weapon.element, element, weapon.damage, 1);
			//Get stunned from friendly fire
			stunned = true;
			animator.SetTrigger(hash.hurtTrigger);
			animator.SetBool(hash.stunnedBool, true);
			takeDamage(dmg, weapon.element);

			superEffectiveSmoke(element, weapon.element);
		}
		
	}

	public void superEffectiveSmoke(string e1, string e2) {
		if((damageCalculator.getElementFromString(e1).opposite) == (damageCalculator.getElementFromString(e2).name)) {
			Instantiate((Resources.Load("Effects/SESmoke")), transform.position, Quaternion.Euler(-90,0,0));
		}
	}

	bool isHitFromBehind() {
		Vector3 toTarget = (playerPos - transform.position).normalized;
		if (Vector3.Dot(toTarget, transform.forward) > 0) return false;
		else return true;
	}

	//COMBAT: TAKE DAMAGE
	public void takeDamage(int damage, string element="Null") {
		hitCounter++;
		StartCoroutine(flashWhite());
		mostRecentAttackElem = element;
		if(!dead || !dying)
		{
			//MAKE THEM REACT TO PAAAAINNNN
			if(isHitFromBehind()) {
				stunned = true;
				animator.SetTrigger(hash.hurtTrigger);
				animator.SetBool(hash.stunnedBool, true);
			}
			else if(animator.GetCurrentAnimatorStateInfo(0).nameHash != hash.enemyHurtState
			        && animator.GetCurrentAnimatorStateInfo(0).nameHash != hash.attackState) animator.SetTrigger(hash.hurtTrigger);
			currentLife -= damage; //DEDUCT DAMAGE RATE FROM CURRENT LIFE


			if(currentLife <= 0) Die();
			else {
				hurtCaution = true; //LOOK FOR THE THING THAT HIT THEM!
				//animator.SetBool(hash.turningBool, true);
			}

			//Look at player always 
			Vector3 p = player.transform.position;
			Vector3 pos = new Vector3(p.x, this.transform.position.y, p.z);
			transform.LookAt(pos);

			//Update Enemy HP Bar
			lifeBar.maxValue = maxLife;
			lifeBar.value = currentLife;
		}

	}

	protected IEnumerator flashWhite () {
		blinker.active = true;
		isInvincible = true;
		yield return new WaitForSeconds(0.2f);
		blinker.active = false;
		isInvincible = false;
	}

	protected void takeSunDamage(int damage) {
		currentLife -= damage; //DEDUCT DAMAGE RATE FROM CURRENT LIFE
		if(currentLife <= 0) Die();
	}

	protected void DestroySelf() {
		//ANIMATION EVENT FOR THE END OF DEATH
		Destroy(this.gameObject);
	}

	protected void Die() {
		dying = true;
		if(agent!=null) agent.speed = 0;
		if(rigidbody!=null) rigidbody.velocity = Vector3.zero;
		animator.SetTrigger(hash.dyingTrigger);
		if (!dead) {
			dead = true;
			animator.SetBool (hash.deadBool, true);
		}
	}

	protected void spawnLoot() {
		bool playSound = true;
		int c = Random.Range (0, 11);
		if(c < 4 && commonLoot!=null) Instantiate(commonLoot, transform.position, commonLoot.transform.rotation);
		else if(c < 2 && rareLoot!=null) Instantiate(commonLoot, transform.position, rareLoot.transform.rotation);
		else playSound = false;
		if(playSound) {
			AudioClip sound = Resources.Load<AudioClip>("Sound Effects/Misc/Drop");
			Debug.Log(sound);
			makeSound(sound);
		}
	}

	protected void MarkAsDead(){
		//ANIMATION EVENT FOR START OF DEATH
		dead = true;
		animator.SetBool (hash.deadBool, true);
	}


	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Shadow") { //SHADOW OVERRIDES SUNLIGHT TRIGGER
			inShadow = true;
			inSunlight = false;
			
		}
		
		else if (other.gameObject.tag == "Skylight") {
			inSunlight = true;
			
		}
		
	}
	
	void OnTriggerExit(Collider other) {
		
		if (other.gameObject.tag == "Skylight") {
			inSunlight = false;
			
		}
		
		if (other.gameObject.tag == "Shadow") {
			inShadow = false;
			
		}
		
		
	}
	
	protected void takeSunDamage(float rate) {
		burnCounter++;
		if(burnCounter >= burnCounterTime) {
			currentLife -= rate;
			burnCounter = 0f;
		}
		
	}

	protected void setPitch(int pitch) {
		audio.pitch = pitch;
	}

	protected void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		if(audio.enabled) {
			audio.clip = clip;
			audio.Play();
		}
	}

	protected void sendParryMessage() {
		//Intended for animation events
		//if(player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
		//if(player.GetComponent<PlayerContainer>().currentTarget == this.transform) player.GetComponent<PlayerContainer>().startParry();
	}

	public void knockback(Vector3 dir) {
		if(rigidbody != null) rigidbody.AddForce(dir * (rigidbody.mass*100));
	}

	protected void spawnEffect(Transform effect) {
		Instantiate(effect, transform.position, transform.rotation);
	}

	public void quickLook() {
		if(agent!=null) agent.updateRotation = false;
		Vector3 targetPos = new Vector3(playerPos.x, this.transform.position.y, playerPos.z);
		transform.rotation = Quaternion.LookRotation (targetPos - transform.position);
	}
	
}
