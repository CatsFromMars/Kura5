using UnityEngine;
using System.Collections;

public class Talk : MonoBehaviour {
	public bool walkToSpeaker = false;
	public bool lookatSpeaker = false;
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
	private Flags flags;
	public bool isNeroSavePoint = false;
	public bool annieEmilSplit = false;
	public TextAsset[] annieLoops;
	public TextAsset[] emilLoops;
	public bool customTagEnter=false;
	public string customTag = "Bullet";
	public bool setNPCFlag = false;

	void Start() {
		data = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameData>();
		flags = data.gameObject.GetComponent<Flags>();
		if(autoSpeak) {
			flags.AddCutsceneFlag(textLoops[0].name);
		}
		if(autoSpeak&&!flags.CheckCutsceneFlag(textLoops[0].name)) {
			Speak();
			flags.SetCutscene(textLoops[0].name);
		}
	}

	void OnTriggerEnter(Collider other) {
		bool custom = customTagEnter && other.tag == customTag;
		if((other.tag == "Player"&&!customTagEnter)||custom) {
			inRange = true;
			data.nearInteractable = true;
			if(!autoSpeak && !npcObject) Speak();
			p = other.GetComponent<PlayerContainer>();
		}

	}

	void OnTriggerExit(Collider other) {
		bool custom = customTagEnter && other.tag == customTag;
		if((other.tag == "Player"&&!customTagEnter)||custom) {
			inRange = false;
			data.nearInteractable = false;
		}
	}

	void Update() {
		bool push = (Input.GetButtonDown("Charge") || Input.GetButtonDown("Confirm"));
		bool b = false;
		if(p != null && inRange) b = !p.performingAction;
		if(Time.timeScale != 0 && !isTalking && npcObject && push && inRange && b) {
			Speak();
		}
	}

	void Speak() {
		isTalking = true;
		otenkoAppear = true;
		if(annieEmilSplit) {
			if(data.currentPlayer==GameData.player.Annie) text = annieLoops [textindex];
			else if(data.currentPlayer==GameData.player.Emil) text = emilLoops [textindex];
		}
		else text = textLoops [textindex];
		StartCoroutine (SpeakCoroutine());
		if(annieEmilSplit) {
			if(data.currentPlayer==GameData.player.Annie) textindex = (textindex+1)%annieLoops.Length;
			else if(data.currentPlayer==GameData.player.Emil) textindex = (textindex+1)%emilLoops.Length;
		}
		else textindex = (textindex+1)%textLoops.Length;
	}

	IEnumerator SpeakCoroutine() {
		if(autoSpeak) while(Time.timeScale!=1) yield return null;
		if(walkToSpeaker) {
			PlayerContainer player;
			player = GameObject.FindWithTag ("Player").GetComponent<PlayerContainer> ();
			Vector3 pos = transform.position + this.transform.forward*4f;
			yield return StartCoroutine(player.characterWalkTo(pos, this.transform));
		}
		else if(lookatSpeaker) {
			PlayerContainer player;
			player = GameObject.FindWithTag ("Player").GetComponent<PlayerContainer> ();
			Vector3 pos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
			player.transform.LookAt(pos);
		}
		if(isNeroSavePoint) SaveLoad.Save();
		yield return StartCoroutine(DisplayDialogue.Speak(text));
		isTalking = false;
		otenkoAppear = false;
		if(destroyGameobject) Destroy(this.gameObject);
		if(setNPCFlag) {
			flags.AddCutsceneFlag(textLoops[0].name);
			flags.SetCutscene(textLoops[0].name);
		}
	}
	
}
