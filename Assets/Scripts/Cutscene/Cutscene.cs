using UnityEngine;
using System.Collections;

public class Cutscene : MonoBehaviour {
	public TextAsset cutsceneDialogue;
	public bool showBothPlayers = false;
	public bool fadeout = true;
	private Flags flags;
	SceneTransition transition;
	Animator annieAnimator;
	Animator emilAnimator;

	void Awake() {
		GameObject c = GameObject.FindGameObjectWithTag ("GameController");
		flags = c.GetComponent<Flags>();
		transition = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>();
		flags.AddCutsceneFlag(cutsceneDialogue.name);
	}

	void endCutscene() {
		if(annieAnimator!=null) annieAnimator.Rebind();
		if(emilAnimator!=null) emilAnimator.Rebind();
		flags.SetCutscene(cutsceneDialogue.name);
	}

	IEnumerator playCutscene() {
		if(fadeout) {
			Time.timeScale = 0;
			yield return StartCoroutine (transition.cutsceneArrange(showBothPlayers));
		}
		yield return StartCoroutine(DisplayDialogue.Speak(cutsceneDialogue));
		if(fadeout)yield return StartCoroutine (transition.deArrange());
		endCutscene();
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player" && !flags.CheckCutsceneFlag(cutsceneDialogue.name)) StartCoroutine(playCutscene());
	}
}
