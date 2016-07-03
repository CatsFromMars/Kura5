using UnityEngine;
using System.Collections;

public class Vaquero : EnemyClass {
	public bool appeared = false;
	public float meleeRange = 7f;
	private float decisionWaitTime = 1.5f;
	public Transform bulletSpawner;
	public Transform bullet;
	public float dashSpeed = 10f;
	public Transform[] wayPoints;
	public Transform wisp;
	public Transform wispSpawner;
	//Countdown
	public SpriteRenderer countdownSprite;
	public Sprite[] countdownSprites;
	private float countdownTime=1f;
	public AudioClip countdownSound;

	void markedAsAppeared(){
		appeared = true;
	}

	public void StartBossFight() {
		animator.updateMode = AnimatorUpdateMode.Normal;
		StartCoroutine (Move());
	}

	// Update is called once per frame
	void Update () {
		ManageMovement();
	}

	void ManageMovement() {
		if(currentAnim(Animator.StringToHash("Base Layer.Running"))) {
			agent.speed = dashSpeed;
			agent.updatePosition = true;
		}
		else agent.speed = 0;
	}

	void SpawnFlame() {
		Instantiate(wisp, wispSpawner.transform.position, Quaternion.identity);
	}

	IEnumerator Move() {
		animator.SetBool (Animator.StringToHash ("Moving"), true);
		foreach (Transform w in wayPoints) {
			animator.SetBool (Animator.StringToHash ("Moving"), true);
			agent.SetDestination(w.transform.position);
			while(agent.remainingDistance > agent.stoppingDistance) {
				yield return null;
			}
			animator.SetBool(Animator.StringToHash("Moving"), false);
			yield return new WaitForSeconds (decisionWaitTime);
		}
		animator.SetBool(Animator.StringToHash("Moving"), false);
		yield return new WaitForSeconds (decisionWaitTime);
		StartCoroutine(Attack());
	}

	IEnumerator Attack() {
		animator.ResetTrigger(Animator.StringToHash("Shoot"));
		animator.ResetTrigger (Animator.StringToHash("Fire"));
		UpdatePlayerPos ();
		float d = Vector3.Distance(player.transform.position, transform.position);

		animator.SetTrigger (Animator.StringToHash("Shoot"));
		countdownSprite.sprite = countdownSprites[0];
		makeSound (countdownSound);
		yield return new WaitForSeconds(countdownTime);
		countdownSprite.sprite = countdownSprites[1];
		makeSound (countdownSound);
		yield return new WaitForSeconds(countdownTime);
		countdownSprite.sprite = countdownSprites[2];
		makeSound (countdownSound);
		yield return new WaitForSeconds(countdownTime);
		countdownSprite.sprite = countdownSprites[3];
		animator.SetTrigger (Animator.StringToHash("Fire"));
		yield return new WaitForSeconds (decisionWaitTime);
		countdownSprite.sprite = null;
		StartCoroutine(Move());
	}

	void UpdatePlayerPos() {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerPos = player.position;
	}

	void Shoot() {
		//Animation event for Bullet firing
		UpdatePlayerPos ();
		quickLook();
		bulletSpawner.LookAt (playerPos);
		Instantiate(bullet, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
	}

	public void cutSceneDeath() {
		StartCoroutine (cutsceneDeathStart ());
	}

	public IEnumerator cutsceneDeathStart() {
		//animation event for DEATH
		CamLooker looker;
		looker = GameObject.FindGameObjectWithTag ("CamFollow").GetComponent<CamLooker> ();
		looker.zoomToTarget(this.transform);
		yield return new WaitForSeconds (2);
		UpdatePlayerPos ();
		looker.zoomToTarget(player);
		Destroy (this.gameObject);
	}
}
