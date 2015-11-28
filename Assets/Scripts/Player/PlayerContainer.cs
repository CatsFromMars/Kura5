using UnityEngine;
using System.Collections;

public class PlayerContainer : MonoBehaviour {

	//MOVEMENT
	public float playerSpeed = 8;
	public float playerTargetingSpeed = 4f;
	public float playerRunningSpeed = 8f;
	public float playerRollingSpeed = 18f;
	protected float vertical;
	protected float horizontal;
	public bool forceLookAtTarget = true;
	protected Vector3 currentKnockbackDir;

	//TARGETING VARIABLES
	public Transform lockOnUI;
	protected Transform lockOn;
	public Transform currentTarget;
	protected Animator currentTargetAnimator;

	//ACTION VARIABLES
	protected bool dead = false;
	public bool targeting = false;
	protected bool moving = false;
	protected bool charging = false;
	public bool rolling = false;
	protected bool attacking = false;
	protected bool whistling = false;
	protected bool holdingWeapon = false;
	protected bool performingAction = false;
	public bool parrying = false;

	//VARIABLES REGARDING WHETHER AN ACTION SHOULD BE PERFORMED
	protected bool ableToPush = false;
	protected bool ableToMove = true;
	protected bool ableToRotate = true;
	public bool playerInControl = true;
	protected bool isAttacking = false;
	public bool inSunlight = false;
	public bool inShadow = false;
	protected int parryCounter = 0;
	private int parryWaitTime = 2;
	protected bool canParry = true;
	public bool invincible = false;
	private float invincibilityFrames = 0.5f;

	//ANIMATION AND DATA VARIABLES
	public GameObject soundEffect;
	public AudioClip chargingSound;
	private Transform blinker;
	public ParticleSystem smoke;
	public AudioClip parryAlertNoise;
	protected Animator animator;
	protected NavMeshAgent agent;
	protected CharacterController controller;
	protected HashIDs hash;
	protected GameData gameData;
	protected GameObject globalData;
	public LightLevels lightLevels;
	public DamageCalculator damageCalculator;

	//MISC VARIABLES
	public species playerSpecies;
	public string element = "Sol";
	public AudioClip hurt;
	
	public enum species 
	{
		Human, Vampire
	}

	void Awake () {
		//ACCESS TO HASHES, ANIMATOR AND GLOBAL DATA
		globalData = GameObject.FindGameObjectWithTag("GameController");
		hash = globalData.GetComponent<HashIDs>();
		gameData = globalData.GetComponent<GameData>();
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		lightLevels = GameObject.FindGameObjectWithTag("LightLevels").GetComponent<LightLevels>();
		damageCalculator = globalData.GetComponent<DamageCalculator>();
		agent = GetComponent<NavMeshAgent> ();
		blinker = transform.FindChild ("Flash");
		Vector3 startingPos = new Vector3(transform.position.x, 0, transform.position.z);
		transform.position = startingPos;
	}

	void FixedUpdate() {
		//updateInput();
		//updateAnimations();
		if(moving || currentAnim(hash.rollState)) manageMovement();
		if(!rolling) {
			updateInputDirection();
		}
		else if(rolling) {
			if(vertical == 0 && horizontal == 0) {
				vertical = transform.forward.z;
				horizontal = transform.forward.x;
			}
		}
	}

	protected void manageMovement () {
		ableToRotate = (!(charging || dead || whistling || targeting&&forceLookAtTarget || targeting&&!moving)) && !(currentAnim(hash.comboState1) ||                                                               currentAnim(hash.comboState3) ||
		                                                                    currentAnim(hash.shootingState));
		//Speed
		if(rolling) playerSpeed = playerRollingSpeed;
		else if(targeting) playerSpeed = playerTargetingSpeed;
		else playerSpeed = playerRunningSpeed;

		//Actually move and rotate player
		if(currentAnim(hash.runningState) || currentAnim(hash.rollState) || (currentAnim(hash.targetState))) movePlayer (horizontal, vertical);
		if(ableToRotate) rotatePlayer (horizontal, vertical);
	}

	protected void updateAnimations() {
		if(parrying) {
			animator.SetTrigger (hash.blockTrigger);
			StartCoroutine(parryCooldown());
		}
		animator.SetBool(hash.movingBool, moving);
		animator.SetBool(hash.taiyouBool, charging);
		animator.SetBool(hash.whistleBool, whistling);
		animator.SetBool(hash.rollingBool, Input.GetButtonDown("Roll"));
		animator.SetBool(hash.holdWeaponBool, holdingWeapon);
		animator.SetBool(hash.attackBool, attacking);
		animator.SetBool(hash.targetingBool, targeting);
	}

	protected void updateInput() {
		if(playerInControl) {
			parrying = (canParry && Input.GetButtonDown("Block") && targeting);
			charging = Input.GetButton("Charge");
			rolling = (currentAnim(hash.rollState));
			whistling = Input.GetButtonDown("Whistle");
			holdingWeapon = Input.GetButton("Attack") || Input.GetButtonDown("Attack");
			attacking = Input.GetButtonUp("Attack");
			targeting = Input.GetButton ("Target") && Time.timeScale != 0;
			moving = (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
			performingAction = attacking || holdingWeapon || whistling || charging;
		}
	}

	public void updateInputDirection() {
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");
	}

	public void movePlayer(float horizontal, float vertical) {
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		controller.Move(targetDirection * Time.deltaTime * playerSpeed);
	}

	public void rotatePlayer(float horizontal, float vertical) {
		
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, 100 * Time.deltaTime);
		
		transform.rotation = newRotation;
	}
	

	public void OnTriggerStay(Collider other) {
		
		if (other.gameObject.tag == "Shadow") { //SHADOW OVERRIDES SUNLIGHT TRIGGER
			inShadow = true;
			inSunlight = false;
		}
		
		else if (other.gameObject.tag == "Skylight") {
			inSunlight = true;
			
		}
	}
	
	public void OnTriggerExit(Collider other) {
		
		if (other.gameObject.tag == "Skylight") {
			inSunlight = false;
			
		}
		
		if (other.gameObject.tag == "Shadow") {
			inShadow = false;
			
		}
	}

	public void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "EnemyWeapon") {
			WeaponData weapon = other.collider.GetComponent<WeaponData>();
			int d = weapon.damage;
			string e = weapon.element;
			Vector3 k = weapon.knockBack * other.transform.forward;
			hitPlayer(d, e, k);
			
		}
		
	}

	IEnumerator startInvinciblity() {
		invincible = true;
		StartCoroutine (blink ());
		yield return new WaitForSeconds (invincibilityFrames);
		invincible = false;
	}

	IEnumerator parryCooldown() {
		canParry = false;
		yield return new WaitForSeconds (parryWaitTime);
		canParry = true;
	}

	IEnumerator blink() {
		while(invincible) {
			blinker.active = true;
			yield return new WaitForSeconds(0.1f);
			blinker.active = false;
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void hitPlayer(int d, string e, Vector3 knockback) {
		//To be called by melee enemies
		if(!invincible) {
			makeSound(hurt);
			StartCoroutine(startInvinciblity());
			int damage = damageCalculator.getDamage(e, element, d);
			currentKnockbackDir = knockback;
			if(!currentAnim(hash.blockState)) getHurt(damage, knockback);
			else Instantiate(Resources.Load("Effects/Parry") as GameObject, transform.position, Quaternion.identity);
		}
	}

	public void knockBack(Vector3 kdir) {
		controller.Move(kdir * Time.deltaTime * 2);
		//Vector3 elev = new Vector3(transform.position.x, yElevation, transform.position.z);
		//transform.position = elev;
		
	}
	
	protected virtual void getHurt(int damage, Vector3 knockbackDir) {
		//To be overrided in child class controller
	}
	
	protected void Die() {
		playerSpeed = 0; //CANT MOVE WHEN DEAD!
		animator.SetTrigger(hash.dyingTrigger);
		dead = true;
		//AudioSource audio = Camera.main.GetComponent<AudioSource>();
		//audio.clip = Resources.Load ("Sound Effects/Death") as AudioClip;
		//audio.Play();
	}

	protected void targetEnemy() {
		if(currentTarget == null) {
			GameObject closest = FindClosestEnemy();
			if(closest != null) { 
				currentTarget = closest.transform;
				currentTargetAnimator = currentTarget.GetComponent<Animator>();
			}
		}

		else {
			Vector3 targetPos = new Vector3(currentTarget.position.x, this.transform.position.y, 
			                                currentTarget.position.z);

			bool m = currentAnim(hash.runningState) || currentAnim(hash.rollState);
			if(forceLookAtTarget || !m) this.transform.LookAt(targetPos);
			else rotatePlayer (horizontal, vertical);
		}
	}

	protected void untargetEnemy() {
		currentTarget = null;
	}

	GameObject FindClosestEnemy() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance && objectOnScreen(go.transform)) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}

	private bool objectOnScreen(Transform target) {
		Vector3 viewPos = Camera.main.WorldToViewportPoint(target.position);
		if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z >= 0)
						return true;
		else return false;
	}
	
	public bool currentAnim(int hash) {
		return animator.GetCurrentAnimatorStateInfo(0).nameHash == hash;
	}

	//Gets sent as a message to tell when player can parry
	public void doParry() {
		parryCounter++;
	}
	
	public void faceCamera() {
		GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
		Vector3 pos = new Vector3(cam.transform.position.x,this.transform.position.y,cam.transform.position.z);
		transform.LookAt(pos);
	}

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
	}

	public void attemptSwap() {
		GameObject.FindGameObjectWithTag("PlayerSwapper").GetComponent<CharacterSwapper>().switchPlayers ();
	}
}
