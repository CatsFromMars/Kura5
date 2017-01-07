using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public Transform mainMenu;
	public Transform[] menus;
	public GameObject quickPauseMenu; //Super speical menu
	private int currentIndex = 0;
	private bool menuOpen = false;
	public bool performingAction = false; //can't swap menus in the middle of an action
	//Sounds
	public Transform HUD;
	public AudioClip open;
	public AudioClip close;
	public AudioClip swap;
	public AudioSource music;
	
	// Update is called once per frame
	void Update () {
		//Quick Pause
		if (Input.GetKeyDown(KeyCode.Escape)&& Time.timeScale != 0 && !Input.GetButton("Swap") && !Input.GetButton("Target")) {
			bool canOpen = GetUtil.getPlayerContainer().playerInControl;
			if(!menuOpen&&canOpen) openPauseMenu();
		}

		//Inventory Menu
		if(Input.GetButtonDown("Inventory") && Time.timeScale != 0 && !Input.GetButton("Swap") && !Input.GetButton("Target")) {
			bool canOpen = GetUtil.getPlayerContainer().playerInControl;
			if(!menuOpen&&canOpen) openMenu();
		}
		if (Input.GetButtonDown ("Target") && !performingAction) {
			if(menuOpen) {
				currentIndex = (currentIndex+1)%menus.Length;
				swapToMenu(currentIndex);
			}
		}
		else if (Input.GetButtonDown ("Swap") && !performingAction) {
			if(menuOpen) {
				currentIndex = (currentIndex-1)%menus.Length;
				if(currentIndex == -1) currentIndex = menus.Length-1;
				swapToMenu(currentIndex);
			}
		}
	}

	void openPauseMenu() {
		music.Pause();
		HUD.gameObject.SetActive(false);
		Time.timeScale = 0;
		makeSound (open);
		quickPauseMenu.SetActive(true);
		menuOpen = true;
	}

	public void closePauseMenu() {
		music.Play ();
		menuOpen = false;
		Time.timeScale = 1;
		makeSound (close);
		quickPauseMenu.SetActive (false);
		HUD.gameObject.SetActive (true);
	}

	void openMenu() {
		music.Pause();
		HUD.gameObject.SetActive (false);
		Time.timeScale = 0;
		closeAll();
		makeSound (open);
		menus [currentIndex].gameObject.SetActive (true);
		menuOpen = true;
	}

	void swapToMenu(int index) {
		if(!quickPauseMenu.activeSelf) {
			closeAll();
			makeSound (swap);
			menus [index].gameObject.SetActive (true);
		}
	}

	void closeAll() {
		foreach (Transform menu in menus) {
			menu.gameObject.SetActive(false);
		}
	}

	public void closeMenu() {
		//make noise
		music.Play ();
		HUD.gameObject.SetActive (true);
		Time.timeScale = 1;
		makeSound(close);
		closeAll();
		menuOpen = false;
	}

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
	}
}
