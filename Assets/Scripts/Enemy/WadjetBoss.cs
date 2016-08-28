using UnityEngine;
using System.Collections;

public class WadjetBoss : BossEnemy {
	public Transform debrisObject;
	public float debrisFallPos = 35f;
	private int numberDebris = 10;
	private float debrisWaitTime = 2;
	public Animator shadow;
	public Transform player; //REPLACE LATER! REPLACE. LATER!
	public Vector3 originalPosition;
	public ParticleSystem breath;
	public Transform scream;
	private bool wasFrozen;
	public Transform sonicWave;
	private bool facingPlayer = false;
	private bool isDark=false;
	private bool slithering=false;
	public Transform[] wayPoints;
	private int currentPoint = 0;
	public GameObject shadowSealVFX;
	public GameObject breakFreeVFX;
	//environment vars
	public ParticleSystem fog;
	public ParticleSystem darkFogRing;
	public GameObject skylights;
	public GameObject projectile;
	public Transform projectileSpawner;


	void OnEnable() {
		//StartCoroutine(Decide());
	}

	void Update() {
		manageSpeed();
	}

	public override IEnumerator INITIAL()
	{
		while (state==attackPattern.INITIAL) {
		
			Debug.Log("Back to snaking");
			animator.SetBool(Animator.StringToHash("Moving"), true);
			agent.updateRotation = true;
			if(state!=attackPattern.INITIAL) break;
			foreach (Transform w in wayPoints) {
				agent.SetDestination(w.transform.position);
				if(state!=attackPattern.INITIAL) break;
				while(agent.remainingDistance > agent.stoppingDistance) {
					yield return null;
					if(state!=attackPattern.INITIAL) break;
				}
				Debug.Log("Yeah! Made it!");
				if(state!=attackPattern.INITIAL) break;
				yield return null;
			}
			if(state!=attackPattern.INITIAL) break;
			//Goto Player
			Transform minPoint = wayPoints[0];
			foreach (Transform w in wayPoints) {
				float compareDist = Vector3.Distance(w.transform.position, player.transform.position);
				float curDist = Vector3.Distance(minPoint.transform.position, player.transform.position);
				if(compareDist < curDist) minPoint = w;
			}

			foreach (Transform q in wayPoints) {
				if(q==minPoint) agent.updateRotation = false;
				agent.SetDestination(q.transform.position);
				while(agent.remainingDistance > agent.stoppingDistance) {
					yield return null;
				}
				if(q==minPoint) {
					break;
				}
				else if(state!=attackPattern.INITIAL) break;
				yield return null;
			}

			agent.updateRotation = false;
			animator.SetBool(Animator.StringToHash("Moving"), false);
			if(state!=attackPattern.INITIAL) break;
			yield return new WaitForSeconds(3);
			animator.SetBool(Animator.StringToHash("Desparate"), false);
		}
	}

	public override IEnumerator WEAKNESS() {
		//La Lupe gets shadowsealed by emil
		while (state==attackPattern.WEAKNESS) {
			//Debug.Log("Still frozen");
			yield return new WaitForSeconds(3);
			//Debug.Log("Should be free!");
			shadowSealVFX.SetActive(false);
			animator.SetBool(Animator.StringToHash("Frozen"), false);
			breakFreeVFX.SetActive(true);
			yield return new WaitForSeconds(5);
			animator.SetBool(Animator.StringToHash("Desparate"), true);
			state=attackPattern.INITIAL;
		}
	}
	
	void manageSpeed() {
		//if(facingPlayer) RotateTowards(player);
		
		if(currentAnim(Animator.StringToHash("Base Layer.Slither")) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0)) {
			if(!facingPlayer) agent.updateRotation = true;
			agent.speed = speed;
		}
		else {
			agent.updateRotation = false;
			agent.speed = 0;
		}
	}

	public void playBreath() {
		breath.Play();
		shadow.gameObject.SetActive(true);
		fog.Play();
		darkFogRing.Play();
		skylights.SetActive (false);
		animator.SetBool(Animator.StringToHash("Dark"), true);
	}

	public void playScream() {
		shadow.SetTrigger (Animator.StringToHash ("Disable"));
		fog.Stop();
		darkFogRing.Stop();
		skylights.SetActive (true);
		animator.SetBool(Animator.StringToHash("Dark"), false);
	}

	public void tailSlap() {
		StartCoroutine(spawnDebris());
	}

	IEnumerator spawnDebris() {
		//Adjust later, spawn over the player's head for now
		//(Reference Muspell? Carmilla?)
		yield return new WaitForSeconds (debrisWaitTime);
		Vector3 spawnPos = playerPos;
		spawnPos.y = debrisFallPos;
		Instantiate (debrisObject, spawnPos, Random.rotation);
	}
	
	public override void ShadowSeal() {
		if(state != attackPattern.WEAKNESS) {
			Debug.Log("Stiff as a statue!");
			breakFreeVFX.SetActive(false);
			shadowSealVFX.SetActive(true);
			animator.SetTrigger(Animator.StringToHash("ShadowSeal"));
			animator.SetBool(Animator.StringToHash("Frozen"), true);
			state = attackPattern.WEAKNESS;
		}
	}

	public void ShootProjectile() {
		//ADD AS ANIMATION EVENT
		Vector3 projectileSpawnPoint = projectileSpawner.transform.position;
		Instantiate(projectile, projectileSpawnPoint, projectileSpawner.transform.rotation);
	}
	
	public void DarkenRoom() {
		//To be called as animation event 
	}

	private void RotateTowards(Transform target) {
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
	}
}
