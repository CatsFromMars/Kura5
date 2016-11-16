using UnityEngine;
using System.Collections;

public class WadjetBoss : BossEnemy {
	public Transform debrisObject;
	public float debrisFallPos = 35f;
	private int numberDebris = 10;
	private float debrisWaitTime = 2;
	public Animator shadow;
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
	//environment vars
	public ParticleSystem fog;
	public GameObject skylights;
	public GameObject projectile;
	public Transform projectileSpawner;
	public Transform[] tails;
	public Transform tailTip;
	public ParticleSystem[] tailparticles;
	public GameObject fogForm;
	public ParticleSystem selfFog;
	private int screamThresh = 6;
	public CapsuleCollider c;
	public Animator tipAnimator;
	public GameObject humanForm;
	public GameObject armature;
	public Transform[] snakeSpawners;
	public Transform babySnake;


	void OnEnable() {
		//StartCoroutine(Decide());
		foreach(Transform t in tails) {
			t.gameObject.SetActive(false);
		}
		StartCoroutine (spawnTails());
	}

	public override void Die() {
		base.Die();
		clearSnakes();
		breath.gameObject.SetActive (false);
		tipAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
		foreach(ParticleSystem p in tailparticles) {
			p.Stop();
		}
		foreach(Transform t in tails) {
			t.GetComponent<Animator>().SetTrigger(Animator.StringToHash("Exit"));
		}
		shadow.SetTrigger (Animator.StringToHash ("Disable"));
		fog.Stop();
		skylights.SetActive (true);
		tipAnimator.SetTrigger(Animator.StringToHash("Exit"));
		humanForm.SetActive (true);
	}

	IEnumerator spawnTails() {
		foreach(ParticleSystem p in tailparticles) {
			p.Play();
		}
		yield return new WaitForSeconds (0.9f);
		int index = 0;
		foreach(Transform t in tails) {
			determineTail(index,t);
			index++;
			yield return new WaitForSeconds(0.9f);
		}
	}

	void determineTail(int index, Transform tail) {
		float percent = ((currentLife / maxLife)*10)/2;
		if(index < percent) tail.GetComponent<Animator>().gameObject.SetActive(true);
	}

	IEnumerator despawnTails() {
		foreach(Transform t in tails) {
			t.GetComponent<Animator>().SetTrigger(Animator.StringToHash("Exit"));
			yield return new WaitForSeconds(0.9f);
		}

		foreach(ParticleSystem p in tailparticles) {
			p.Stop();
		}
	}

	bool tailSegmentsSlain(Transform self=null) {
		foreach(Transform t in tails) {
			if(t.gameObject.active && t!=self) return false;
		}
		return true;
	}

	public override IEnumerator INITIAL()
	{
		while (state==attackPattern.INITIAL) {
			c.enabled = true;
			while (!currentAnim(Animator.StringToHash("Base Layer.Idle"))) {
				if(state!=attackPattern.INITIAL) break;
				else yield return null;
			}
			transform.tag = "Enemy";
			armature.gameObject.SetActive(true);
			if(state!=attackPattern.INITIAL) break;
			if(hitCounter > screamThresh) {
				hitCounter = 0;
				if(state!=attackPattern.INITIAL) break;
				animator.SetBool(Animator.StringToHash("Scream"), true&&!dying);
				StartCoroutine(despawnTails());
				state = attackPattern.WEAKNESS;
			}
			if(tailSegmentsSlain())showTip();
			yield return null;
		}
	}

	public void showTip() {
		//reveal the tip of the tail
		tailTip.gameObject.SetActive(true);
	}

	public void showTipIfAllDestroyed(Transform self) {
		if(tailSegmentsSlain(self)&&!tailTip.gameObject.active) {
			tailTip.gameObject.SetActive(true);
		}
	}

	public override IEnumerator WEAKNESS() {
		while (state==attackPattern.WEAKNESS) {
			breath.Stop();
			c.enabled = false;
			transform.tag = "Untagged";
			tipAnimator.SetTrigger(Animator.StringToHash("Exit"));
			animator.SetBool(Animator.StringToHash("Hidden"), true);
			while (!currentAnim(Animator.StringToHash("Base Layer.Hide"))) yield return null;
			StartCoroutine(spawnSnakes());
			selfFog.Stop();
			yield return new WaitForSeconds(1f);
			fogForm.SetActive(true);
			yield return new WaitForSeconds(2f);
			armature.gameObject.SetActive(false);
			animator.ResetTrigger (Animator.StringToHash ("Tail"));
			animator.ResetTrigger(Animator.StringToHash ("Shocked"));
			while(fogForm.active == true) yield return null;
			selfFog.Play();
			foreach(ParticleSystem p in tailparticles) {
				p.Play();
			}
			yield return new WaitForSeconds(1f);
			StartCoroutine(spawnSnakes());
			yield return new WaitForSeconds(1f);
			animator.SetBool(Animator.StringToHash("Hidden"), false);
			while (!currentAnim(Animator.StringToHash("Base Layer.Breath"))) yield return null;
			StartCoroutine(spawnTails());
			animator.SetBool(Animator.StringToHash("Scream"), false);
			state = attackPattern.INITIAL;
			armature.gameObject.SetActive(true);
		}
	}

	public void tailSliced() {
		//play this when the tail gets cut off
		Debug.Log ("OUCH!");
		if(currentLife>0) {
			if(!dying) {
				animator.SetTrigger (Animator.StringToHash ("Tail"));

			}
			StartCoroutine(despawnTails());
			state = attackPattern.WEAKNESS;
		}
	}

	public void shocked() {
		//happens if tail gets shadow sealed
		float percent = ((currentLife / maxLife)*10)/2;
		if(percent > 3) {
			animator.SetTrigger(Animator.StringToHash ("Shocked"));
			showTip();
		}
	}

	public void playBreath() {
		shadow.gameObject.SetActive(true);
		fog.Play();
		skylights.SetActive (false);
	}

	IEnumerator spawnSnakes() {
		if(GameObject.FindGameObjectsWithTag("Enemy").Length<10) {
			for(int i=0;i<3;i++) {
				Instantiate(babySnake,snakeSpawners[i].position, snakeSpawners[i].rotation);
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	public void playScream() {
		shadow.SetTrigger (Animator.StringToHash ("Disable"));
		fog.Stop();
		skylights.SetActive (true);
		animator.SetBool(Animator.StringToHash("Dark"), false);
	}

	IEnumerator spawnDebris() {
		//Adjust later, spawn over the player's head for now
		//(Reference Muspell? Carmilla?)
		yield return new WaitForSeconds (debrisWaitTime);
		Vector3 spawnPos = playerPos;
		spawnPos.y = debrisFallPos;
		Instantiate (debrisObject, spawnPos, Random.rotation);
	}
	
	public override void shadowSeal() {
		//Debug.Log("Stiff as a statue!");
		shadowStunBreakEffect.gameObject.SetActive (false);
		shadowStunBreakEffect.gameObject.SetActive (true);
	}

	public void ShootProjectile() {
		//ADD AS ANIMATION EVENT
		Vector3 projectileSpawnPoint = projectileSpawner.transform.position;
		Instantiate(projectile, projectileSpawnPoint, projectileSpawner.transform.rotation);
	}
	
	public void DarkenRoom() {
		//To be called as animation event 
	}

	void clearSnakes() {
		GameObject[] snakes = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach(GameObject s in snakes) {
			if(s.name.Contains("Snake")) Destroy(s.gameObject);
		}
	}

	private void RotateTowards(Transform target) {
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
	}
}
