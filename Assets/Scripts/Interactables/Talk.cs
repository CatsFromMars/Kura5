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
	public TextAsset text;
	private bool isTalking = false;

	void Awake() {
		Controller = GameObject.FindGameObjectWithTag("GameController");
		Data = Controller.GetComponent<GameData>();
		dialogueSpeech = text.text.Split('\n');
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
		bool push = Input.GetButtonDown("Charge") || Input.GetButtonDown("Confirm");
		if(!isTalking && npcObject && push && inRange) {
			StartCoroutine(Speak());
		}
	}

	IEnumerator Speak() {
		Time.timeScale = 0; //Pause
		isTalking = true;
		dialogue = Controller.GetComponent<Dialogue>();
		for(int i = 0; i < dialogueSpeech.Length; i++) {
			string speech = dialogueSpeech[i];
			int j = speech.IndexOf(":");
			if(j != -1) speech = speech.Insert(j+2, "\n");
			dialogue.Show(speech, null, null);
			while(!dialogue.isFinished) yield return null;
		}
		isTalking = false;
		Time.timeScale = 1;
	}

	
}
