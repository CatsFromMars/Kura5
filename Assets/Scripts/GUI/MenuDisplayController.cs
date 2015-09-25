using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuDisplayController : MonoBehaviour {

	public float space = 1.1f;
	public Transform corner;
	public Inventory inventory;
	public GameObject selector;
	public GameObject modelPoint;
	public Text descriptionText;

	private GameObject currentDisplay;
	private int pushWaitTime = 10;
	public int pushCounter;
	private int index = 0;

	public List<Item> slots = new List<Item>();


	//public GameObject model;

	// Use this for initialization
	void Awake() {
		inventory = GameObject.FindGameObjectWithTag("GameController").GetComponent<Inventory>();
		if (inventory == null || modelPoint == null || selector == null || descriptionText == null)
			Debug.LogError("KeyComponent of the display is null!");

		slots = inventory.itemsList;
		pushCounter = pushWaitTime;
		setUpSlots();
	}

	void Start() {
		displaySlots();
		updateDescription();
	}

	void Update() {
		manageSelector();
		manageInventoryInput();
	}

	void manageInventoryInput() {
		//Manages doing stuff in items (using, moving, trashing, etc)
		if(Input.GetButtonDown("Confirm")) {
			//Use Item
			inventory.selectedItem = slots[index];
			inventory.useCurrentItem(index);
			displaySlots();
		}
	}

	//Manages selector movement. Relies on index variable, which is agnostic to item slots.
	void manageSelector() {
		if(Input.GetKey(KeyCode.RightArrow)) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				pushCounter = 0;
				index = (index+1)%(inventory.slotsX*inventory.slotsY);
				moveSelector();
			}
		}
		else if (Input.GetKey(KeyCode.LeftArrow)) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				pushCounter = 0;
				index = (index-1)%(inventory.slotsX*inventory.slotsY);
				if (index < 0) index = 0;
				moveSelector();
			}
		}
		else if (Input.GetKey(KeyCode.UpArrow)) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				pushCounter = 0;
				index = (index-inventory.slotsY)%(inventory.slotsY*inventory.slotsX);
				if (index < 0) index = 0;
				moveSelector();
			}
		}
		else if (Input.GetKey(KeyCode.DownArrow)) {
			pushCounter++;
			if(pushCounter>=pushWaitTime) {
				pushCounter = 0;
				index = (index+inventory.slotsY)%(inventory.slotsY*inventory.slotsX);
				if (index < 0) index = 0;
				moveSelector();
			}
		}

		if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) ||
		   Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) pushCounter = pushWaitTime;
	}

	//Actually moves the selector itself.
	void moveSelector() {
		int x = index % 4;
		int y = Mathf.FloorToInt(index / 4f);
		Vector3 pos = new Vector3(corner.localPosition.x + x*space, corner.localPosition.y - y*space, -0.2f);
		selector.transform.position = pos;
		snapIntoPlace(selector, pos);
		if(slots[index] != null) updateDescription();
	}

	void updateDescription() {
		descriptionText.text = slots[index].description;
		updateDisplay();
	}

	//Updates the display model
	void updateDisplay() {
		if(currentDisplay != null) Destroy(currentDisplay.gameObject);
		GameObject item = null;
		GameObject model = Resources.Load("Items/" + slots[index].name) as GameObject;
		if(model != null) {
			currentDisplay = Instantiate(model, modelPoint.transform.position, Quaternion.identity) as GameObject;
			currentDisplay.transform.parent = modelPoint.transform;
			currentDisplay.transform.localRotation = Quaternion.Euler(0,0,0);
			currentDisplay.transform.localScale = new Vector3 (1f, 1f, 1f);
		}
	}

	//Displays all the item models according to slots
	void displaySlots() {
		int i = 0;
		for (int y = 0; y < 4; y++) {
			for (int x = 0; x < 4; x++) {
				Vector3 pos = new Vector3(corner.localPosition.x + x*space, corner.localPosition.y - y*space, -0.2f);
				GameObject model = Resources.Load("Items/" + slots[i].name) as GameObject;
				GameObject item = null;
				Debug.Log (slots[i].name);
				if(model != null) {
					item = Instantiate(model, pos, Quaternion.identity) as GameObject;
				}
				else {
					model = Resources.Load("Items/Null") as GameObject;
					item = Instantiate(model, pos, Quaternion.identity) as GameObject;
				}
				snapIntoPlace(item, pos);
				i++;
			}
		}
	}

	//Sets up empty slots
	void setUpSlots() {
		int i = 0;
		for (int y = 0; y < 4; y++) {
			for (int x = 0; x < 4; x++) {
				Vector3 pos = new Vector3(corner.localPosition.x + x*space, corner.localPosition.y - y*space, -0.2f);
				GameObject model = Resources.Load("Items/Null") as GameObject;
				GameObject item = Instantiate(model, pos, Quaternion.identity) as GameObject;
				snapIntoPlace(item, pos);
				i++;
			}
		}
	}

	//Utility function for positioning
	void snapIntoPlace(GameObject obj, Vector3 pos) {
		obj.transform.parent = this.transform;
		obj.transform.localPosition = pos;
		obj.transform.localRotation = Quaternion.Euler(0,0,0);
		obj.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
	}
}
