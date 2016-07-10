using UnityEngine;
using System.Collections;

public class WadjetBoss : BossEnemy {
	public Transform debrisObject;
	public float debrisFallPos = 35f;
	private int numberDebris = 10;
	public Animator shadow;
	public Transform player; //REPLACE LATER! REPLACE. LATER!
	public Vector3 originalPosition;
	public ParticleSystem breath;
	public Transform scream;
	private bool wasFrozen;
	public Transform sonicWave;
	private bool facingPlayer = false;
	SceneTransition transition; 

	//More test vars
	public Transform[] wayPoints;
	private int currentPoint = 0;

	void Update() {
		playerPos = player.transform.position;
		if (!frozen) {
			manageAttackStates();
		}
		else {
			wasFrozen = true;
		}
		manageSpeed();
	}

	void Start() {
		originalPosition = transform.position;
		transition = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>();
	}

	void manageSpeed() {
		if(facingPlayer) RotateTowards(player);

		if(currentAnim(Animator.StringToHash("Base Layer.Slither"))) {
			if(!facingPlayer) agent.updateRotation = true;
			agent.speed = speed;
		}
		else {
			agent.updateRotation = false;
			agent.speed = 0;
		}
	}
	
	public override void InitialPattern() {
		if(currentAnim(Animator.StringToHash("Base Layer.Idle"))) canBeSealed = true;
		//breath attack
		animator.SetBool (Animator.StringToHash ("Breath"), true);
		if(wasFrozen && !frozen) {
			wasFrozen = false;
			//animator.SetTrigger(Animator.StringToHash ("Recoil"));
			DesparatePattern();
		}
		else if(currentAnim(Animator.StringToHash("Base Layer.Pause"))) {
			DesparatePattern();
		}
	}

	public void playBreath() {
		breath.Play ();
	}

	public void playScream() {
		StartCoroutine (startScream());
	}

	public void toggleFog() {
		shadow.SetBool (Animator.StringToHash("Dark"), !shadow.GetBool(Animator.StringToHash("Dark")));
	}

	public override void DesparatePattern() {
		hitCounter = 0;
		animator.SetBool (Animator.StringToHash ("Breath"), false);
		animator.ResetTrigger(Animator.StringToHash ("Recoil"));
		if(currentAnim(Animator.StringToHash("Base Layer.Scream"))) currentAttackPattern = attackPattern.INITIAL;
		else if(currentAttackPattern != attackPattern.DESPARATE) {
			currentAttackPattern = attackPattern.DESPARATE;
			StartCoroutine(LoopAround());
		}
	}

	protected IEnumerator LoopAround()
	{	
		canBeSealed = false;
		agent.updateRotation = true;
		//Loop around once
		foreach (Transform w in wayPoints) {
			agent.SetDestination(w.transform.position);
			while(agent.remainingDistance > agent.stoppingDistance) {
				yield return null;
				animator.SetBool(Animator.StringToHash("Moving"), true);
			}
			yield return null;
		}
		//Goto Player
		Transform minPoint = wayPoints[0];
		foreach (Transform w in wayPoints) {
			float compareDist = Vector3.Distance(w.transform.position, player.transform.position);
			float curDist = Vector3.Distance(minPoint.transform.position, player.transform.position);
			if(compareDist < curDist) minPoint = w;
		}

		//Loop to closets playerPoint
		foreach (Transform w in wayPoints) {
			if(w == minPoint) {
				//agent.updateRotation = false;
				//facingPlayer = true;
			}

			agent.SetDestination(w.transform.position);
			while(agent.remainingDistance > agent.stoppingDistance) {
				animator.SetBool(Animator.StringToHash("Moving"), true);
				yield return null;
			}
			yield return null;
			if(w==minPoint) break;
		}

		agent.updateRotation = false;
		facingPlayer = true;
		while(agent.velocity.x != 0 || agent.velocity.z != 0) yield return null;
		animator.SetBool(Animator.StringToHash("Moving"), false);
		yield return new WaitForSeconds(0.7f);
		agent.updateRotation = true;
		facingPlayer = false;
	}

	public void DarkenRoom() {
		//To be called as animation event 
	}

	public void spawnDebris() {
		StartCoroutine (Debris());
	}

	public IEnumerator Debris() {
		Vector3 p = player.transform.position;
		p.y = debrisFallPos;
		Instantiate(debrisObject, p, Quaternion.identity);
		for(int i=0; i<numberDebris; i++){
			float x = Random.Range(-20,20);
			float z = Random.Range(-20,20);
			float m = Mathf.PerlinNoise(x,z);
			Vector3 pos = p + new Vector3(x*m, debrisFallPos, z*m);
			pos.y = debrisFallPos;
			Instantiate(debrisObject, pos, Quaternion.identity);
			yield return new WaitForSeconds(0.5f);
		}
		yield return new WaitForSeconds(3f);
		animator.SetBool(Animator.StringToHash("Dissapear"), false);
	}

	public IEnumerator startScream() {
		facingPlayer = false;
		for(int i=0; i<5; i++) {
			Instantiate(sonicWave, scream.transform.position, scream.transform.rotation);
			yield return new WaitForSeconds(0.2f);
		}
	}

	private void RotateTowards(Transform target) {
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
	}
}
