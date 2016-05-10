using UnityEngine;
using System.Collections;

public class BossEnemy : EnemyClass {
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
		if(!frozen) StartCoroutine(startShadowSeal());
	}

	IEnumerator startShadowSeal() {
		if(canBeSealed) {
			Debug.Log ("SHADOW SEAL GOOOOO");
			animator.SetTrigger(Animator.StringToHash("ShadowSeal"));
			yield return new WaitForSeconds(0.3f);
			shadowStunEffect.gameObject.SetActive(true);
			frozen = true;
			animator.enabled = false;
			yield return new WaitForSeconds(lightLevels.darkness.GetValue());
			animator.enabled = true;
			frozen = false;
			shadowStunEffect.gameObject.SetActive(false);
		}
	}
}
