using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	private GameObject playerPos;
	private NavMeshAgent agent;
	public enum followAs {SNAPTO, COMPANION}
	public followAs followType = followAs.SNAPTO;
	private Animator animator;

	void Awake() {
		playerPos = GameObject.FindGameObjectWithTag("PlayerSwapper");
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update () {
		if(followType==followAs.SNAPTO) transform.position = playerPos.transform.position;
		else if(followType==followAs.COMPANION && Time.timeScale!=0) {
			if(agent!=null) {
				if(agent.remainingDistance <= agent.stoppingDistance) agent.SetDestination(playerPos.transform.position);
				if(agent.velocity.sqrMagnitude < 4f) animator.SetBool(Animator.StringToHash("Idle"), true);
				else animator.SetBool(Animator.StringToHash("Idle"), false);
			}
		}
	}
}
