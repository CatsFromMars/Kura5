using UnityEngine;
using System.Collections;

public class StealthCoffin : MonoBehaviour {
	public PlayerContainer player;
	public Transform model;
	private Animator animator;
	private bool initiated = false;
	private NavMeshAgent agent;

	void Awake() {
		animator = model.GetComponent<Animator>();
		player = GetActivePlayer.getActivePlayer().GetComponent<PlayerContainer>();
	}

	// Update is called once per frame
	void Update () {
		if(player.inCoffin) {
			if(!initiated) {
				animator.SetBool (Animator.StringToHash ("Appear"), true);
				model.gameObject.SetActive(true);
				transform.parent = null;
				initiated = true;
			}
			transform.position = player.transform.position;
			transform.rotation = player.transform.rotation;
			updateAnimations();
		}
		else {
			initiated = false;
			animator.SetBool (Animator.StringToHash ("Appear"), false);
		}
	}

	void updateAnimations() {
		animator.SetBool (Animator.StringToHash ("Moving"), player.isPlayerMoving());
	}
}
