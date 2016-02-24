using UnityEngine;
using System.Collections;

public class Bat : EnemyClass {
	public GameObject biteBox;
	public float interval = 3;
	private bool detected = false;
	public AudioSource voice;
	private Transform playerContainer;

	void Update() {
		bool canMove = animator.GetCurrentAnimatorStateInfo (0).nameHash == hash.walkState;

		if(!dead) {
			if(canMove) {
				agent.Resume();
				agent.updateRotation = true;
			}
			else { 
				agent.velocity = Vector3.zero;
				agent.Stop();
				agent.updateRotation = false;
			}
		}
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player" && !detected) {
			playerContainer = GameObject.FindGameObjectWithTag ("PlayerSwapper").transform;
			animator.SetTrigger(Animator.StringToHash("PlayerDetected"));
			detected = true;
			StartCoroutine(combatLoop());
			player = other.transform;
			voice.Play();
		}
	}

	IEnumerator combatLoop() {
		while(!dead) {
			if(player.gameObject.activeSelf == false) 
			playerPos = playerContainer.position;
			agent.SetDestination (playerContainer.position);
			playerPos = playerContainer.position;
			quickLook();
			yield return new WaitForSeconds(interval);
		}
	}
}
