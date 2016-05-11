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
	protected attackPattern currentAttackPattern;

	protected void manageAttackStates() {
		switch (currentAttackPattern)
		{
		case attackPattern.INITIAL:
			InitialPattern();
			break;

		case attackPattern.WEAKNESS:
			WeaknessPattern();
			break;

		case attackPattern.DESPARATE:
			DesparatePattern();
			break;
		}
	}

	public virtual void InitialPattern() {
		//to be overwritten by child class
	}

	public virtual void WeaknessPattern() {
		//to be overwritten by child class
	}

	public virtual void DesparatePattern() {
		//to be overwritten by child class
	}

	void GetPlayerDistance () {
		if (player != null) {
			distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
		}
		else {
			player = GameObject.FindWithTag("Player").transform;
			distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
		}
	}

	public void ShadowSeal() {
		if(!frozen && !sealCooldown) StartCoroutine(startShadowSeal());
	}

	IEnumerator startShadowSeal() {
		if(canBeSealed && !sealCooldown) {
			Debug.Log ("SHADOW SEAL GOOOOO");
			sealCooldown = true;
			animator.SetTrigger(Animator.StringToHash("ShadowSeal"));
			animator.SetBool(Animator.StringToHash("Frozen"), true);
			shadowStunEffect.gameObject.SetActive(true);
			frozen = true;
			//animator.enabled = false;
			yield return new WaitForSeconds(lightLevels.darkness.GetValue());
			//animator.enabled = true;
			frozen = false;
			animator.SetBool(Animator.StringToHash("Frozen"), false);
			shadowStunEffect.gameObject.SetActive(false);
			shadowStunBreakEffect.GetComponent<ParticleSystem>().Play();
			yield return new WaitForSeconds (sealWaitTime);
			sealCooldown = false;
		}
	}
}
