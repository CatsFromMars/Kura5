using UnityEngine;
using System.Collections;

public class Talk : MonoBehaviour {
	public bool walkToSpeaker = false;
	public bool autoSpeak = false; //Do this for intro cutscenes and stuff.
	public bool destroyGameobject = false;
	public bool npcObject = false; //Talk on button press, as opposed to automatically
	bool inRange = false; //In range to talk?
	private bool isTalking = false;
	private TextAsset text;
	public TextAsset[] textLoops;
	private int textindex = 0;
	private GameData data;
	public bool otenkoAppear = false; //For hint pannels
	private PlayerContainer p;

	void Awake() {
		if(autoSpeak) Speak();
		data = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameData> ();
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			inRange = true;
			data.nearInteractable = true;
			if(!autoSpeak && !npcObject) Speak();
			p = other.GetComponent<PlayerContainer>();
		}

	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player") {
			inRange = false;
			data.nearInteractable = false;
		}
	}

	void Update() {
		bool push = Time.timeScale != 0 && (Input.GetButtonDown("Charge") || Input.GetButtonDown("Confirm"));
		bool b = false;
		if(p != null && inRange) b = p.currentAnim (p.hash.idleState) || p.currentAnim (p.hash.runningState);
		if(!isTalking && npcObject && push && inRange && b) {
			Speak();
		}
	}

	void Speak() {
		otenkoAppear = true;
		text = textLoops [textindex];
		StartCoroutine (SpeakCoroutine());
		textindex = (textindex+1)%textLoops.Length;
	}

	IEnumerator SpeakCoroutine() {
		if(walkToSpeaker) {
			PlayerContainer player;
			player = GameObject.FindWithTag ("Player").GetComponent<PlayerContainer> ();
			Vector3 pos = transform.position + this.transform.forward*4f;
			yield return StartCoroutine(player.characterWalkTo(pos, this.transform));
		}
		isTalking = true;
		yield return StartCoroutine(DisplayDialogue.Speak(text));
		isTalking = false;
		otenkoAppear = false;
	}
	
}
