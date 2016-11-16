using UnityEngine;
using System.Collections;

public class Ectoplasm : MonoBehaviour {

	public PurificationController purification;
	public Transform[] waypoints;
	public Generator[] generators;
	public enum ectoState {WANDER, DEACTIVATE, SEALED, VULNERABLE, ATTACK, STUNNED};
	public ectoState state = ectoState.WANDER;
	private NavMeshAgent agent;
	public Transform coffin;
	public Animator animator;
	public ShadowSeal seal;
	public GameObject burnSmoke;
	public GameObject shadowDrain;
	public GameObject attackEffect;
	
	public float normalBaseOffset = 3f;
	public float stunnedBaseOffset = 2.5f;
	public float normalSpeed = 3.5f;
	public float hitSpeed = 20f;
	public float attackSpeed = 20f;
	private float normalStoppingDistance = 2f;
	private float stunnedStoppingDistance = 0f;
	private float attackStoppingDistance = 0f;
	private float attackRotateSpeed = 30000f;
	private float normalRotateSpeed = 380f;

	private float hitTime = 0.15f;
	private float stunnedTime = 0.3f;
	private bool isHit = false;

	public float attackWaitTime = 4f;
	public float attackDuration = 3f;
	private float knockbackModifyer = 5f;
	private float burnDistance = 6f;

	private Vector3 lastHitForward = Vector3.zero; //For Knockback

	public GameObject immortalAttack;
	public GameObject immortalAttackDay;
	public GameObject immortalAttackNight;
	public GameObject immortalImage;

	private Generator targetGenerator;

	//Audio
	public AudioClip attackNoise;
	public AudioClip burnedClip;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		StartCoroutine(bossLoop());
		StartCoroutine (attack ());
	}

	void OnCollisionEnter(Collision collision) {
		
		if (collision.collider.gameObject.tag == "Bullet") {
			if(state==ectoState.SEALED) isHit = true;
			if(state==ectoState.ATTACK||state==ectoState.WANDER) {
				if(targetGenerator!=null) targetGenerator.state = Generator.generatorState.ACTIVATED;
				lastHitForward = collision.gameObject.transform.forward*knockbackModifyer;
				seal.ableToCharge = true;
				Debug.Log("Hit!");
				state = ectoState.STUNNED;
			}
		}
		
	}

	void Update() {
		if(state!=ectoState.ATTACK||purification.complete) attackEffect.SetActive(false);
		if(purification.complete) {
			immortalImage.SetActive(false);
			immortalAttackDay.SetActive(false);
			immortalAttackNight.SetActive(false);
			setEffect();
		}
	}

	IEnumerator bossLoop() {
		while(!purification.began) yield return null;
		animator.updateMode = AnimatorUpdateMode.Normal;
		while(!purification.complete) {
			yield return StartCoroutine(state.ToString());
		}
		Debug.Log("ENDED!");
	}

	public virtual IEnumerator SEALED() {
		while (state==ectoState.SEALED) {
			agent.updateRotation = false;
			//change elevation
			animator.SetBool(Animator.StringToHash("Stunned"),true);
			agent.Stop();
			agent.baseOffset = stunnedBaseOffset;
			agent.speed = 0;
			agent.stoppingDistance = stunnedStoppingDistance;
			if(isHit||(Vector3.Distance(transform.position,coffin.transform.position))<=(burnDistance/2f)) {
				agent.SetDestination(coffin.transform.position);
				agent.speed = hitSpeed;
				yield return new WaitForSeconds(hitTime);
				agent.speed = 0;
				isHit = false;
				yield return null;
				if(agent.remainingDistance <= agent.stoppingDistance) {
					//Initial damage
					purification.takeSealDamage();
					if(purification.inDarkMode) yield return StartCoroutine(sealDamage());
					else yield return StartCoroutine(generatorDamage());
					yield return null;
				}
			}
			agent.updateRotation = true;
			yield return null;
		}
	}

	public void setEffect() {
		Debug.Log ("setting...");
		if(purification.inDarkMode) shadowDrain.SetActive(true);
		else burnSmoke.SetActive(true);
	}

	IEnumerator generatorDamage() {
		Debug.Log("YIKES!");
		animator.SetBool(Animator.StringToHash("Sealed"),true);
		makeSound(burnedClip);
		immortalImage.SetActive(true);
		makeSound (burnedClip);
		burnSmoke.SetActive(true);
		yield return new WaitForSeconds(2.1f);
		if(seal.isActivated) seal.breakSeal();
		immortalImage.SetActive(false);
		animator.SetBool(Animator.StringToHash("Sealed"),false);
		animator.SetBool(Animator.StringToHash("Stunned"),false);
		burnSmoke.SetActive(false);
		state = ectoState.WANDER;
	}

	IEnumerator sealDamage() {
		//omnomnomnom
		seal.lockedIn = true;
		animator.SetBool(Animator.StringToHash("Sealed"),true);
		shadowDrain.SetActive(true);
		for(int i=0; i<3; i++) {
			yield return new WaitForSeconds(1f);
			purification.takeSealDamage();
			yield return null;
		}
		yield return new WaitForSeconds(2f);
		shadowDrain.SetActive(false);
		if(seal.isActivated) seal.breakSeal();
		animator.SetBool(Animator.StringToHash("Sealed"),false);
		animator.SetBool(Animator.StringToHash("Stunned"),false);
		state = ectoState.WANDER;
		seal.lockedIn = false;
	}

	public IEnumerator attack() {
		while(!purification.complete) {
			yield return new WaitForSeconds(attackWaitTime);
			if(purification.inDarkMode) immortalAttack = immortalAttackNight;
			else immortalAttack = immortalAttackDay;
			immortalAttack.SetActive(true);
			yield return new WaitForSeconds(attackDuration);
			immortalAttack.SetActive(false);
			yield return null;
		}
	}

	public virtual IEnumerator STUNNED() {
		while (state==ectoState.STUNNED) {
			animator.SetTrigger(Animator.StringToHash("Hit"));
			agent.updateRotation = false;
			agent.stoppingDistance = stunnedStoppingDistance;
			if(lastHitForward==Vector3.zero) lastHitForward=transform.forward*-1*knockbackModifyer;
			agent.SetDestination(transform.position+lastHitForward);
			agent.speed = hitSpeed;
			yield return new WaitForSeconds(hitTime);
			agent.speed = 0;
			if(!purification.inDarkMode&&(seal.isActivated||(Vector3.Distance(transform.position,coffin.transform.position))<=burnDistance)) {
				state = ectoState.SEALED;
				break;
			}
			yield return new WaitForSeconds(stunnedTime);
			lastHitForward = Vector3.zero;
			agent.updateRotation = true;
			state = ectoState.WANDER;
			yield return null;
		}
	}
	

	Generator getRandomActiveGenerator() {
		int len = 0;
		foreach (Generator g in generators) {
			if(g.state == Generator.generatorState.ACTIVATED) len++;
		}

		Generator[] activeGens = new Generator[len];
		if(len==0) return null;
		int index = 0;
		foreach (Generator h in generators) {
			if(h.state == Generator.generatorState.ACTIVATED) {
				activeGens[index] = h;
				index++;
			}
		}

		return activeGens[Random.Range(0, activeGens.Length-1)];
	}

	public virtual IEnumerator ATTACK() {
		bool attackingSeal = false;
		animator.SetBool(Animator.StringToHash("Stunned"),false);
		animator.SetBool(Animator.StringToHash("Attack"),true);
		while (state==ectoState.ATTACK) {
			makeSound(attackNoise);
			agent.stoppingDistance = attackStoppingDistance;
			agent.baseOffset = normalBaseOffset;
			agent.speed = attackSpeed;
			agent.angularSpeed = attackRotateSpeed;
			targetGenerator = getRandomActiveGenerator();
			if(targetGenerator!=null) agent.SetDestination(targetGenerator.transform.position);
			else {
				attackingSeal = true;
				agent.SetDestination(seal.transform.position);
			}

			while(agent.remainingDistance > agent.stoppingDistance) {
				if(state==ectoState.ATTACK) yield return null;
				else break;
			}
			if(seal.isActivated) {
				state = ectoState.SEALED;
				break;
			}
			animator.SetTrigger(Animator.StringToHash("Draining"));
			if(targetGenerator!=null) {
				attackEffect.SetActive(true);
				targetGenerator.state = Generator.generatorState.DANGER;
				float counter = 0;
				while(counter < 4) {
					counter+=Time.deltaTime;
					if(targetGenerator.state == Generator.generatorState.ACTIVATED) {
						Debug.Log("Hit while attacking!");
						state=ectoState.STUNNED;
						attackEffect.SetActive(false);
						break;
					}
					else if(seal.isActivated) {
						state = ectoState.SEALED;
						attackEffect.SetActive(false);
						break;
					}
					yield return null;
				}
			}
			else if(attackingSeal) {
				//break it!
				float counter = 0;
				seal.ableToCharge = false;
				attackEffect.SetActive(true);
				while(counter < 4) {
					counter+=Time.deltaTime;
					if(seal.isActivated) {
						state = ectoState.SEALED;
						attackEffect.SetActive(false);
						break;
					}
					else if(state==ectoState.STUNNED) {
						attackEffect.SetActive(false);
						animator.SetBool(Animator.StringToHash("Attack"),false);
						break;
					}
					yield return null;
				}
				if(state == ectoState.ATTACK) seal.breakSeal();
				else { 
					animator.SetBool(Animator.StringToHash("Attack"),false);
					break;
				}
				attackEffect.SetActive(false);
			}
			animator.SetBool(Animator.StringToHash("Attack"),false);
			//Debug.Log(targetGenerator);
			if(targetGenerator!=null&&targetGenerator.state == Generator.generatorState.DANGER) targetGenerator.state = Generator.generatorState.COOLDOWN;
			if(state == ectoState.ATTACK) state = ectoState.WANDER;
		}
	}

	public IEnumerator decideAttack() {
		yield return new WaitForSeconds(6);
		while(state==ectoState.SEALED) yield return null; 
		if(state!=ectoState.SEALED) state = ectoState.ATTACK;
	}

	public virtual IEnumerator WANDER() {
		animator.SetBool(Animator.StringToHash("Stunned"),false);
		StartCoroutine (decideAttack());
		agent.stoppingDistance = normalStoppingDistance;
		agent.baseOffset = normalBaseOffset;
		agent.speed = normalSpeed;
		agent.angularSpeed = normalRotateSpeed;
		//To be overwritten by child-class
		while (state==ectoState.WANDER) {
			Transform randomWaypoint = waypoints[Random.Range(0, waypoints.Length-1)];
			agent.SetDestination(randomWaypoint.position);
			while(agent.remainingDistance > agent.stoppingDistance) {
				if(state==ectoState.WANDER) yield return null;
				else if(seal.isActivated) {
					state = ectoState.SEALED;
					break;
				}
				else break;
			}
			yield return null;
		}
	}

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
	}
}
