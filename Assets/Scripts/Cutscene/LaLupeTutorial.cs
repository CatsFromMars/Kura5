using UnityEngine;
using System.Collections;

public class LaLupeTutorial : MonoBehaviour {
	AnnieController player;
	private Animator animator;
	public TextAsset speech1; //Missed me speech
	public TextAsset speech2; //Cries and runs away
	public TextAsset speech3; //Alt Missed me
	public TextAsset speech4; //Oh no! Out of energy!
	private int warpIndex=0;
	public Transform[] warpPoints;
	private float meleeRange = 6f;
	private float distanceFromPlayer = 0;
	public ParticleSystem fog;
	public ParticleSystem smoke;
	private bool done = false;
	private bool cutscene2 = false;
	private bool playerOnEmpty = false;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<AnnieController>();
		animator = GetComponent<Animator>();
	}

	void Update() {
		if (player.getEnergy()<player.getEnergyCost() && !playerOnEmpty) {
			playerOnEmpty = true;

			//Sneak a solar fruit into the player's inventory
			GameObject global = GameObject.FindGameObjectWithTag ("GameController");
			Inventory inventory = global.GetComponent<Inventory>();
			if(inventory.checkForConsumable(3) == -1) inventory.AddConsumable(3);

			//Cutscene
			animator.SetBool(Animator.StringToHash("Attacking"), false);
			animator.SetTrigger(Animator.StringToHash("Warp"));
			animator.updateMode = AnimatorUpdateMode.UnscaledTime;
			StartCoroutine(DisplayDialogue.Speak(speech4));
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if(Time.timeScale > 0) {
			if(other.tag == "Player") {
				//Smack player if they get too close
				animator.SetBool(Animator.StringToHash("Attacking"), true);
				animator.SetTrigger(Animator.StringToHash("Warp"));
			}
			if(other.tag == "Bullet") {
				bool beingTargeted = player.targeting;
				if(!beingTargeted) {
					//cutscene if player tries to shoot
					animator.SetBool(Animator.StringToHash("Attacking"), false);
					animator.SetTrigger(Animator.StringToHash("Warp"));
					animator.updateMode = AnimatorUpdateMode.UnscaledTime;
					if(!cutscene2) {
						StartCoroutine(missedMe(speech1));
					}
					else {
						StartCoroutine(missedMe(speech3));
					}
					cutscene2=true;
				}
			}
		}
	}

	IEnumerator missedMe(TextAsset txt) {
		yield return new WaitForSeconds(0.6f);
		StartCoroutine(DisplayDialogue.Speak(txt));
	}

	void OnCollisionEnter(Collision collision) {
		if(Time.timeScale > 0) {
			if (collision.gameObject.tag == "Bullet" && !done) {
				animator.SetBool(Animator.StringToHash("CutsceneMode"), true);
				StartCoroutine(startCutscene());
			}
		}
	}

	IEnumerator startCutscene() {
		done = true;
		animator.updateMode = AnimatorUpdateMode.UnscaledTime;
		animator.SetBool(Animator.StringToHash("Attacking"), false);
		animator.SetTrigger(Animator.StringToHash("Warp"));
		yield return new WaitForSeconds (0.7f);
		smoke.gameObject.SetActive(true);
		StartCoroutine(DisplayDialogue.Speak(speech2));
	}

	void KnockOverPlayer() {
		player.knockOver();
	}

	void Attack() {
		animator.updateMode = AnimatorUpdateMode.Normal;
		animator.ResetTrigger(Animator.StringToHash("Warp"));
		Vector3 pos = player.transform.position + player.transform.forward*-2;
		pos = new Vector3(pos.x, transform.position.y, pos.z);
		transform.position = pos;
		Vector3 lookPos = new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z);
		transform.LookAt (lookPos);
		animator.SetBool(Animator.StringToHash("Attacking"), false);
	}

	void Warp() {
		//Teleport away from Annie
		animator.ResetTrigger(Animator.StringToHash("Warp"));
		warpIndex++;
		if(warpIndex > warpPoints.Length-1) warpIndex=0;
		transform.position = warpPoints[warpIndex].position;
		transform.rotation = warpPoints[warpIndex].rotation;
	}

	void Slash() {
		distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

		if(distanceFromPlayer <= meleeRange) {
			player.hitPlayer(3, "Cloud", 5*transform.forward);
		}
		
	}

	void faceCamera() {
		GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
		Vector3 pos = new Vector3(cam.transform.position.x,this.transform.position.y,cam.transform.position.z);
		transform.LookAt(pos);
	}

	public void killSelf() {
		fog.Stop();
	}
}
