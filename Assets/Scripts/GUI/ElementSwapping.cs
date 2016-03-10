using UnityEngine;
using System.Collections;

public class ElementSwapping : MonoBehaviour {

	public Inventory inventory;
	public GameData data;
	public LensGUI gui;
	private int[] annieLens; //valid lenses Annie can swap to
	private int[] emilLens; //valid lenses Emil can swap to
	int sol;
	int dark;
	int fire;
	int frost;
	int earth;
	int cloud;
	int luna;
	int astro;
	int empty;
	private int annieIndex = 0;
	private int emilIndex = 0;
	public Transform arrow;
	private bool open = false;
	public AudioClip swapSound;
	public AudioClip openSound;

	void Awake() {
		annieLens = new int[5];
		emilLens = new int[6];
	}

	void getCurrentIndexes() {
		//To be filled in later
	}

	void updateList() {
		sol = inventory.checkForLens(0);
		dark = inventory.checkForLens(1);
		fire = inventory.checkForLens(2);
		frost = inventory.checkForLens(3);
		earth = inventory.checkForLens(4);
		cloud = inventory.checkForLens(5);
		luna = inventory.checkForLens(6);
		astro = inventory.checkForLens(7);
		empty = inventory.checkForLens(8);

		annieLens[0] = sol; //Sol
		annieLens[1] = fire; //Fire
		annieLens[2] = earth; //Earth
		annieLens[3] = luna; //Luna
		annieLens[4] = astro; //Astro

		emilLens[0] = dark; //Dark
		emilLens[1] = frost; //Frost
		emilLens[2] = cloud; //Cloud
		emilLens[3] = luna; //Luna
		emilLens[4] = astro; //Astro
		emilLens[5] = empty; //Empty

		gui.fireLens.gameObject.SetActive(fire!=-1);
		gui.frostLens.gameObject.SetActive(frost!=-1);
		gui.earthLens.gameObject.SetActive(earth!=-1);
		gui.cloudLens.gameObject.SetActive(cloud!=-1);
		gui.lunaLens.gameObject.SetActive(luna!=-1);
		gui.astroLens.gameObject.SetActive(astro!=-1);
		gui.astroLens.gameObject.SetActive(empty!=-1);
	}

	// Update is called once per frame
	void Update () {
		updateDisplay ();

		if(Input.GetButton("Swap")) { //if holding button down
			if(!open) {
				open = true;
				Time.timeScale = 0;
				arrow.gameObject.SetActive(true);
				makeSound(openSound);
			}
			if(Input.GetButtonDown("Target")) toggleLens();
		}
		else {
			if(open) {
				open = false;
				arrow.gameObject.SetActive(false);
				Time.timeScale = 1;
			}
		}
	}

	void updateDisplay() {
		if (data.currentPlayer == GameData.player.Annie) {
			gui.solLens.gameObject.SetActive(true);
			//int inventoryLocation = annieLens[annieIndex];
			//string l = inventory.lensList[inventoryLocation].name;
			if(gui.curDisplay != data.annieCurrentElem.ToString()) gui.swapTo(data.annieCurrentElem.ToString());
		}
		else if(data.currentPlayer == GameData.player.Emil) {
			gui.solLens.gameObject.SetActive(false);
			//int inventoryLocation = emilLens[emilIndex];
			//string l = inventory.lensList[inventoryLocation].name;
			if(gui.curDisplay != data.emilCurrentElem.ToString()) gui.swapTo(data.emilCurrentElem.ToString());
		}
	}

	void toggleLens() {
		updateList ();
		makeSound(swapSound);
		if (data.currentPlayer == GameData.player.Annie) {
			//Is probably dangerous in the event there are no lenses...
			gui.solLens.gameObject.SetActive(true);


			annieIndex = (annieIndex+1)%annieLens.Length;
			int inventoryLocation = annieLens[annieIndex];
			while(inventoryLocation == -1) {
				annieIndex = (annieIndex+1)%annieLens.Length;
				inventoryLocation = annieLens[annieIndex];
			}
			inventory.EquipLens(inventoryLocation);
			gui.swapTo(inventory.lensList[inventoryLocation].element);
		}
		else if(data.currentPlayer == GameData.player.Emil) {
			//Is probably dangerous in the event there are no lenses...
			gui.solLens.gameObject.SetActive(false);

			emilIndex = (emilIndex+1)%emilLens.Length;
			int inventoryLocation = emilLens[emilIndex];
			while(inventoryLocation == -1) {
				emilIndex = (emilIndex+1)%emilLens.Length;
				inventoryLocation = emilLens[emilIndex];
			}
			inventory.EquipLens(inventoryLocation);
			gui.swapTo(inventory.lensList[inventoryLocation].element);
		}

	}
	
	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
	}
	
}
