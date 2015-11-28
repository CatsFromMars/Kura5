using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour {
	public Inventory inventory;
	public GameData data;
	public Transform corner;
	public GameObject slot;
	public Transform modelPoint;
	public Text description;
	public Text name;
	private GameObject currentDisplayModel;
	private int delta = 37;
	private int pushWaitTime = 15;
	public int pushCounter;
	private int index = 0;

	float horiz;
	float vert;

	//Sounds
	private AudioSource audio;
	public AudioClip selectNoise;
	public AudioClip confirm;
	public AudioClip use;
	public AudioClip deny;
	public AudioClip open;

	public enum player {ANNIE, EMIL};
	private player selectedPlayer;

	public enum state {SELECT_ITEM, SELECT_PLAYER};
	private state currentState;

	//Selector Variables
	int currentSelection = 0; //index of current inventory slot
	public Transform selector;
	public Transform playerSelector;

	//Slider Variables
	public Slider annieENESlider;
	public Slider annieLIFESlider;
	public Slider emilLIFESlider;
	public Slider emilENESlider;
	
	public List<Consumable> slots = new List<Consumable>();

	void Awake() {
		audio = GetComponent<AudioSource>();
		GameObject global = GameObject.FindGameObjectWithTag ("GameController");
		inventory = global.GetComponent<Inventory>();
		data = global.GetComponent<GameData>();

		if (inventory == null || selector == null || data == null)
			Debug.LogError("KeyComponent of the display is null!");
		slots = inventory.itemsList;
		pushCounter = pushWaitTime;
		selectedPlayer = player.ANNIE;
		currentState = state.SELECT_ITEM;
	}

	// Update is called once per frame
	void Start() {
		drawSlots ();
		redrawAll ();
	}

	void OnEnable() {
		currentState = state.SELECT_ITEM;
		playerSelector.active = false;
		redrawAll();
	}

	void Update() {
		horiz = Input.GetAxisRaw ("Horizontal");
		vert = Input.GetAxisRaw ("Vertical");
		if(currentState == state.SELECT_ITEM) selectItemOnGrid();
		else if(currentState == state.SELECT_PLAYER) selectPlayer();
	}

	void updateSliders() {
		annieLIFESlider.value = data.annieCurrentLife;
		annieENESlider.value = data.annieCurrentEnergy;
		emilLIFESlider.value = data.emilCurrentLife;
		emilENESlider.value = data.emilCurrentEnergy;

	}

	bool selectedPlayerNotDead() {
		if(selectedPlayer == player.ANNIE) return (data.annieCurrentLife > 0);
		else if(selectedPlayer == player.EMIL) return (data.emilCurrentLife > 0);
		else return false;
	}

	void selectPlayer() {
		//Select Player to use an item on
		playerSelector.active = true;
		//Confirm
		if(Input.GetButtonDown("Charge")) { //Charge is basically the A button
			if(selectedPlayerNotDead()) {
				inventory.useCurrentConsumable(index, selectedPlayer.ToString());
				currentState = state.SELECT_ITEM;
				redrawAll();
				playerSelector.active = false;
				makeSound(use);
			}
			else makeSound(deny);
		}
		else if(Input.GetButtonDown("Attack")) { //Basically the "B" button
			currentState = state.SELECT_ITEM;
			playerSelector.active = false;
			makeSound(deny);
		}
		else if (vert != 0) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				pushCounter = 0;
				Vector3 pos = playerSelector.localPosition;
				if(selectedPlayer == player.ANNIE) {
					selectedPlayer = player.EMIL;
					pos.y = -121f;
					makeSound(selectNoise);
				}
				else if(selectedPlayer == player.EMIL) {
					selectedPlayer = player.ANNIE;
					pos.y = -30f;
					makeSound(selectNoise);
				}

				playerSelector.localPosition = pos;
			}
		}
		else pushCounter = pushWaitTime;
	}

	void selectItemOnGrid() {

		//Select Item
		if(Input.GetButtonDown("Charge")) { //Charge is basically the A button
			if(slots[index].name != null) { 
				currentState = state.SELECT_PLAYER;
				makeSound(confirm);
			}
		}
		else if (horiz == 1) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				pushCounter = 0;
				int row = Mathf.FloorToInt(index / inventory.slotsX);
				int col = index % inventory.slotsX;
				index = ((col+1) % (inventory.slotsX)) + row*inventory.slotsY;
				moveSelector();
			}
		}
		else if (horiz == -1) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				pushCounter = 0;
				int row = Mathf.FloorToInt(index / inventory.slotsX);
				int col = index % inventory.slotsX;
				col = col-1;
				if(col<0) col = inventory.slotsX-1;
				index = ((col) % (inventory.slotsX)) + row*inventory.slotsY;
				moveSelector();
			}
		}
		else if (vert == -1) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				pushCounter = 0;
				index = (index+inventory.slotsY)%(inventory.slotsY*inventory.slotsX);
				moveSelector();
			}
		}
		else if (vert == 1) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				pushCounter = 0;
				int row = Mathf.FloorToInt(index / inventory.slotsX);
				int col = index % inventory.slotsX;
				row = row-1;
				if(row<0) row = inventory.slotsY-1;
				index = col + (row*inventory.slotsY);
				moveSelector();
			}
		}
		else pushCounter = pushWaitTime;
	}

	//Actually moves the selector itself.
	void moveSelector() {
		int x = index % inventory.slotsX;
		int y = Mathf.FloorToInt(index / inventory.slotsX);
		Vector3 pos = new Vector3(x*37, y*-37, 0);
		selector.transform.localPosition = pos;
		//if(slots[index] != null) updateDescription();
		displayModel ();
		makeSound (selectNoise);
	}

	void clearAll() {
		GameObject[] itemIcons = GameObject.FindGameObjectsWithTag("ItemGUI");
		foreach(GameObject item in itemIcons) {
			Destroy(item.gameObject);
		}
	}

	void drawSlots() {
		int index = 0;
		for (int i=0; i<5; i++) {
			for(int j=0; j<5; j++) {
				Vector3 pos = new Vector3(37*j, -37*i, 9.5f);
				displayItem(slot, pos);
				index++;
			}
		}
	}

	void redrawAll() { //Hard-coded at 25 slots
		clearAll ();
		int index = 0;
		for(int i=0; i<5; i++) {
			for(int j=0; j<5; j++) {
				Vector3 pos = new Vector3(37*j, -37*i, 0);
				displayItem(inventory.itemsList[index].model, pos);
				index++;
			}
		}
		displayModel();
		updateSliders ();
	}

	void displayItem(GameObject model, Vector3 pos) {
		if (model == null)
			return;
		else {
			GameObject item = Instantiate (model, Vector3.zero, Quaternion.identity) as GameObject;
			item.transform.parent = corner;
			item.transform.localPosition = pos;
			item.transform.localRotation = Quaternion.Euler (0, 0, 0);
		}
	}

	void displayModel() {
		modelPoint.transform.localRotation = Quaternion.identity;
		if(currentDisplayModel != null) Destroy(currentDisplayModel.gameObject);
		GameObject model = Resources.Load("Items/" + slots[index].name) as GameObject;
		if(model != null) {
			currentDisplayModel = Instantiate(model, modelPoint.transform.position, Quaternion.identity) as GameObject;
			currentDisplayModel.transform.parent = modelPoint.transform;
			currentDisplayModel.transform.localRotation = Quaternion.Euler(0,0,0);
			currentDisplayModel.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
			description.text = slots[index].description;
			name.text = slots[index].name.ToUpper();
		}
		else { 
			description.text = "";
			name.text = "";
		}
	}

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
	}
}
