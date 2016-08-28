using UnityEngine;
using System.Collections;

public class DarkLoans : MenuClass {
	public Animator doomy;
	public AudioClip doomyThankYou;
	public AudioClip doomyWelcome;
	public AudioClip darkLoansTheme;
	private MusicManager music;
	public TextAsset text1;
	public TextAsset notAvailable;
	public TextAsset thankyou;
	public TextAsset revival;
	public TextAsset reset;
	private bool exiting = false;

	void Start() {
		music = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>();
		music.changeMusic(darkLoansTheme);
		transform.parent = Camera.main.transform;
		transform.localPosition = Vector3.zero;
		DisplayDialogue.finishDialogue(true);
		StartCoroutine(DisplayDialogue.Speak(text1, true, false));
		Time.timeScale = 0;

	}

	public override void ChooseOption() {
		if(!exiting) {
			if (index == 0) {
				//RevivePartner
				StartCoroutine(revivePartner());
			}
			else if(index == 1) {
				//RestartRoom
				StartCoroutine(resetRoom());
			}
			else if(index == 2) {
				//DarkLoans
				DisplayDialogue.finishDialogue(true);
				StartCoroutine(DisplayDialogue.Speak(notAvailable, true, false));
			}
		}
	}

	public override void ExitMenu() {
		if(!exiting) {
			StopAllCoroutines();
			exiting = true;
			StartCoroutine (exit());
		}
	}

	IEnumerator resetRoom() {
		exiting = true;
		DisplayDialogue.finishDialogue(true);
		StartCoroutine(DisplayDialogue.Speak(reset, true, false));
		GameObject.FindGameObjectWithTag ("Player").transform.position = data.lastCheckpoint;
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(1.5f));
		StartCoroutine(exit());
	}

	IEnumerator revivePartner() {
		GameOverHandler go = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameOverHandler>();
		if(data.annieCurrentLife <= 0) {
			exiting = true;
			data.annieCurrentLife = data.annieMaxLife/2;
			go.annie.revive();
			DisplayDialogue.finishDialogue(true);
			StartCoroutine(DisplayDialogue.Speak(revival, true, false));
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(2f));
			StartCoroutine(exit());
		}
		else if(data.emilCurrentLife <= 0) {
			exiting = true;
			data.emilCurrentLife = data.emilMaxLife/2;
			go.emil.revive();
			DisplayDialogue.finishDialogue(true);
			StartCoroutine(DisplayDialogue.Speak(revival, true, false));
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(2f));
			StartCoroutine(exit());
		}
		else {
			DisplayDialogue.finishDialogue(true);
			StartCoroutine(DisplayDialogue.Speak(notAvailable, true));
		}
	}

	IEnumerator exit() {
		Debug.Log ("Goodbye!");
		exiting = true;
		transform.parent = null;
		doomy.SetTrigger (Animator.StringToHash("Goodbye"));
		DisplayDialogue.finishDialogue(true);
		StartCoroutine(DisplayDialogue.Speak(thankyou, true, false));
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(2.0f));
		DisplayDialogue.finishDialogue();
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(0.3f));
		scene.gotoScene (data.sceneName);
		music.changeMusic (music.previousMusic);
		Destroy (this.gameObject);
	}
}
