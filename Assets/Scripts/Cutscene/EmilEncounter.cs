using UnityEngine;
using System.Collections;

public class EmilEncounter : MonoBehaviour {
	private CapsuleCollider col;
	private SphereCollider trigger;
	private Animator animator;
	public TextAsset close;
	public TextAsset cutscene;
	public GameObject walkingOutCutsceneTrigger;
	private bool done=false;
	// Use this for initialization
	void Awake () {
		animator = GetComponent<Animator>();
		col = GetComponent<CapsuleCollider>();
		trigger = GetComponent<SphereCollider>();
		walkingOutCutsceneTrigger.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeScale != 0) {
			col.enabled = true;
			trigger.enabled = true;
		}
		else {
			col.enabled = false;
			trigger.enabled = false;
		}
	}

	IEnumerator startCutscene() {
		yield return new WaitForSeconds (1f);
		StartCoroutine(DisplayDialogue.Speak(cutscene));
	}

	void activateTrigger() {
		//animation event
		walkingOutCutsceneTrigger.SetActive(true);
	}

	void killSelf() {
		Destroy (this.gameObject);
	}

	void OnTriggerEnter(Collider other) {
		if(Time.timeScale > 0) {
			//if(other.tag == "Player" && !done) {
				//done=true;
				//StartCoroutine(DisplayDialogue.Speak(close));
			//}
			if(other.tag == "Bullet") {
				StartCoroutine(startCutscene());
				animator.SetTrigger(Animator.StringToHash("Parry"));
			}
		}
	}

}
