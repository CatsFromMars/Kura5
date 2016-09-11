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

	void OnEnabled() {
		//StartCoroutine(Decide());
		foreach(Transform t in tails) {
			t.gameObject.SetActive(false);
		}
		StartCoroutine (spawnTails());
	}

	IEnumerator spawnTails() {
		foreach(ParticleSystem p in tailparticles) {
			p.Play();
		}
		yield return new WaitForSeconds (0.9f);
		foreach(Transform t in tails) {
			t.GetComponent<Animator>().gameObject.SetActive(true);
			yield return new WaitForSeconds(0.9f);
		}
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

	bool tailSegmentsSlain() {
		foreach(Transform t in tails) {
			if (t.gameObject.active) return false;
		}
		return true;
	}

	public override IEnumerator INITIAL()
	{
		while (state==attackPattern.INITIAL) {
			c.enabled = true;
			transform.tag = "Enemy";
			if(hitCounter > screamThresh) {
				hitCounter = 0;
				animator.SetBool(Animator.StringToHash("Scream"), true);
				StartCoroutine(despawnTails());
				state = attackPattern.WEAKNESS;
			}
			yield return null;
		}
	}

	public void showTip() {
		//reveal the tip of the tail
		tailTip.gameObject.SetActive(true);
	}

	public void showTipIfAllDestroyed() {
		if(tailSegmentsSlain()&&!tailTip.gameObject.activeSelf) {
			tailTip.gameObject.SetActive(true);
		}
	}

	public override IEnumerator WEAKNESS() {
		while (state==attackPattern.WEAKNESS) {
			c.enabled = false;
			transform.tag = "Untagged";
			animator.SetBool(Animator.StringToHash("Hidden"), true);
			while (!currentAnim(Animator.StringToHash("Base Layer.Hide"))) yield return null;
			selfFog.Stop();
			yield return new WaitForSeconds(1f);
			fogForm.SetActive(true);
			while(fogForm.active == true) yield return null;
			selfFog.Play();
			foreach(ParticleSystem p in tailparticles) {
				p.Play();
			}
			yield return new WaitForSeconds(2f);
			animator.SetBool(Animator.StringToHash("Hidden"), false);
			while (!currentAnim(Animator.StringToHash("Base Layer.Breath"))) yield return null;
			StartCoroutine(spawnTails());
			animator.SetBool(Animator.StringToHash("Scream"), false);
			state = attackPattern.INITIAL;
		}
	}

	public void tailSliced() {
		//play this when the tail gets cut off
		Debug.Log ("OUCH!");
		animator.SetTrigger (Animator.StringToHash ("Tail"));
		StartCoroutine(despawnTails());
		state = attackPattern.WEAKNESS;
	}

	public void shocked() {
		//happens if tail gets shadow sealed
		animator.SetTrigger(Animator.StringToHash ("Shocked"));
		showTip();
	}

	public void playBreath() {
		breath.Play();
		shadow.gameObject.SetActive(true);
		fog.Play();
		skylights.SetActive (false);
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

	private void RotateTowards(Transform target) {
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
	}
}
