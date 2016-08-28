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
	public enum attackPattern {INITIAL, WEAKNESS, DESPARATE}
	public attackPattern state;
	private IEnumerator currentBehavior;

	void Start() {
		StartCoroutine(bossLoop());
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

	public virtual void ShadowSeal() {
		Debug.Log ("I got shadow sealed!");
	}

}
