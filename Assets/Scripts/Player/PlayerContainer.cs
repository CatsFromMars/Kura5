using UnityEngine;
using System.Collections;

public class PlayerContainer : MonoBehaviour {

	//MOVEMENT
	public float playerSpeed = 8;
	public float sidleSpeed = 7;
	public float playerTargetingSpeed = 4f;
	public float playerRunningSpeed = 8f;
	public float playerPullSpeed = 4f;
	public float playerRollingSpeed = 18f;
	private float rotationSpeed = 30f;
	private float sidleRotationSpeed = 10f;
	private Quaternion sidleRot;
	private int sidleCounter;
	private int sidleWaitTime = 5; //number of frames it takes to flatten against a wall
	protected float vertical;
	protected float horizontal;
	protected Vector3 lastNonZeroAxis;
	public bool forceLookAtTarget = true;
	protected Vector3 currentKnockbackDir;

	//TARGETING VARIABLES
	public Transform lockOnUI;
	protected Transform lockOn;
	public Transform currentTarget;
	protected Animator currentTargetAnimator;

	//ACTION VARIABLES
	public bool nearCoffin = false;
	public bool knockedOver = false;
	protected bool pullingCoffin = false;
	protected bool dead = false;
	public bool targeting = false;
	protected bool moving = false;
	protected bool sidling = false;
	protected bool charging = false;
	public bool rolling = false;
	protected bool attacking = false;
	protected bool whistling = false;
	protected bool holdingWeapon = false;
	public bool performingAction = false;
	public bool parrying = false;
	public bool isIndoors = false;
	protected bool inSnow = false;
	public bool inCoffin = false;
	public bool burning=false;
	protected bool canSidleLeft = false;
	protected bool canSidleRight = false;

	//VARIABLES REGARDING WHETHER AN ACTION SHOULD BE PERFORMED
	protected bool canCharge = true;
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
	private float invincibilityFrames = 1f;

	//ANIMATION AND DATA VARIABLES
	public ElementSwapping lensSwapper;
	public GameObject weaponObject;
	private MeshRenderer weaponMeshRenderer;
	public GameObject soundEffect;
	public AudioClip chargingSound;
	private Transform blinker;
	public ParticleSystem smoke;
	public AudioClip parryAlertNoise;
	private Transform follow;
	protected Animator animator;
	protected NavMeshAgent agent;
	protected CharacterController controller;
	protected Transform swapper;
	public HashIDs hash;
	protected GameData gameData;
	protected GameObject globalData;
	public LightLevels lightLevels;
	protected CamLooker looker; //Camera
	public DamageCalculator damageCalculator;
	public GameObject body;
	public GameObject meshObjects;
	public GameObject playerArmature;
	protected SkinnedMeshRenderer mesh;
	AudioSource audio;
	protected AudioSource voice;
	protected float originalVolume;
	protected float originalPitch;

	//MISC VARIABLES
	protected bool alreadySpawnedParryEffect = false;
	public species playerSpecies;
	public string element = "Sol";
	public AudioClip hurt;
	public AudioClip[] hurtVoices;
	public AudioClip[] dieVoices;
	public AudioClip[] rollVoices;
	public AudioClip[] switchVoices;
	
	public enum species 
	{
		Human, Vampire
	}

	void Awake () {
		//ACCESS TO HASHES, ANIMATOR AND GLOBAL DATA
		audio = GetComponent<AudioSource> ();
		globalData = GameObject.FindGameObjectWithTag("GameController");
		looker = GameObject.FindGameObjectWithTag("CamFollow").GetComponent<CamLooker>();
		follow = GameObject.FindWithTag("CamFollow").transform;
		hash = globalData.GetComponent<HashIDs>();
		gameData = globalData.GetComponent<GameData>();
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		lightLevels = GameObject.FindGameObjectWithTag("LightLevels").GetComponent<LightLevels>();
		damageCalculator = globalData.GetComponent<DamageCalculator>();
		agent = GetComponent<NavMeshAgent> ();
		blinker = transform.FindChild ("Flash");
		swapper = GameObject.FindWithTag("PlayerSwapper").transform;
		Vector3 startingPos = new Vector3(transform.position.x, 0, transform.position.z);
		transform.position = startingPos;
		voice = transform.FindChild ("Voice").GetComponent<AudioSource>();
		originalVolume = voice.volume;
		originalPitch = voice.pitch;
		if(body!=null) {
			mesh = body.GetComponent<SkinnedMeshRenderer>();
			toggleBlendShape(0);
		}
		meshObjects = transform.FindChild ("Body").gameObject;
		if(weaponObject!=null) weaponMeshRenderer = weaponObject.GetComponent<MeshRenderer>();
	}

	void OnEnable() {
		//When made active
		//aka when swapped out
		//if(GameObject.FindGameObjectsWithTag("Player").Length==1) {
		//	animator.SetTrigger (Animator.StringToHash ("Switch"));
		//	if(switchVoices.Length>0) playVoiceClip(switchVoices[Random.Range(0, switchVoices.Length)]);
		//}
		invincible = false;
		blinker.active = false;
		burning = false;
	}

	void OnLevelWasLoaded(int level) {
		invincible = false;
		blinker.active = false;
		StopAllCoroutines();
		Vector3 pos = transform.position + this.transform.forward * 3f;
		inSunlight = false;
		inShadow = false;
		untargetEnemy();
		zoomToPlayer();
		if(lockOn != null) {
			Destroy(lockOn.gameObject);
		}

	}

	void FixedUpdate() {
		updateInput();
		//updateAnimations();
		if(Time.timeScale==0) untargetEnemy();
		if(playerInControl) {
			if(moving || currentAnim(hash.rollState)) manageMovement();
			if(!rolling) {
				updateInputDirection();
			}
			else if(rolling) {
				if(vertical == 0 && horizontal == 0) {
					vertical = lastNonZeroAxis.x;
					horizontal = lastNonZeroAxis.y;
				}
			}
		}
	}

	void stopSpeed() {
		//used for sidling
		playerSpeed = 0;
	}

	void startSpeed() {
		//used for sidling
		playerSpeed = sidleSpeed;
	}

	protected void manageMovement () {
		ableToRotate = (!(knockedOver || charging || dead || whistling || targeting&&forceLookAtTarget || targeting&&!moving)) && !(currentAnim(hash.comboState1) ||                                                               currentAnim(hash.comboState3) ||
		                                                                    currentAnim(hash.shootingState));
		//Speed
		//if(sidling) playerSpeed = sidleSpeed;
		if(rolling) playerSpeed = playerRollingSpeed;
		else if(pullingCoffin || inCoffin) playerSpeed = playerPullSpeed;
		else if(targeting) playerSpeed = playerTargetingSpeed;
		else if (!sidling) playerSpeed = playerRunningSpeed;

		//Actually move and rotate player
		if(currentAnim(hash.runningState) || currentAnim(hash.pullingState) || currentAnim(hash.rollState) || currentAnim(Animator.StringToHash("Locomotion.Sidleright")) || currentAnim(Animator.StringToHash("Locomotion.Sidleleft")) || currentAnim(Animator.StringToHash("Combat.Burn")) || (currentAnim(hash.targetState))) movePlayer (horizontal, vertical);
		if(ableToRotate) rotatePlayer (horizontal, vertical);

		if(sidling) detectSidleEdges ();
	}

	protected void updateAnimations() {
		if(animator.GetBool(Animator.StringToHash("CutsceneMode"))) blinker.gameObject.SetActive(false);
		animator.SetBool(hash.pullingBool, pullingCoffin);
		if(Time.timeScale != 0) {
			if(parrying) {
				animator.SetTrigger (hash.blockTrigger);
				StartCoroutine(parryCooldown());
			}
			animator.SetBool(hash.movingBool, moving);
			animator.SetBool(hash.chargingTrigger,Input.GetButtonDown("Charge")); //Trigger Charging
			animator.SetBool(hash.taiyouBool, charging);
			//animator.SetBool(hash.whistleBool, whistling);
			animator.SetBool(hash.rollingBool, Input.GetButtonDown("Roll"));
			animator.SetBool(hash.holdWeaponBool, holdingWeapon);
			animator.SetBool(hash.attackBool, attacking);
			animator.SetBool(hash.targetingBool, targeting);
			animator.SetBool(Animator.StringToHash("DoingAnything"),Input.anyKey); //Put here for the idle animations
			animator.SetBool(Animator.StringToHash("Sidling"),sidling);
			//for sidling only
			animator.SetBool(Animator.StringToHash("Left"),(horizontal==-1)&&canSidleLeft&&sidling); //left and up
			animator.SetBool(Animator.StringToHash("Right"),(horizontal==1)&&canSidleRight&&sidling); //right and down
		}
		else {
			canCharge = false;
			animator.SetBool(hash.movingBool, false);
			animator.SetBool(hash.rollingBool, false);
			animator.SetBool(hash.taiyouBool, false);
			animator.SetBool(hash.holdWeaponBool, false);
			animator.SetBool(hash.targetingBool, false);
			animator.SetBool(Animator.StringToHash("Sidling"),false);
			animator.SetBool(Animator.StringToHash("DoingAnything"),true); //disable idle anim on cutcenes
		}
		weaponMeshRenderer.enabled = !inCoffin;
		if(meshObjects!=null && playerArmature!=null) {
			meshObjects.SetActive(!inCoffin);
			playerArmature.SetActive(!inCoffin);
		}
		if(performingAction||animator.GetBool(Animator.StringToHash("CutsceneMode"))) inCoffin = false;
		
	}

	protected void updateInput() {
		if(playerInControl) {
			parrying = (canParry && Input.GetButtonDown("Block") && targeting);
			if(!Input.GetButton("Charge")) canCharge = true;
			charging = canCharge && Input.GetButton("Charge") && !inCoffin && !gameData.nearInteractable;
			rolling = (currentAnim(hash.rollState));
			//whistling = Input.GetButtonDown("Whistle");
			holdingWeapon = ((Input.GetButton("Attack") || Input.GetButtonDown("Attack"))) && !gameData.nearInteractable;
			attacking = Input.GetButtonUp("Attack") && !gameData.nearInteractable;
			pullingCoffin = Input.GetButton("Charge") && nearCoffin;
			targeting = Input.GetButton ("Target") && Time.timeScale != 0 && !rolling;
			//if((Input.GetButtonDown("Target") || Input.GetButtonUp("Target")) && Time.timeScale != 0) handleTargetZoom();
			//if(sidling&&horizontal!=0&&vertical!=0) moving = false;
			moving = (horizontal != 0 || vertical != 0);
			performingAction = (attacking || holdingWeapon || whistling || charging || rolling || currentAnim(hash.hurtState)) && Time.timeScale != 0;
			//if(sidling&&!(currentAnim(hash.sidleState)||currentAnim(hash.sidleRightState)||currentAnim(hash.sidleLeftState))) sidling = false;
		}
		else canCharge = false;
	}

	bool checkForDirectionalForwardness() {
		//figure out as better way to handle this later
		//return false;
		if(transform.forward.z<0&&vertical>0) return true;
		else if(transform.forward.z>0&&vertical<0) return true;
		else return false;
	}

	public void updateInputDirection() {
		checkForDirectionalForwardness ();
		horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
		vertical = Mathf.RoundToInt(Input.GetAxis("Vertical"));
		//horizontal = (Input.GetAxis("Horizontal"));
		//vertical = (Input.GetAxis("Vertical"));
		if(horizontal != 0 || vertical !=0) lastNonZeroAxis = new Vector3 (horizontal, vertical, 0);
		if((horizontal==0&&vertical==0)||(checkForDirectionalForwardness())) sidling=false;
	}

	public void movePlayer(float horizontal, float vertical) {
		Vector3 targetDirection;
		if(sidling) {
			int m=1;
			if(transform.rotation.eulerAngles.y<=180)m=-1;
			if(horizontal*m > 0) controller.Move(transform.right*m * Time.deltaTime * playerSpeed);
			else if(horizontal*m < 0) controller.Move(transform.right*m*-1 * Time.deltaTime * playerSpeed);
		}
		else {
			targetDirection = new Vector3(horizontal, 0f, vertical);
			controller.Move(targetDirection * Time.deltaTime * playerSpeed);
		}
	}

	public void rotatePlayer(float horizontal, float vertical) {
		
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		Quaternion targetRotation = transform.rotation;
		if(!sidling) targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		else {
			targetRotation = sidleRot;
		}
		float rotSpeed;
		if(sidling) rotSpeed = sidleRotationSpeed;
		else rotSpeed = rotationSpeed;
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, rotSpeed * Time.fixedDeltaTime);
		transform.rotation = newRotation;
	}

	void hurtPlayer(WeaponData weapon, GameObject other) {
		int d = weapon.damage;
		string e = weapon.element;
		Vector3 k = weapon.knockBack * other.transform.forward;
		if(weapon.knockBack == 0) k = transform.forward*-3;
		hitPlayer(d, e, k);
	}

	public void OnParticleCollision(GameObject other) {
		Debug.Log (other.name);
		if (other.gameObject.tag == "EnemyWeapon") {
			WeaponData weapon = other.GetComponent<WeaponData>();
			hurtPlayer(weapon, other);
		}
	}

	public void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "EnemyWeapon") {
			WeaponData weapon = other.collider.GetComponent<WeaponData>();
			hurtPlayer(weapon, other.gameObject);
		}
	}

	public void OnTriggerStay(Collider other) {

		if (other.name == "Weatherbox") {
			isIndoors = other.GetComponent<SkylightWeather>().isIndoors;
			inSnow = other.GetComponent<SkylightWeather>().snowActive;
		}

		if (other.gameObject.tag == "EnemyWeapon") {
			WeaponData weapon = other.collider.GetComponent<WeaponData>();
			hurtPlayer(weapon, other.gameObject);
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

	protected void checkForLensSwap() {
		if(currentAnim(hash.idleState)) lensSwapper.checkForToggle();
	}

	public void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "EnemyWeapon") {
			WeaponData weapon = other.collider.GetComponent<WeaponData>();
			int d = weapon.damage;
			string e = weapon.element;
			Vector3 k = weapon.knockBack * other.transform.forward;
			if(weapon.knockBack == 0) k = transform.forward*-3;
			hitPlayer(d, e, k);
			
		}
		
	}

	public void startInvincibilityFrames() {
		StartCoroutine (startInvinciblity());
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
		alreadySpawnedParryEffect = false;
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
		if(!invincible && !dead) {
			int damage = damageCalculator.getDamage(e, element, d);
			currentKnockbackDir = knockback;
			if(!currentAnim(hash.blockState)) {
				getHurt(damage, knockback);
				makeSound(hurt);
				ShakeScreenAnimEvent.LittleShake();
				if(gameObject.activeSelf) StartCoroutine(startInvinciblity());
			}
			else if(!alreadySpawnedParryEffect) {
					Instantiate(Resources.Load("Effects/Parry") as GameObject, transform.position, Quaternion.identity);
					ShakeScreenAnimEvent.ShakeScreen();
					alreadySpawnedParryEffect = true;
				}
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
		playerSpeed = 0; //Can't move while dead
		//Have player kneel if partner is alive
		if(gameData.canSwapToAnnie&&gameData.canSwapToEmil) animator.SetTrigger(Animator.StringToHash("Kneel"));
		else animator.SetTrigger(hash.dyingTrigger);
		dead = true;
		//AudioSource audio = Camera.main.GetComponent<AudioSource>();
		//audio.clip = Resources.Load ("Sound Effects/Death") as AudioClip;
		//audio.Play();
	}

	protected void handleTargetZoom() {
		if(currentTarget != null&&currentTarget.gameObject.activeSelf) looker.zoomToTarget(currentTarget, 50);
		else if(looker.currentLook != this.transform && Time.timeScale != 0) zoomToPlayer();
	}

	protected void zoomToEnemy() {
		follow.parent = null;
		if(Time.timeScale!=0) looker.zoomToTarget(currentTarget, 25);
	}

	public void zoomToPlayer() {
		follow.parent = swapper;
		looker.zoomToTarget(this.transform, 50);
	}

	protected void targetEnemy() {
		if(currentTarget == null || !currentTarget.gameObject.activeSelf) {
			GameObject closest = FindClosestEnemy();
			if(closest != null) { 
				currentTarget = closest.transform;
				currentTargetAnimator = currentTarget.GetComponent<Animator>();
			}
		}

		else {
			Vector3 targetPos = new Vector3(currentTarget.position.x, this.transform.position.y, 
			                                currentTarget.position.z);

			bool m = currentAnim(hash.runningState) || currentAnim(hash.rollState) || currentAnim(hash.pullingState);
			if(forceLookAtTarget || !m) this.transform.LookAt(targetPos);
			else rotatePlayer (horizontal, vertical);
		}
	}

	protected void untargetEnemy() {
		currentTarget = null;
	}

	void swapElement(string e) {
		element = e;
	}

	protected GameObject FindClosestEnemy() {
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

	public void playVoiceClip(AudioClip clip) {
		//ANIMATION EVENTS FOR VOICE ACTING
		voice.volume = originalVolume;
		voice.pitch = originalPitch;
		voice.clip = clip;
		voice.Play();
	}

	public void switchVoiceClip() {
		if(switchVoices.Length>0) playVoiceClip(switchVoices[Random.Range(0, switchVoices.Length)]);
	}

	public void attemptSwap() {
		GameObject.FindGameObjectWithTag("PlayerSwapper").GetComponent<CharacterSwapper>().switchPlayers(true);
	}

	public void toggleBlendShape(float weight) {
		//Animation event intended for swapping shake keys on the player
		mesh.SetBlendShapeWeight (0, weight);
	}

	public void randomRollingSound() {
		//For rolling animation
		playVoiceClip(rollVoices[Random.Range(0, rollVoices.Length)]);
	}

	public void knockOver() {
		animator.SetTrigger (Animator.StringToHash("KnockedOver"));
	}

	void sidleRotation() {
		//Animation Event
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		RaycastHit hit;

		if(currentAnim(Animator.StringToHash("Locomotion.Sidleleft"))) {
			if (Physics.Raycast(transform.position-(transform.right), fwd, out hit, 1.5f)) {
				Vector3 rot = Quaternion.LookRotation(-hit.normal).eulerAngles;
				Vector3 r = transform.rotation.eulerAngles;
				r.y = rot.y;
				//Debug.Log("Rot: "+r);
				sidleRot = Quaternion.Euler(r);
			}
			else sidleRot = transform.rotation;
		}
		else if(currentAnim(Animator.StringToHash("Locomotion.Sidleright"))) {
			if (Physics.Raycast(transform.position+(transform.right), fwd, out hit, 1.5f)) {
				Vector3 rot = Quaternion.LookRotation(-hit.normal).eulerAngles;
				Vector3 r = transform.rotation.eulerAngles;
				r.y = rot.y;
				Debug.Log("Rot: "+r);
				sidleRot = Quaternion.Euler(r);
			}
			else sidleRot = transform.rotation;
		}
		else {
			sidleRot = transform.rotation;
		}
	}

	void correctPosition() {
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
		
		if (Physics.Raycast (transform.position, fwd, out hit, 2f)) {
			//Repositionplayer
			float m = 0.9f;
			Vector3 pos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
			if(playerSpecies == species.Human) m = 1f; //Offset for Annie
			Vector3 newPos = pos-(transform.forward*m);
			transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * sidleSpeed);
		}

	}

	void detectSidleEdges() {
		//Note: Layer mask these in the future for scenery, props, and default only
		Vector3 fwd = transform.TransformDirection(Vector3.forward);

		//Left
		if (Physics.Raycast(transform.position-(transform.right), fwd, 1.4f)) {
			canSidleLeft=true;
		}
		else canSidleLeft=false;
		//Right
		if (Physics.Raycast(transform.position+(transform.right), fwd, 1.4f)) { 
			canSidleRight=true;
		}
		else canSidleRight=false;

		if (!(canSidleLeft||canSidleRight)) sidling=false;

		correctPosition ();
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		//handle sidle
//		bool diag = (horizontal != 0 && vertical != 0);
//		//bool layered = hit.transform.gameObject.layer == LayerMask.NameToLayer ("Scenery");
//		bool tagged = hit.transform.gameObject.tag == "Wall";
//		if(!diag||sidling||!diag||inCoffin||!tagged) return;
//		//Debug.Log ("TRYING to sidle");
//		if((horizontal!=0&&vertical!=0)&&tagged&&!sidling&&currentAnim(hash.runningState)) sidleCounter++;
//		if((horizontal!=0&&vertical!=0)&&tagged&&!sidling&&sidleCounter>sidleWaitTime) {
//			Vector3 fwd = transform.TransformDirection(Vector3.forward);
//			RaycastHit h;
//			if (Physics.Raycast(transform.position, fwd, out h, 2f)) {
//				sidling = true;
//				sidleCounter=0;
//				playerSpeed=0;
//				Vector3 pos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
//				transform.rotation = Quaternion.LookRotation(-hit.normal);
//				//transform.position = pos;
//				float m = 0.9f;
//				if(playerSpecies == species.Human) m = 1.1f; //Offset for Annie
//				transform.position = pos-(transform.forward*m);
//				sidleRot = Quaternion.LookRotation(-hit.normal);
//				//Debug.Log("SIDLING");
//			}
//		}
	}

	public IEnumerator characterWalkTo(Vector3 target, Transform lookAt=null) {
		//Move player to target
		GameObject menu = GameObject.Find ("Menu");
		if(menu!=null) menu.SetActive (false);
		playerInControl = false;
		agent.SetDestination (target);
		transform.LookAt(target);
		agent.speed = playerRunningSpeed;
		while (Vector3.Distance(transform.position, target) >= 0.8f) {
			playerInControl = false;
			animator.SetBool(hash.holdWeaponBool, false);
			animator.SetBool(hash.ankouBool, false);
			animator.SetBool(hash.taiyouBool, false);
			animator.SetBool(hash.movingBool, true);
			yield return null;
		}
		agent.velocity = Vector3.zero;
		agent.Stop();
		animator.SetBool(hash.movingBool, false);
		if(lookAt!=null) {
			Vector3 pos = new Vector3 (lookAt.position.x, transform.position.y, lookAt.position.z);
			transform.LookAt(pos);
		}
		yield return new WaitForSeconds(0.3f);
		playerInControl = true;
		if(menu!=null) menu.SetActive (true);
	}

	public bool isPlayerMoving() {
		return moving;
	}

	public bool playerHiddenInCoffin() {
		return inCoffin && !moving;
	}

	public void revive() {
		dead = false;
		animator.Rebind();
	}
}
