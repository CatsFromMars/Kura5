using UnityEngine;
using System.Collections;

public class Snake : EnemyClass {
	public float interval = 0.2f;
	private Transform playerContainer;

	void Start() {
		playerContainer = GetUtil.getPlayer().transform;
		StartCoroutine(combatLoop());
	}

	void Update() {
		bool canMove = false;
		if(animator!=null) canMove = animator.GetCurrentAnimatorStateInfo (0).nameHash == hash.walkState;
		if(!dead) {
			if(canMove) {
				agent.Resume();
				//agent.updateRotation = true;
			}
			else { 
				agent.velocity = Vector3.zero;
				agent.Stop();
				//agent.updateRotation = false;
			}
		}
	}

	IEnumerator combatLoop() {
		yield return new WaitForSeconds (0.2f);
		while(!dead) {
			if(playerContainer.gameObject.activeSelf == false) playerContainer = GetUtil.getPlayer().transform;;
			agent.SetDestination (playerContainer.position);
			playerPos = playerContainer.position;
			//quickLook();
			yield return new WaitForSeconds(interval);
		}
	}
}
