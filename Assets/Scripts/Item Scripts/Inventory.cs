using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	GameData gameData;
	GameObject globalData;
	ItemDatabase database;

	//25 Item Slots, no more, no less.
	public int slotsX = 5;
	public int slotsY = 5;

	//Wrapper List
	public List<Item> slots = new List<Item>();

	//Storage
	public List<Consumable> itemsList = new List<Consumable>();
	public List<KeyItem> keyItemsList = new List<KeyItem>();
	public List<Lens> lensList = new List<Lens>();

	void Awake() {
		//GET DATA
		globalData = GameObject.FindGameObjectWithTag("GameController");
		gameData = globalData.GetComponent<GameData>();
		database = globalData.GetComponent<ItemDatabase>();

		database.initItems (); //LOAD ITEMS

		//For testing purposes. Adds two apples.
		itemsList.Add (database.consumableItems[0]);
		itemsList.Add (database.consumableItems[0]);
		itemsList.Add (database.consumableItems[1]);
		itemsList.Add (database.consumableItems[1]);
		itemsList.Add (database.consumableItems[1]);
		itemsList.Add (database.consumableItems[2]);
		itemsList.Add (database.consumableItems[2]);
		itemsList.Add (database.consumableItems[3]);
		itemsList.Add (database.consumableItems[3]);
		itemsList.Add (database.consumableItems[4]);
		itemsList.Add (database.consumableItems[0]);
		itemsList.Add (database.consumableItems[0]);
		itemsList.Add (database.consumableItems[1]);
		itemsList.Add (database.consumableItems[1]);
		itemsList.Add (database.consumableItems[1]);
		itemsList.Add (database.consumableItems[2]);
		itemsList.Add (database.consumableItems[2]);
		itemsList.Add (database.consumableItems[3]);
		itemsList.Add (database.consumableItems[3]);
		itemsList.Add (database.consumableItems[4]);



		//INIT SLOTS FOR ITEMS
		for (int i = 0; i < slotsX * slotsY; i++)
		{
			slots.Add(new Item());
			itemsList.Add(new Consumable());
		}
		
		//INIT SLOTS FOR KEYITEMS
		for (int i = 0; i < slotsX * slotsY; i++)
		{
			slots.Add(new Item());
			keyItemsList.Add(new KeyItem());
		}
		
		//INIT SLOTS FOR ELEMENTS
		for (int i = 0; i < slotsX * slotsY; i++)
		{
			slots.Add(new Item());
			lensList.Add(new Lens());
		}
	}

	//Functions for manipulating inventory

	public bool AddConsumable(int itemID) {
		//Add item to inventory via item id
		//If returns false, that means it failed to add the item
		Consumable item = database.consumableItems[itemID];
		for(int i = 0; i < slotsX*slotsY; i++)
		{
			if(itemsList[i].name == null)
			{
				itemsList[i] = item;
				return true;
			}
		}
		return false;
	}

	public bool AddKeyItem(int itemID) {
		//Add item to inventory via item id
		//If returns false, that means it failed to add the item
		KeyItem item = database.keyItems[itemID];
		for(int i = 0; i < slotsX*slotsY; i++)
		{
			if(keyItemsList[i].name == null)
			{
				keyItemsList[i] = item;
				return true;
			}
		}
		return false;
	}

	public bool AddLens(int itemID) {
		//Add item to inventory via item id
		//If returns false, that means it failed to add the item
		Lens item = database.lens[itemID];
		for(int i = 0; i < slotsX*slotsY; i++)
		{
			if(lensList[i].name == null)
			{
				lensList[i] = item;
				return true;
			}
		}
		return false;
	}

	public int checkForKeyItem(int itemID) {
		//RETURNS INDEX OF ITEM. RETURNS -1 IF NOT FOUND
		for(int i = 0; i < slotsX*slotsY; i++)
		{
			if(keyItemsList[i].id == itemID)
			{
				return i;
			}
		}
		return -1;
	}

	public void removeKeyItem(int index) {
		KeyItem empty = new KeyItem();
		keyItemsList [index] = empty;
		slots [index] = empty;
	}

	public void removeConsumableItem(int index) {
		Consumable empty = new Consumable();
		itemsList [index] = empty;
		slots [index] = empty;
	}

	public void useCurrentConsumable(int index, string player) { //player = ANNIE or EMIL
		//TO BE EDITED LATER
		Consumable item = itemsList[index];
		//Restore Life
		if (item.effect == "RESTORE_LIFE") {
			int healing = item.strength;
			if(item.preference != player && item.preference != "NONE") {
				healing = Mathf.FloorToInt(healing*0.2f);
			}
			if(player == "ANNIE" && gameData.annieCurrentLife > 0) gameData.annieCurrentLife += healing;
			else if(player == "EMIL" && gameData.emilCurrentLife > 0) gameData.emilCurrentLife += healing;
		}
		//Restore Energy
		if (item.effect == "RESTORE_ENERGY") {
			int healing = item.strength;
			if(item.preference != player && item.preference != "NONE") {
				healing = Mathf.FloorToInt(healing*0.2f);
			}
			if(player == "ANNIE") gameData.annieCurrentEnergy += healing;
			else if(player == "EMIL") gameData.emilCurrentEnergy += healing;
		}

		//Remove item
		removeConsumableItem(index);
	}

}
