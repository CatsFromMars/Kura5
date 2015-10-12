using UnityEngine;
using System.Collections;

public class UnburdenPuzzle : MonoBehaviour {

	public SwitchScript[] switches;
	private bool puzzleSolved;
	private Animator a;

	void Awake() {
		a = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		if(checkForPuzzleClear()) a.SetBool(Animator.StringToHash("Activated"), true);
	}

	bool checkForPuzzleClear() {
		bool solved = true;
		foreach (SwitchScript s in switches) {
			if(s.activated == true) solved = false;
		}
		return solved;
	}
}
