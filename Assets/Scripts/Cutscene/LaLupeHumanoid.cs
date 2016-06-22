using UnityEngine;
using System.Collections;

public class LaLupeHumanoid : MonoBehaviour {
	private Animator animator;
	private float meleeRange = 6f;
	private float distanceFromPlayer = 0;
	public Transform bossForm;


	// Use this for initialization
	void Awake () {
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider other) {
		if(Time.timeScale > 0) {
			if(other.tag == "Player") {
				//Smack player if they get too close
				animator.SetBool(Animator.StringToHash("Attacking"), true);
				animator.SetTrigger(Animator.StringToHash("Warp"));
			}
			if(other.tag == "Bullet") {
				Debug.Log("Beyatch Smacked");
				//back to snake!
				bossForm.gameObject.SetActive(true);
				Vector3 pos = transform.position;
				pos.y = bossForm.position.y;
				bossForm.position = pos;
				gameObject.SetActive(false);
			}
		}
	}

	void Attack() {
		Transform player = GameObject.FindGameObjectWithTag ("Player").transform;
		animator.updateMode = AnimatorUpdateMode.Normal;
		animator.ResetTrigger(Animator.StringToHash("Warp"));
		Vector3 pos = player.transform.position + player.transform.forward*-2;
		pos = new Vector3(pos.x, transform.position.y, pos.z);
		transform.position = pos;
		Vector3 lookPos = new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z);
		transform.LookAt (lookPos);
		animator.SetBool(Animator.StringToHash("Attacking"), false);
	}

	void Slash() {
		Transform player = GameObject.FindGameObjectWithTag ("Player").transform;
		distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
		
		if(distanceFromPlayer <= meleeRange) {
			player.GetComponent<PlayerContainer>().hitPlayer(5, "Cloud", 5*transform.forward);
		}
	}
}
