using UnityEngine;
using System.Collections;

public class Talk : MonoBehaviour {
	public bool autoSpeak = false; //Do this for intro cutscenes and stuff.
	public bool destroyGameobject = false;
	public bool npcObject = false; //Talk on button press, as opposed to automatically
	bool inRange = false; //In range to talk?
	private bool isTalking = false;
	public TextAsset text;
	public TextAsset[] textLoops;
	private int textindex = 0;
	private GameData data;

	void Awake() {
		if(autoSpeak) Speak();
		data = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameData> ();
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			inRange = true;
			data.nearInteractable = true;
			if(!autoSpeak && !npcObject) Speak();
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
		if(!isTalking && npcObject && push && inRange) {
			Speak();
		}
	}

	void Speak() {
		text = textLoops [textindex];
		StartCoroutine (SpeakCoroutine());
		textindex = (textindex+1)%textLoops.Length;
	}

	IEnumerator SpeakCoroutine() {
		isTalking = true;
		yield return StartCoroutine(DisplayDialogue.Speak(text));
		isTalking = false;
	}
	
}
