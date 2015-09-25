using UnityEngine;
using System.Collections;

public class Talk : MonoBehaviour {
	public Texture2D portrait;
	public string[] dialogueSpeech;
	//private int currentDialogue = 0;
	Dialogue dialogue;
	GameObject Controller;
	GameData Data;
	public bool autoSpeak = false; //Do this for intro cutscenes and stuff.
	public bool destroyGameobject = false;
	public bool npcObject = false; //Talk on button press, as opposed to automatically
	bool inRange = false; //In range to talk?

	void Awake() {
		Controller = GameObject.FindGameObjectWithTag("GameController");
		Data = Controller.GetComponent<GameData>();
		if(autoSpeak) StartCoroutine(Speak());
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			inRange = true;
			if(!autoSpeak && !npcObject) StartCoroutine(Speak());
		}

	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player") inRange = false;
	}

	void Update() {
		if(npcObject && Input.GetButtonDown("Confirm") && inRange) {
			if(!Data.isTalking)StartCoroutine(Speak());
		}
	}

	IEnumerator Speak() {
		if(Data != null) Data.isTalking = true;
		dialogue = Controller.GetComponent<Dialogue>();
		for(int i = 0; i < dialogueSpeech.Length; i++) {
			dialogue.Show(dialogueSpeech[i], null, null);
			while(!dialogue.isFinished) yield return null;
		}
		if(Data != null) Data.isTalking = false;
		if(destroyGameobject) Destroy (this.gameObject);
		//else Destroy(this);
	}

	
}
