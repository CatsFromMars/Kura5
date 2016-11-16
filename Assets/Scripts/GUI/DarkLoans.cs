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
	//private bool exiting = false;

	void Start() {
		music = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>();
		music.previousMusic = music.audio.clip;
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
				//StartCoroutine(resetRoom());
				DisplayDialogue.finishDialogue(true);
				StartCoroutine(DisplayDialogue.Speak(notAvailable, true, false));
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
		GameObject[] gos = GameObject.FindGameObjectsWithTag("EnemyWeapon");
		if(gos.Length==0) {
			exiting = true;
			DisplayDialogue.finishDialogue(true);
			yield return StartCoroutine(DisplayDialogue.Speak(reset));
			GameObject.FindGameObjectWithTag ("Player").transform.position = data.lastCheckpoint;
			StartCoroutine(exit());
		}
		else {
			DisplayDialogue.finishDialogue(true);
			StartCoroutine(DisplayDialogue.Speak(notAvailable, true));
		}
	}

	IEnumerator revivePartner() {
		GameOverHandler go = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameOverHandler>();
		if(data.annieCurrentLife <= 0) {
			exiting = true;
			data.annieCurrentLife = data.annieMaxLife/2;
			go.annie.revive();
			DisplayDialogue.finishDialogue(true);
			yield return StartCoroutine(DisplayDialogue.Speak(revival));
			StartCoroutine(exit());
		}
		else if(data.emilCurrentLife <= 0) {
			exiting = true;
			data.emilCurrentLife = data.emilMaxLife/2;
			go.emil.revive();
			DisplayDialogue.finishDialogue(true);
			yield return StartCoroutine(DisplayDialogue.Speak(revival));
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
		//transform.parent = null;
		doomy.SetTrigger (Animator.StringToHash("Goodbye"));
		DisplayDialogue.finishDialogue(true);
		StartCoroutine(DisplayDialogue.Speak(thankyou, true, false));
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(2.0f));
		Time.timeScale = 1;
		music.changeMusic (music.previousMusic);
		DisplayDialogue.finishDialogue();
		Dialogue dialogue = data.gameObject.GetComponent<Dialogue>();
		dialogue.Hide();
		Destroy (this.gameObject);
	}
}
