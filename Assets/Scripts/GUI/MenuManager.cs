using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	public Transform mainMenu;
	public Transform[] menus;
	private int currentIndex = 0;
	private bool menuOpen = false;
	public bool performingAction = false; //can't swap menus in the middle of an action
	//Sounds
	public Transform HUD;
	public AudioClip open;
	public AudioClip close;
	public AudioClip swap;


	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Inventory") && Time.timeScale != 0) {
			if(!menuOpen) openMenu();
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

	void openMenu() {
		HUD.gameObject.SetActive (false);
		Time.timeScale = 0;
		closeAll();
		makeSound (open);
		menus [0].gameObject.SetActive (true);
		menuOpen = true;
	}

	void swapToMenu(int index) {
		closeAll();
		makeSound (swap);
		menus [index].gameObject.SetActive (true);
	}

	void closeAll() {
		foreach (Transform menu in menus) {
			menu.gameObject.SetActive(false);
		}
	}

	public void closeMenu() {
		//make noise
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
