using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour {
	public MenuManager manager;
	public AudioSource menuAudio;
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
	private int swapIndex = 0;
	private string trashPrompt;

	float horiz;
	float vert;

	//Sounds
	public AudioClip selectNoise;
	public AudioClip confirm;
	public AudioClip use;
	public AudioClip deny;
	public AudioClip open;
	public AudioClip trash;

	public enum player {ANNIE, EMIL};
	private player selectedPlayer;

	public enum listKind {CONSUMABLES, VALUABLES, LENS, GUN, SWORD};
	public listKind itemDisplayType;

	public enum state {SELECT_STATE, SELECT_ITEM, SWAP_ITEM, SELECT_PLAYER, PROMPT_DELETE};
	private state currentState;

	public enum function {USE, SWAP, DELETE};
	private function currentFunction;

	public enum prompt {YES, NO}
	private prompt deletePrompt;

	//Selector Variables
	int currentSelection = 0; //index of current inventory slot
	public Transform selector;
	public Transform playerSelector;
	public Transform stateSelector;
	public Transform swapSelector;
	public Transform deleteSelector;

	//State Variables
	public Transform useState;
	public Transform sortState;
	public Transform deleteState;

	//Slider Variables
	public Slider annieENESlider;
	public Slider annieLIFESlider;
	public Slider emilLIFESlider;
	public Slider emilENESlider;

	//Current Elem Variables
	public Sprite darkText;
	public Sprite solText;
	public Sprite fireText;
	public Sprite frostText;
	public Sprite earthText;
	public Sprite cloudText;
	public Sprite lunaText;
	public Sprite astroText;
	public Sprite noneText;
	public SpriteRenderer annieCurElem;
	public SpriteRenderer emilCurElem;
	
	public List<Consumable> slots = new List<Consumable>();
	public List<KeyItem> keySlots = new List<KeyItem>();

	void Awake() {
		trashPrompt = "        YES, TRASH IT\n         NO, KEEP IT";
		GameObject global = GameObject.FindGameObjectWithTag ("GameController");
		inventory = global.GetComponent<Inventory>();
		data = global.GetComponent<GameData>();

		if (inventory == null || selector == null || data == null)
			Debug.LogError("KeyComponent of the display is null!");

		//ALSO SWAP OUT ITEMS HERE
		slots = inventory.itemsList;
		keySlots = inventory.keyItemsList;

		pushCounter = pushWaitTime;
		selectedPlayer = player.ANNIE;
		currentState = state.SELECT_STATE;
		currentFunction = function.USE;
	}

	// Update is called once per frame
	void Start() {
		drawSlots ();
		redrawAll ();
	}

	void OnEnable() {
		pushCounter = pushWaitTime;
		//selectedPlayer = player.ANNIE;
		currentState = state.SELECT_STATE;
		//currentFunction = function.USE;
		//deletePrompt = prompt.YES;
		selector.active = false;
		swapSelector.active = false;
		playerSelector.active = false;
		deleteSelector.active = false;
		redrawAll();
	}

	void Update() {
		horiz = Input.GetAxisRaw ("Horizontal");
		vert = Input.GetAxisRaw ("Vertical");
		if(currentState == state.SELECT_STATE) selectState();
		else if(currentState == state.SWAP_ITEM) swapItem(swapIndex);
		else if(currentState == state.SELECT_ITEM) selectItemOnGrid();
		else if(currentState == state.SELECT_PLAYER) selectPlayer();
		else if(currentState == state.PROMPT_DELETE) promptDeletion();

		if(currentState != state.SELECT_STATE) manager.performingAction = true;
		else manager.performingAction = false;
	}

	void updateSliders() {
		annieLIFESlider.value = data.annieCurrentLife;
		annieENESlider.value = data.annieCurrentEnergy;
		emilLIFESlider.value = data.emilCurrentLife;
		emilENESlider.value = data.emilCurrentEnergy;
		GameData.elementalProperty a = data.annieCurrentElem;
		GameData.elementalProperty e = data.emilCurrentElem;
		if(a==GameData.elementalProperty.Sol) annieCurElem.sprite=solText;
		else if(a==GameData.elementalProperty.Fire) annieCurElem.sprite=fireText;
		else if(a==GameData.elementalProperty.Earth) annieCurElem.sprite=earthText;
		if(e==GameData.elementalProperty.Dark) emilCurElem.sprite=darkText;
		else if(e==GameData.elementalProperty.Frost) emilCurElem.sprite=frostText;
		else if(e==GameData.elementalProperty.Cloud) emilCurElem.sprite=cloudText;
		else if(e==GameData.elementalProperty.Null) emilCurElem.sprite=noneText;
	}

	bool selectedPlayerNotDead() {
		if(selectedPlayer == player.ANNIE) return (data.annieCurrentLife > 0);
		else if(selectedPlayer == player.EMIL) return (data.emilCurrentLife > 0);
		else return false;
	}

	void selectState() {
		stateSelector.active = true;
		playerSelector.active = false;
		selector.active = false;

		if (Input.GetButtonDown ("Confirm")) { //Charge is basically the A button
			if(currentFunction == function.USE || currentFunction == function.DELETE) {
				currentState = state.SELECT_ITEM;
				makeSound(confirm);
			}
			else if(currentFunction == function.SWAP) {
				currentState = state.SELECT_ITEM;
				makeSound(confirm);
			}
		}
		else if(Input.GetButtonDown("Deny")) { //Basically the "B" button
			manager.closeMenu();
		}
		else if (vert == -1) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				Vector3 pos = stateSelector.localPosition;
				makeSound(selectNoise);
				pushCounter = 0;
				if(currentFunction == function.USE) {
					pos.y = -8.3f;
					currentFunction = function.SWAP;
				}
				else if(currentFunction == function.SWAP) {
					if(deleteState != null) {
						pos.y = -16.7f;
						currentFunction = function.DELETE;
					}
					else {
						pos.y = 0;
						currentFunction = function.USE;
					}
				}
				else if (currentFunction == function.DELETE) {
					pos.y = 0;
					currentFunction = function.USE;
				}
				stateSelector.localPosition = pos;
			}
		}
		else if (vert == 1) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				Vector3 pos = stateSelector.localPosition;
				makeSound(selectNoise);
				pushCounter = 0;

				if(currentFunction == function.USE) {
					if(deleteState != null) {
						pos.y = -16.7f;
						currentFunction = function.DELETE;
					}
					else {
						pos.y = -8.3f;
						currentFunction = function.SWAP;
					}
				}
				else if(currentFunction == function.SWAP) {
					pos.y = 0;
					currentFunction = function.USE;
				}
				else if (currentFunction == function.DELETE) {
					pos.y = -8.3f;
					currentFunction = function.SWAP;
				}
				stateSelector.localPosition = pos;
			}
		}
		else pushCounter = pushWaitTime;


	}

	void promptDeletion() {
		deleteSelector.active = true;
		description.text = trashPrompt;
		name.text = "TRASH THIS?";
		//Confirm
		if(Input.GetButtonDown("Confirm")) { //Charge is basically the A button
			Vector3 pos = deleteSelector.localPosition;

			if(deletePrompt == prompt.YES) {
				makeSound(trash);
				inventory.removeConsumableItem(index);
			}
			else {
				makeSound(deny);
			}

			redrawAll();
			pos.y = -7.88f;
			deletePrompt = prompt.YES;
			deleteSelector.localPosition = pos;
			currentState = state.SELECT_ITEM;
			deleteSelector.active = false;
		}
		else if(Input.GetButtonDown("Deny")) { //Basically the "B" button
			Vector3 pos = deleteSelector.localPosition;
			pos.y = -7.88f;
			deletePrompt = prompt.YES;

			redrawAll();
			currentState = state.SELECT_ITEM;
			deleteSelector.localPosition = pos;
			deleteSelector.active = false;
			makeSound(deny);
		}
		else if (vert != 0) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				pushCounter = 0;
				Vector3 pos = deleteSelector.localPosition;
				if(deletePrompt == prompt.NO) {
					deletePrompt = prompt.YES;
					pos.y = -7.88f;
					makeSound(selectNoise);
				}
				else if(deletePrompt == prompt.YES) {
					deletePrompt = prompt.NO;
					pos.y = -8.57f;
					makeSound(selectNoise);
				}
				
				deleteSelector.localPosition = pos;
			}
		}
		else pushCounter = pushWaitTime;
	}

	void selectPlayer() {
		//Select Player to use an item on
		playerSelector.active = true;
		//Confirm
		if(Input.GetButtonDown("Confirm")) { //Charge is basically the A button
			if(inventory.useCurrentConsumable(index, selectedPlayer.ToString())) {
				//inventory.useCurrentConsumable(index, selectedPlayer.ToString());
				currentState = state.SELECT_ITEM;
				redrawAll();
				playerSelector.active = false;
				makeSound(use);
			}
			else makeSound(deny);
		}
		else if(Input.GetButtonDown("Deny")) { //Basically the "B" button
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

	bool itemNotNull() {
		if(itemDisplayType == listKind.CONSUMABLES) return slots[index].name != null;
		else if(itemDisplayType == listKind.VALUABLES) return keySlots[index].name != null;
		else return false;
	}

	void selectItemOnGrid() {

		selector.active = true;
		swapSelector.active = false;
		//Select Item
		if(Input.GetButtonDown("Confirm")) { //Charge is basically the A button
			if(itemNotNull()) {
				if(currentFunction == function.USE) {
					if(itemDisplayType == listKind.CONSUMABLES) currentState = state.SELECT_PLAYER;
					else if(itemDisplayType == listKind.VALUABLES) {
						//use key item
						inventory.useCurrentKeyItem(index);
						//exit menu
						manager.closeMenu();
					}
				}
				else if(currentFunction == function.SWAP) {
					moveSelector();
					moveSwapSelector();
					swapIndex = index;
					currentState = state.SWAP_ITEM;
				}
				else if(currentFunction == function.DELETE) {
					currentState = state.PROMPT_DELETE;
				}
				makeSound(confirm);
			}
		}
		else if(Input.GetButtonDown("Deny")) { //Basically the "B" button
			currentState = state.SELECT_STATE;
			swapSelector.active = false;
			makeSound(deny);
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

	void swapItem(int currentIndex) {
		swapSelector.active = true;
		//Select Item
		if(Input.GetButtonDown("Confirm")) { //Charge is basically the A button
			if(itemDisplayType == listKind.CONSUMABLES) ListUtil.SwapConsumables(inventory.itemsList, currentIndex, index);
			else if(itemDisplayType == listKind.VALUABLES) ListUtil.SwapValuables(inventory.keyItemsList, currentIndex, index);
			currentState = state.SELECT_ITEM;
			redrawAll();
			moveSelector();
			moveSwapSelector();
			makeSound(confirm);
		}
		else if(Input.GetButtonDown("Deny")) { //Basically the "B" button
			currentState = state.SELECT_ITEM;
			swapSelector.active = false;
			makeSound(deny);
		}
		else if (horiz == 1) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				pushCounter = 0;
				int row = Mathf.FloorToInt(index / inventory.slotsX);
				int col = index % inventory.slotsX;
				index = ((col+1) % (inventory.slotsX)) + row*inventory.slotsY;
				moveSwapSelector();
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
				moveSwapSelector();
			}
		}
		else if (vert == -1) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				pushCounter = 0;
				index = (index+inventory.slotsY)%(inventory.slotsY*inventory.slotsX);
				moveSwapSelector();
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
				moveSwapSelector();
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
	//Move swap selector
	void moveSwapSelector() {
		int x = index % inventory.slotsX;
		int y = Mathf.FloorToInt(index / inventory.slotsX);
		Vector3 pos = new Vector3(x*37, y*-37, 0);
		swapSelector.transform.localPosition = pos;
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
				//List is displayed here
				if(itemDisplayType == listKind.CONSUMABLES) displayItem(inventory.itemsList[index].model, pos);
				else if(itemDisplayType == listKind.VALUABLES) displayItem(inventory.keyItemsList[index].model, pos);
				index++;
			}
		}
		displayModel();
		updateSliders();
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
		GameObject model = getModel();
		if(model != null) {
			currentDisplayModel = Instantiate(model, modelPoint.transform.position, Quaternion.identity) as GameObject;
			currentDisplayModel.transform.parent = modelPoint.transform;
			currentDisplayModel.transform.localRotation = Quaternion.Euler(0,0,0);
			currentDisplayModel.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
			description.text = getDescription();
			name.text = getName().ToUpper();
		}
		else {
			description.text = "";
			name.text = "";
		}
	}

	private string getName() {
		if(itemDisplayType == listKind.CONSUMABLES) return slots[index].name;
		else if(itemDisplayType == listKind.VALUABLES) return keySlots[index].name;
		else return "";
	}
	private string getDescription() {
		if(itemDisplayType == listKind.CONSUMABLES) return slots[index].description;
		else if(itemDisplayType == listKind.VALUABLES) return keySlots[index].description;
		else return "";
	}
	private GameObject getModel() {
		if(itemDisplayType == listKind.CONSUMABLES) return Resources.Load("Items/" + slots[index].name) as GameObject;
		else if(itemDisplayType == listKind.VALUABLES) return Resources.Load("Items/" + keySlots[index].name) as GameObject;
		else return null;
	}

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		menuAudio.clip = clip;
		menuAudio.Play();
	}
}
