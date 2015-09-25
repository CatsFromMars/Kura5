using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	//THIS IS SOME KIND OF WEIRD INTERFACE FOR THE INVENTORY
	//CONTAINS ITEM EFFECT DEFINITIONS 
	//ALSO THE LIST FOR THE ACTUAL INVENTORY

	//TO DO STILL
	//ACTUALLY ADD THE HEALING EFFECTS OF THE ITEMS, INCLUDING PLAYER SPECIES
	//MAKE A SORT AND DROP OPTION FOR ITEMS
	//ENEMIES NEED LOOT!
	//

	GameData gameData;
	GameObject globalData;
	int restoreSomeValue;
	private ItemDataBase database;
	public int slotsX, slotsY;
	private bool windowToggle = false;
	private int boxSize = 26;
	private int spaceInBetween = 5;
	private int dx = 300;
	private int dy = 300;
	private bool showDescription = false;
	public Item selectedItem;
	private string currentMenu;

	//GUI BUTTONS
	private Texture2D sortIcon;
	private Texture2D trashIcon;

	//GUI TABS
	private Texture2D InventoryTab;
	private Texture2D KeyItemsTab;
	private Texture2D ElementsTab;

	//SELECTORS
	private Texture2D annieSelector;
	private Texture2D emilSelector;

	private Texture2D selectedTab; //WHICH TAB THE PLAYER SELECTS

	//EFFECTS
	int someRestoreValue = 30;

	//CURRENT INVENTORY 
	public List<Item> inventoryList = new List<Item>();
	//INVENTORY LIST ROIGHT THAR!
	public List<Item> itemsList = new List<Item>();
	//KEY ITEMS LIST FER YER ORBS AND KEYS
	public List<Item> keyItemsList = new List<Item>();
	//ELEMENT ITEMS LIST FOR ELEMENT SWAPPING
	public List<Item> elementList = new List<Item>();

	//SLOTS
	public List<Item> slots = new List<Item>();

	//DISPLAY
	public GameObject menu; //References the menu display. This only handles abstract item stuff

	// Use this for initialization
	void Awake() {

		//LOAD RESOURCES
		//TABS
		InventoryTab = Resources.Load<Texture2D>("GUI/ItemsTab-Inventory");
		KeyItemsTab = Resources.Load<Texture2D>("GUI/ItemsTab-KeyItems");
		ElementsTab = Resources.Load<Texture2D>("GUI/ItemsTab-Elements");

		//SELECTORS
		annieSelector = Resources.Load<Texture2D>("Item Icons/annieSelector");
		emilSelector = Resources.Load<Texture2D>("Item Icons/emilSelector");

		//BUTTONS
		sortIcon = Resources.Load<Texture2D>("Item Icons/Sort");
		trashIcon = Resources.Load<Texture2D>("Item Icons/Trash");

		selectedTab = InventoryTab;

		//GET DATA
		globalData = GameObject.FindGameObjectWithTag("GameController");
		gameData = globalData.GetComponent<GameData>();
		database = globalData.GetComponent<ItemDataBase>();

		//TEST
		itemsList.Add (database.items[0]);
		itemsList.Add (database.items[0]);
		itemsList.Add (database.items[1]);
		itemsList.Add (database.items[0]);
		itemsList.Add (database.items[1]);
		//itemsList.Add (database.items[2]);

		//keyItemsList.Add (database.items[3]);

		//elementList.Add (database.items[6]);
		//elementList.Add (database.items[5]);
		//elementList.Add (database.items[4]);

		//INIT NUMBER OF SLOTS
		slotsX = 4;
		slotsY = 4;

		//INIT SLOTS FOR CURRENT LIST
		for (int i = 0; i < slotsX * slotsY; i++)
		{
			slots.Add(new Item());
			inventoryList.Add(new Item());
		}

		//INIT SLOTS FOR ITEMS
		for (int i = 0; i < slotsX * slotsY; i++)
		{
			slots.Add(new Item());
			itemsList.Add(new Item());
		}

		//INIT SLOTS FOR KEYITEMS
		for (int i = 0; i < slotsX * slotsY; i++)
		{
			slots.Add(new Item());
			keyItemsList.Add(new Item());
		}

		//INIT SLOTS FOR ELEMENTS
		for (int i = 0; i < slotsX * slotsY; i++)
		{
			slots.Add(new Item());
			elementList.Add(new Item());
		}


	}

	//FUNCTIONS MEANT FOR MANIPULATING THE INVENTORY
	//SUCH AS USE ITEM

	public void AddItem(int itemID) {
		Debug.Log ("FUNCTION CAAAAALLL");
		//LOOP THROUGH APPROPRIATE LIST
		List<Item> appropriateList = new List<Item>();
		Item item = database.items[itemID];
		Debug.Log (item.name);
		if(item.type == Item.ItemType.Consumable) appropriateList = itemsList;
		if(item.type == Item.ItemType.Quest) appropriateList = keyItemsList;
		if(item.type == Item.ItemType.Upgrade) appropriateList = elementList;
		for(int i = 0; i < appropriateList.Count; i++)
		{
			if(appropriateList[i].name == null)
			{
				appropriateList[i] = item;
				break;
			}
		}
		
	}

	public int InventoryContains(int itemID, List<Item> inventoryList) {
		//RETURNS INDEX OF ITEM. RETURNS -1 IF NOT FOUND
		for(int i = 0; i < inventoryList.Count; i++)
		{
			if(inventoryList[i].idNumber == itemID)
			{
				return i;
			}
		}
		return -1;
		
	}

	public void useCurrentItem(int index) {
		if(selectedTab == ElementsTab) swapElements(index);

		if(selectedTab == InventoryTab)
		{
			//CHECK TO SEE IF THE PLAYER SHOULD GET SICK
			bool sideEffects = false;
			if(gameData.currentPlayer == GameData.player.Annie && selectedItem.elem == Item.element.Dark) sideEffects = true;
			if(gameData.currentPlayer == GameData.player.Emil && selectedItem.elem == Item.element.Sol) sideEffects = true;

			//APPLY ITEM EFFECTS

			//RESTORE SOME LIFE
			if(selectedItem.effect == Item.ItemEffect.RestoreSomeLife) {
				if(gameData.currentPlayer == GameData.player.Annie) {
					if(sideEffects) gameData.annieCurrentLife -= someRestoreValue;
					else gameData.annieCurrentLife += someRestoreValue;
				}
				if(gameData.currentPlayer == GameData.player.Emil) {
					if(sideEffects) gameData.emilCurrentLife -= someRestoreValue;
					else gameData.emilCurrentLife += someRestoreValue;
				}
			}

			//RESTORE SOME ENERGY
			if(selectedItem.effect == Item.ItemEffect.RestoreSomeEnergy) {
				if(gameData.currentPlayer == GameData.player.Annie) {
					if(sideEffects) gameData.annieCurrentEnergy -= someRestoreValue;
					else gameData.annieCurrentEnergy += someRestoreValue;
				}
				if(gameData.currentPlayer == GameData.player.Emil) {
					if(sideEffects) gameData.emilCurrentEnergy -= someRestoreValue;
					else gameData.emilCurrentEnergy += someRestoreValue;
				}
			}

			//REMOVE ITEM
			removeItem(index, itemsList);
		}
	}

	public void swapElements(int index) {
		if(selectedTab == ElementsTab) {
			if(gameData.currentPlayer == GameData.player.Annie) 
			{
				if(selectedItem.elem == Item.element.Sol) gameData.annieCurrentElem = GameData.elementalProperty.Sol;
				else if(selectedItem.elem == Item.element.Fire) gameData.annieCurrentElem = GameData.elementalProperty.Fire;
				else if(selectedItem.elem == Item.element.Earth) gameData.annieCurrentElem = GameData.elementalProperty.Earth;
				//ADD A "NO" FEEDBACK NOISE HERE
			}

			if(gameData.currentPlayer == GameData.player.Emil) 
			{
				if(selectedItem.elem == Item.element.Dark) gameData.emilCurrentElem = GameData.elementalProperty.Dark;
				else if(selectedItem.elem == Item.element.Frost) gameData.emilCurrentElem = GameData.elementalProperty.Frost;
				else if(selectedItem.elem == Item.element.Cloud) gameData.emilCurrentElem = GameData.elementalProperty.Cloud;
				//ADD A "NO" FEEDBACK NOISE HERE
				
			}

		}
	}

	public void removeItem(int index, List<Item> inventoryList) {
		//MAKE AN EMPTY
		Item empty = new Item();
		//DELETE THE USED ITEM
		inventoryList[index] = empty;
		slots[index] = empty;
	}

	void Update()
	{
		//TOGGLE WINDOW
		if(Input.GetButtonDown("Inventory"))
		{
			//windowToggle = !windowToggle;
			menu.active = !menu.active;

		}

		//Pause the game
		if (menu.active) Time.timeScale = 0;
		else Time.timeScale = 1;

	}

}
