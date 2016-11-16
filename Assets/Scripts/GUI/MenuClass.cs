using UnityEngine;
using System.Collections;

public class MenuClass : MonoBehaviour {
	//Utilities
	public bool initialize = true;
	public GameData data;
	public SceneTransition scene;
	//For one dimmensional menus like Dark Loans
	public Transform selector;
	public Transform[] optionButtons;
	public Vector3[] arrowPositions;
	private float pushCounter = 0;
	private float pushWaitTime = 0.2f;
	public int index = 0;
	protected bool exiting = false;
	public bool canPressEnter;
	private bool canPress=false;
	private int confirmDelay = 5;
	private int confirmDelayCounter = 0;

	//Axis
	private float horiz = 0;
	private float vert = 0;

	//Sounds
	public AudioClip selectNoise;
	public AudioClip confirm;
	public AudioClip deny;

	// Use this for initialization
	void Awake() {
		if(initialize) init();
	}

	void init() {
		data = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();
		scene = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>();
	}
	
	void Update () {
		horiz = Mathf.RoundToInt(Input.GetAxisRaw ("Horizontal"));
		vert = Mathf.RoundToInt(Input.GetAxisRaw ("Vertical"));
		if(!exiting) Select();
	}

	public void MoveSelector() {
		//Debug.Log (optionButtons[index]);
		selector.localPosition = arrowPositions [index];
		//update covers
		foreach (Transform o in optionButtons) {
			Transform cover = o.FindChild ("Cover");
			if(cover!=null) {
				if(optionButtons[index] == o) cover.GetComponent<SpriteRenderer>().enabled = false;
				else cover.GetComponent<SpriteRenderer>().enabled = true;
			}
		}
	}

	public void Select() {
		//Confirm
		//Debug.Log (horiz);
		confirmDelayCounter++;
		if (confirmDelayCounter >= confirmDelay) {
			if(!Input.GetButton("Inventory")) {
				canPress = true;
			}
		}
		if(canPress&&(Input.GetButtonDown("Confirm")||(Input.GetButtonDown("Inventory")&&canPressEnter))) { //Charge is basically the A button
			makeSound(confirm);
			ChooseOption();
		}
		else if (Input.GetButtonDown("Deny")) {
			makeSound(deny);
			ExitMenu();
		}
		else if (horiz > 0 || vert < 0) {
			Vector3 pos = selector.localPosition;
			pushCounter+=Time.unscaledDeltaTime;;
			if(pushCounter>=pushWaitTime) {
				if(optionButtons.Length>1) makeSound(selectNoise);
				pushCounter = 0;
				//Select upward
				index++;
				if(index > optionButtons.Length-1) index = 0;
			}
		}
		else if (horiz < 0 || vert > 0) {
			Vector3 pos = selector.localPosition;
			pushCounter+=Time.unscaledDeltaTime;
			if(pushCounter>=pushWaitTime) {
				makeSound(selectNoise);
				pushCounter = 0;
				//Select downward
				index--;
				if(index < 0) index = optionButtons.Length-1;
			}
		}
		else pushCounter = pushWaitTime;
		MoveSelector();
	}
	
	public virtual void ChooseOption() {
		//Debug.Log ("You picked!");
	}

	public virtual void ExitMenu() {
		//Debug.Log ("Exiting Menu! (If you can...)");
	}

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
	}
}
