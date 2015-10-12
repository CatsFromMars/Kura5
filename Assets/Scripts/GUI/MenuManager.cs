using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	public Transform inventoryMenu;
	public Animator menuFade;
	private bool timeFrozen = false;
	private PlayerContainer player;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Inventory")) {
			getPlayer();
			//menuFade.SetTrigger(Animator.StringToHash("FadeOut"));
			//menuFade.SetTrigger(Animator.StringToHash("FadeIn"));
			inventoryMenu.active = !inventoryMenu.active;
			timeFrozen = !timeFrozen;
			player.playerInControl = !inventoryMenu.active;
		}
		//else if(Input.GetButtonDown("Attack")) { //Basically the B Button
		//	inventoryMenu.active = false;
		//	timeFrozen = false;
		//	player.playerInControl = true;
		//}
		if(timeFrozen) Time.timeScale = 0f;
		else Time.timeScale = 1f;
	}

	void getPlayer() {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerContainer>();
	}
}
