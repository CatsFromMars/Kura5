using UnityEngine;
using System.Collections;

public class BossEnemy : EnemyClass {
	public float speed = 30;
	private bool sealCooldown = false;
	private int sealWaitTime = 10;
	protected float distanceFromPlayer;
	protected bool canBeSealed = false;
	//Initial = Opening attack pattern
	//Weakness = Attack pattern that contains weak spot vulnerability
	//Desparate = Attack pattern that deploys if boss is hurt
	public enum attackPattern {INITIAL, WEAKNESS, DESPARATE, DEATH}
	public attackPattern state;
	private IEnumerator currentBehavior;
	public GameObject cutsceneObject;
	protected Transform swapper;

	void Start() {
		StartCoroutine(bossLoop());
		swapper = GameObject.FindGameObjectWithTag("PlayerSwapper").transform;
	}

	IEnumerator bossLoop() {
		//Have the boss keep acting until LIFE hits 0
		while(currentLife > 0) {
			yield return StartCoroutine(state.ToString());
		}
	}

	protected void changeState(attackPattern a) {
		StopCoroutine(currentBehavior);
		state = a;
	}

	public override void Die() {
		base.Die();
		state = attackPattern.DEATH;
		animator.updateMode = AnimatorUpdateMode.UnscaledTime;
		cutsceneObject.SetActive (true);
	}

	public virtual IEnumerator INITIAL() {
		//To be overwritten by child-class
		while (state==attackPattern.INITIAL) {
			Debug.Log("Initial....!");
			yield return null;
		}
	}

	public virtual IEnumerator WEAKNESS() {
		//To be overwritten by child-class
		yield return null;
	}

	public virtual IEnumerator DESPARATE() {
		//To be overwritten by child-class
		yield return null;
	}

}
