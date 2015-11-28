using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	public Transform inventoryMenu;
	public Animator menuFade;
	private bool timeFrozen = false;
	private PlayerContainer player;
	public AudioClip chirp;
	
	// Update is called once per frame
	void Update () {
		bool canOpenMenu = Time.timeScale != 0 && inventoryMenu.active == false;
		bool canCloseMenu = Time.timeScale == 0 && inventoryMenu.active == true;
		if((canOpenMenu||canCloseMenu) && Input.GetButtonDown("Inventory")) {
			getPlayer();
			//menuFade.SetTrigger(Animator.StringToHash("FadeOut"));
			//menuFade.SetTrigger(Animator.StringToHash("FadeIn"));
			inventoryMenu.active = !inventoryMenu.active;
			timeFrozen = !timeFrozen;
			player.playerInControl = !inventoryMenu.active;
			if(inventoryMenu.active) makeSound();

			if(timeFrozen) Time.timeScale = 0f;
			else Time.timeScale = 1f;
		}
		//else if(Input.GetButtonDown("Attack")) { //Basically the B Button
		//	inventoryMenu.active = false;
		//	timeFrozen = false;
		//	player.playerInControl = true;
		//}
	}

	void getPlayer() {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerContainer>();
	}

	public void makeSound() {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		AudioSource a = inventoryMenu.GetComponent<AudioSource>();
		a.clip = chirp;
		a.Play();
	}
}
