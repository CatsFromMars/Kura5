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
		StartCoroutine(DisplayDialogue.Speak(text1, true));
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
				StartCoroutine(DisplayDialogue.Speak(notAvailable, true));
			}
		}
	}

	public override void ExitMenu() {
		if(!exiting) StartCoroutine (exit());
	}

	IEnumerator resetRoom() {
		exiting = true;
		StartCoroutine(DisplayDialogue.Speak(reset, true));
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
			StartCoroutine(DisplayDialogue.Speak(revival, true));
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(2f));
			StartCoroutine(exit());
		}
		else if(data.emilCurrentLife <= 0) {
			exiting = true;
			data.emilCurrentLife = data.emilMaxLife/2;
			go.emil.revive();
			StartCoroutine(DisplayDialogue.Speak(revival, true));
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(2f));
			StartCoroutine(exit());
		}
		else {
			StartCoroutine(DisplayDialogue.Speak(notAvailable, true));
		}
	}

	IEnumerator exit() {
		exiting = true;
		transform.parent = null;
		doomy.SetTrigger (Animator.StringToHash("Goodbye"));
		StartCoroutine(DisplayDialogue.Speak(thankyou, true));
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(2.3f));
		DisplayDialogue.finishDialogue();
		scene.gotoScene (data.sceneName);
		music.changeMusic (music.previousMusic);
		Destroy (this.gameObject);
	}
}
