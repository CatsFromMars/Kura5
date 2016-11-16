using UnityEngine;
using System.Collections;

public class OrderPuzzle : MonoBehaviour {
	public Torch[] solution;
	private Torch[] torches;
	public string flag;
	private GameObject global;
	private Flags flags;
	private bool puzzleSolved = false;
	public AudioClip correct;
	public AudioClip incorrect;

	void Awake() {
		torches = new Torch[solution.Length];
		global = GameObject.FindGameObjectWithTag ("GameController");
		flags = global.GetComponent<Flags>();
		flags.AddOtherFlag(flag);
		if (flags.CheckOtherFlag (flag)) {
			foreach (Torch t in solution) {
				t.light();
			}
		}
	}

	void Update() {
		if(!puzzleSolved) {
			checkForTorchLight();
			if(checkForFull()) checkForSolution();
		}
	}

	void finishPuzzle() {
		Debug.Log ("Puzzle Solved!!!");
		//play jingle
		if (!flags.CheckOtherFlag (flag)) makeSound (correct);
		//flags
		flags.SetOther(flag);
		puzzleSolved = true;
	}

	void addTorchToList(Torch t) {
		//Debug.Log ("Adding torch...");
		for(int i=0; i<torches.Length;i++) {
			if(torches[i]==t) break;
			if(torches[i]==null) {
				Debug.Log("added "+t.gameObject.name);
				torches[i]=t;
				break;
			}
		}
	}

	bool checkForTorch(Torch t) {
		for(int i=0; i<torches.Length;i++) {
			if(torches[i]==t) return true;
		}
		return false;
	}

	void checkForTorchLight() {
		foreach (Torch t in solution) {
			if(t.activated) addTorchToList(t);
		}
	}

	void checkForSolution() {
		//put t1-t3 in the order of the puzzle solution
		bool same = true;
		for(int i=0; i<torches.Length;i++) {
			if(torches[i]!=solution[i]) same=false;
		}
		if(same) finishPuzzle();
		else resetTorches();
	}

	bool checkForFull() {
		foreach (Torch t in torches) {
			if(t==null) return false;
		}
		return true;
	}

	void resetTorches() {
		Debug.Log ("Resetting Torches");
		makeSound (incorrect);
		foreach (Torch t in solution) {
			t.douse();
		}
		torches = new Torch[solution.Length];
	}

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		if(audio.enabled) {
			audio.clip = clip;
			audio.Play();
		}
	}
}
