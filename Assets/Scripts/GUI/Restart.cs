using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {
	//For Game Over related things
	public bool snapsToCamera = true;
	public AudioClip gameOverMusic;
	public Transform gameOverGraphic;
	public Transform continueGraphic;
	public Transform continueOptions;
	public Transform arrow;
	public enum prompt {YES, NO};
	public prompt choice = prompt.YES;
	private int pushCounter = 0;
	private int pushWaitTime = 15;
	private float horiz = 0;
	public AudioSource music;
	private GameObject global;
	private GameOverHandler go;
	public AudioClip[] gameOverQuotes;

	//Sounds
	public AudioClip selectNoise;
	public AudioClip confirm;


	void Awake() {
		if (snapsToCamera) {
			transform.parent = Camera.main.transform;
			transform.localPosition = Vector3.zero;
		}
		StartCoroutine (Options ());
		global = GameObject.FindGameObjectWithTag ("Global");
		go = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameOverHandler>();
	}
	
	// Update is called once per frame
	void Update () {
		horiz = Mathf.RoundToInt(Input.GetAxisRaw ("Horizontal"));

		if (continueOptions.gameObject.activeSelf) {
			Select();
		}
						
	}

	private void Select() {
		//Confirm
		Debug.Log (horiz);
		if(Input.GetButtonDown("Charge")) { //Charge is basically the A button
			transform.parent = null;
			makeSound(confirm);
			if(choice == prompt.YES) {
				//Continue Game
				//continueOptions.gameObject.SetActive(false);
				music.mute = true;
				Retry();
			}
			else {
				continueOptions.gameObject.SetActive(false);
				//Display Game Over
				StartCoroutine(QuitGame());
			}
		}
		else if (horiz != 0) {
			Vector3 pos = arrow.localPosition;
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				makeSound(selectNoise);
				pushCounter = 0;
				if(choice == prompt.NO) {
					choice = prompt.YES;
					pos.x = -8f;
				}
				else if(choice == prompt.YES) {
					choice = prompt.NO;
					pos.x = 2f;
				}
				arrow.localPosition = pos;
			}
		}
		else pushCounter = pushWaitTime;
	}

	IEnumerator Options() {
		Time.timeScale = 1;
		continueOptions.gameObject.SetActive (false);
		yield return new WaitForSeconds (3);
		continueOptions.gameObject.SetActive (true);

	}

	IEnumerator QuitGame() {
		music.mute = true;
		makeSound(gameOverQuotes[Random.Range(0, gameOverQuotes.Length)]);
		continueGraphic.gameObject.SetActive(false);
		gameOverGraphic.gameObject.SetActive(true);

		Time.timeScale = 1;
		yield return new WaitForSeconds (2);
		resetGame();
	}

	void Retry() {
		//Get GO Handler, call "retry"
		StartCoroutine (LoadNewGame());
	}

	IEnumerator LoadNewGame() {
		yield return StartCoroutine(go.sceneManager.fadeOut());
		yield return new WaitForSeconds (1);
		go.Restart();
	}

	void resetGame() {
		Destroy (global.gameObject);
		Application.LoadLevel ("MenuScene");
	}

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
	}
}
