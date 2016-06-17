using UnityEngine;
using System.Collections;

public class MenuClass : MonoBehaviour {
	//Utilities
	public GameData data;
	public SceneTransition scene;
	//For one dimmensional menus like Dark Loans
	public Transform selector;
	public Transform[] optionButtons;
	public Vector3[] arrowPositions;
	private int pushCounter = 0;
	private int pushWaitTime = 15;
	public int index = 0;

	//Axis
	private float horiz = 0;
	private float vert = 0;

	//Sounds
	public AudioClip selectNoise;
	public AudioClip confirm;
	public AudioClip deny;

	// Use this for initialization
	void Awake() {
		data = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();
		scene = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>();
	}
	
	void Update () {
		horiz = Mathf.RoundToInt(Input.GetAxisRaw ("Horizontal"));
		vert = Mathf.RoundToInt(Input.GetAxisRaw ("Vertical"));
		Select();
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
		if(Input.GetButtonDown("Confirm")) { //Charge is basically the A button
			makeSound(confirm);
			ChooseOption();
		}
		else if (Input.GetButtonDown("Deny")) {
			makeSound(deny);
			ExitMenu();
		}
		else if (horiz > 0 || vert < 0) {
			Vector3 pos = selector.localPosition;
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				makeSound(selectNoise);
				pushCounter = 0;
				//Select upward
				index++;
				if(index > optionButtons.Length-1) index = 0;
			}
		}
		else if (horiz < 0 || vert > 0) {
			Vector3 pos = selector.localPosition;
			pushCounter++;
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
