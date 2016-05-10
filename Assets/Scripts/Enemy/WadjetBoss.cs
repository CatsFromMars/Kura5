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
	public ParticleSystem scream;
	private bool wasFrozen;

	//More test vars
	public Transform[] wayPoints;
	private int currentPoint = 0;

	void Update() {
		playerPos = player.transform.position;
		if (!frozen) {
			manageAttackStates();
		}
		else wasFrozen = true;
	}

	void Start() {
		originalPosition = transform.position;
	}
	
	public override void InitialPattern() {
		canBeSealed = true;
		//breath attack
		animator.SetBool (Animator.StringToHash ("Breath"), true);
		if(hitCounter > 3) {
			animator.SetTrigger(Animator.StringToHash ("Recoil"));
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
		scream.Play ();
	}

	public void toggleFog() {
		shadow.SetBool (Animator.StringToHash("Dark"), !shadow.GetBool(Animator.StringToHash("Dark")));
	}

	public override void DesparatePattern() {
		hitCounter = 0;
		animator.ResetTrigger(Animator.StringToHash ("Recoil"));
		if(currentAnim(Animator.StringToHash("Base Layer.Idle"))) currentAttackPattern = attackPattern.INITIAL;
		else if(currentAttackPattern != attackPattern.DESPARATE) {
			currentAttackPattern = attackPattern.DESPARATE;
			StartCoroutine(LoopAround());
		}

	}

	protected IEnumerator LoopAround()
	{	
		agent.updateRotation = true;
		yield return new WaitForSeconds(5f);
		//Loop around once
		foreach (Transform w in wayPoints) {
			agent.SetDestination(w.transform.position);
			while(agent.remainingDistance > agent.stoppingDistance) {
				yield return null;
				animator.SetBool(Animator.StringToHash("Moving"), true);
			}
			yield return null;
		}

		Debug.Log("GOTO");
		//Goto Player
		Transform minPoint = wayPoints[0];
		foreach (Transform w in wayPoints) {
			float compareDist = Vector3.Distance(w.transform.position, player.transform.position);
			float curDist = Vector3.Distance(minPoint.transform.position, player.transform.position);
			if(compareDist < curDist) minPoint = w;
		}

		//Loop to closets playerPoint
		foreach (Transform w in wayPoints) {
			agent.SetDestination(w.transform.position);
			while(agent.remainingDistance > agent.stoppingDistance) {
				animator.SetBool(Animator.StringToHash("Moving"), true);
				yield return null;
			}
			yield return null;
			if(w==minPoint) break;
		}

		while(agent.velocity.x != 0 || agent.velocity.z != 0) yield return null;
		animator.SetBool(Animator.StringToHash("Moving"), false);
		yield return new WaitForSeconds(0.7f);
		quickLook ();
		agent.updateRotation = true;
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
}
