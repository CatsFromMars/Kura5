using UnityEngine;
using System.Collections;

public class LaLupeTutorial : MonoBehaviour {
	AnnieController player;
	private Animator animator;
	public TextAsset speech0; //Hello shoot me please!
	public TextAsset speech1; //Missed me speech
	public TextAsset speech2; //Cries and runs away
	public TextAsset speech3; //Alt Missed me
	public TextAsset speech4; //Oh no! Out of energy!
	public TextAsset alt; //player tried to be a smarty pants, play the alt cutscene
	private int warpIndex=0;
	public Transform[] warpPoints;
	private float meleeRange = 6f;
	private float distanceFromPlayer = 0;
	public ParticleSystem fog;
	public ParticleSystem smoke;
	private bool done = false;
	private bool cutscene2 = false;
	private bool playerOnEmpty = false;
	private Flags flags;
	public GameObject cutsceneExit;
	public GameObject normalExit;
	public CapsuleCollider col;
	public Transform pos2; //used for alt;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<AnnieController>();
		animator = GetComponent<Animator>();
		GameObject c = GameObject.FindGameObjectWithTag ("GameController");
		flags = c.GetComponent<Flags>();
		flags.AddCutsceneFlag(speech2.name);
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

	void OnTriggerStay(Collider other) {
		if(other.tag == "Player" && !done && Time.timeScale!=0) {
			//Smack player if they get too close
			animator.SetBool(Animator.StringToHash("Attacking"), true);
			animator.SetTrigger(Animator.StringToHash("Warp"));
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if(Time.timeScale > 0&&flags.CheckCutsceneFlag(speech0.name)) {
			bool isBeingTargeted = player.targeting;
			if(other.tag == "Bullet") {
				if(!isBeingTargeted && !done && Time.timeScale!=0) {
					//cutscene if player tries to shoot
					col.enabled = false;
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
				else if(isBeingTargeted) col.enabled = true;
			}
		}
	}

	IEnumerator missedMe(TextAsset txt) {
		while(!currentAnim(Animator.StringToHash("Base Layer.Idle"))) yield return null;
		destroyBullets ();
		animator.updateMode = AnimatorUpdateMode.UnscaledTime;
		StartCoroutine(DisplayDialogue.Speak(txt));
	}

	void destroyBullets() {
		GameObject[] b = GameObject.FindGameObjectsWithTag("Bullet");
		foreach(GameObject go in b) {
			Destroy(go);
		}
	}

	void OnCollisionEnter(Collision collision) {
		//Flags flags = GetUtil.getFlags();
		if(collision.gameObject.tag == "Bullet"&&!flags.CheckCutsceneFlag(speech0.name)) {
			done=true;
			flags.SetCutscene(speech0.name);
			StartCoroutine(playAltCutscene());
		}
		else if(Time.timeScale > 0 && !done) {
			bool isBeingTargeted = player.targeting;
			if (collision.gameObject.tag == "Bullet" && isBeingTargeted) {
				animator.SetBool(Animator.StringToHash("CutsceneMode"), true);
				done = true;
				StartCoroutine(startCutscene());
			}
		}
	}

	IEnumerator startCutscene() {
		animator.updateMode = AnimatorUpdateMode.UnscaledTime;
		animator.SetBool(Animator.StringToHash("Attacking"), false);
		animator.SetTrigger(Animator.StringToHash("Warp"));
		yield return new WaitForSeconds (0.7f);
		smoke.gameObject.SetActive(true);
		animator.updateMode = AnimatorUpdateMode.UnscaledTime;
		yield return StartCoroutine(DisplayDialogue.Speak(speech2));
		flags.SetCutscene(speech2.name);
		normalExit.SetActive (false);
		cutsceneExit.SetActive (true);
	}

	void KnockOverPlayer() {
		player.knockOver();
	}

	public bool currentAnim(int hash) {
		return animator.GetCurrentAnimatorStateInfo(0).nameHash == hash;
	}

	IEnumerator playAltCutscene() {
		yield return new WaitForSeconds (0.1f);
		Vector3 pos = player.transform.position + player.transform.forward*-4;
		pos = new Vector3(pos.x, transform.position.y, pos.z);
		pos2.position = pos;
		Vector3 rot = pos2.transform.rotation.eulerAngles;
		rot.y = player.transform.rotation.y;
		pos2.transform.rotation = Quaternion.Euler (rot);
		yield return StartCoroutine(DisplayDialogue.Speak(alt));
		flags.SetCutscene(speech2.name);
		normalExit.SetActive (false);
		cutsceneExit.SetActive (true);
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
