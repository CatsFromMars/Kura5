using UnityEngine;
using System.Collections;

public class TailSegment : BossSegment {
	public bool isTailEnd = false;
	private Animator animator;
	private GameData data;
	private int hitCounter = 0;
	private int hitThresh = 3;
	WadjetBoss wb;
	private NavMeshAgent agent;
	private GameObject player;
	private float distanceFromPlayer = 0f;
	private float range = 8f;
	private int attackTimer = 19;
	private int attackWaitTime = 20;
	private bool shadowsealed = false;

	//Shadow Seal Bits
	public ParticleSystem[] fog; //All fog bits close to this tail segment
	public GameObject shadowSealEffect;
	public GameObject shadowSealBreakEffect;

	void OnEnable() {
		if(shadowSealBreakEffect!=null) shadowSealBreakEffect.SetActive (false);
		shadowsealed = false;
	}
	// Use this for initialization
	void Awake () {
		data = GetUtil.getData();
		animator = GetComponent<Animator>();
		wb = bossParent.transform.GetComponent<WadjetBoss>();
		if(isTailEnd) {
			agent = GetComponent<NavMeshAgent>();
			player = GameObject.FindGameObjectWithTag("PlayerSwapper");
		}
	}

	void Update() {
		if(hitCounter>=hitThresh&&!isTailEnd) {
			hitCounter=0;
			wb.showTipIfAllDestroyed();
			animator.SetTrigger(Animator.StringToHash("Exit"));
		}
		if(isTailEnd) seekPlayer();
	}

	public override void hitWithBullet (Bullet bullet)
	{
		base.hitWithBullet (bullet);
		hitCounter++;
	}

	public override void hitWithSword()
	{
		base.hitWithSword();
		hitCounter++;
		if(isTailEnd) Slice();
	}

	public override void shadowSeal(SafeInt darkness)
	{
		Debug.Log ("Being called here right?");
		base.shadowSeal(darkness);
		if(!shadowsealed && !isTailEnd) StartCoroutine(startSeal(darkness.GetValue()));

	}

	IEnumerator startSeal(int seconds) {
		wb.shocked();
		shadowsealed = true;
		shadowSealBreakEffect.SetActive (false);
		shadowSealEffect.SetActive (true);
		animator.enabled = false;
		yield return new WaitForSeconds(seconds);
		shadowSealBreakEffect.SetActive (true);
		shadowSealEffect.SetActive (false);
		animator.enabled = true;
		yield return new WaitForSeconds(3);
		shadowsealed = false;
		animator.SetTrigger(Animator.StringToHash("Exit"));
		//wb.showTip();
	}
	
	void seekPlayer() {
		//agent.SetDestination(player.transform.position);
		GetPlayerDistance ();
		DecideAttack ();
	}

	void DecideAttack() {
		if(distanceFromPlayer <= range&&Time.timeScale!=0) {
			animator.SetBool(Animator.StringToHash("Attack"), false);
			if(attackTimer >= attackWaitTime) {
				animator.SetBool(Animator.StringToHash("Attack"), true);
				attackTimer = 0;
			}
			else attackTimer++;
		}
		else {
			animator.SetBool(Animator.StringToHash("Attack"), false);
			attackTimer = 0;
		}
	}

	void GetPlayerDistance() {
		if (player != null) {
			distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
		}
	}

	void Slice() {
		//slice it off, send signal to big boss
		animator.SetTrigger(Animator.StringToHash("Slice"));
		wb.tailSliced();
	}

}
