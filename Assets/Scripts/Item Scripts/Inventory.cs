using UnityEngine;
using System;
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

		//For testing purposes.
		itemsList.Add (database.consumableItems[0]);
		itemsList.Add (database.consumableItems[1]);
		itemsList.Add (database.consumableItems[2]);
		itemsList.Add (database.consumableItems[3]);
		itemsList.Add (database.consumableItems[4]);
		keyItemsList.Add (database.keyItems[2]);
		keyItemsList.Add (database.keyItems[1]);

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

	void Start() {
		//Lens
		AddLens (3);
		AddLens (4);
		AddLens (5);

		AddLens (0); //Sol
		AddLens (2); //Fire
		AddLens (1); //Dark

		AddLens (8); //Empty
	}

	//Functions for manipulating inventory

	public int checkForConsumable(int itemID) {
		//RETURNS INDEX OF ITEM. RETURNS -1 IF NOT FOUND
		for(int i = 0; i < slotsX*slotsY; i++)
		{
			if(itemsList[i].id==itemID && itemsList[i].name!=null)
			{
				return i;
			}
		}
		return -1;
	}
	
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
			if(i >= lensList.Count) return false;
			if(lensList[i].name == null)
			{
				lensList[i] = item;
				return true;
			}
		}
		return false;
	}

	public bool EquipLens(int index) {
		Lens lens = lensList [index];
		if (lens.preference == "ANNIE") {
			if(lens.element == "Sol") gameData.annieCurrentElem = GameData.elementalProperty.Sol;
			else if(lens.element == "Fire") gameData.annieCurrentElem = GameData.elementalProperty.Fire;
			else if(lens.element == "Earth") gameData.annieCurrentElem = GameData.elementalProperty.Earth;
			else return false;
		}
		else if (lens.preference == "EMIL") {
			if(lens.element == "Dark") gameData.emilCurrentElem = GameData.elementalProperty.Dark;
			else if(lens.element == "Frost") gameData.emilCurrentElem = GameData.elementalProperty.Frost;
			else if(lens.element == "Cloud") gameData.emilCurrentElem = GameData.elementalProperty.Cloud;
			else if(lens.element == "Null") gameData.emilCurrentElem = GameData.elementalProperty.Null;
			else return false;
		}
		else return false; //Trying to equip an invalid lens

		return true; //Made it out alright! 
	}

	public int checkForLens(int lensID) {
		//RETURNS INDEX OF ITEM. RETURNS -1 IF NOT FOUND
		for(int i = 0; i < slotsX*slotsY; i++)
		{
			if(lensList[i].id == lensID)
			{
				return i;
			}
		}
		return -1;
	}

	public int checkForKeyItem(int itemID) {
		//RETURNS INDEX OF ITEM. RETURNS -1 IF NOT FOUND
		for(int i = 0; i < slotsX*slotsY; i++)
		{
			if(keyItemsList[i].id==itemID && keyItemsList[i].name!=null)
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

	public bool useCurrentKeyItem(int index) {
		KeyItem item = keyItemsList[index];
		if (item.effect == "COFFIN") {
			//Use Coffin: UNSAFE! FIX LATER!
			StealthCoffin c = GameObject.Find ("StealthCoffin").GetComponent<StealthCoffin>();
			c.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContainer>();
			c.player.inCoffin = !c.player.inCoffin;

			return true;
		}
		else if(item.effect == "SUMMONS_DOOMY") {
			SceneTransition scene = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>();
			scene.gotoScene("DarkLoans", true, false, true);
		}



		return false;
	}

	public bool useCurrentConsumable(int index, string player) { //player = ANNIE or EMIL
		//TO BE EDITED LATER
		Consumable item = itemsList[index];
		bool itemUsed = false;
		//Restore Life
		if (item.effect == "RESTORE_LIFE") {
			int healing = item.strength;
			if(item.preference != player && item.preference != "NONE") {
				healing = Mathf.FloorToInt(healing*0.2f);
			}
			if(player == "ANNIE" && gameData.annieCurrentLife > 0 && gameData.annieCurrentLife < gameData.annieMaxLife) {
				gameData.annieCurrentLife += healing;
				itemUsed = true;
			}
			else if(player == "EMIL" && gameData.emilCurrentLife > 0 && gameData.emilCurrentLife < gameData.emilMaxLife) {
				gameData.emilCurrentLife += healing;
				itemUsed = true;
			}
		}
		//Restore Energy
		if (item.effect == "RESTORE_ENERGY") {
			int healing = item.strength;
			if(item.preference != player && item.preference != "NONE") {
				healing = Mathf.FloorToInt(healing*0.2f);
			}
			if(player == "ANNIE" && gameData.annieCurrentLife > 0 && gameData.annieCurrentEnergy < gameData.annieMaxEnergy) {
				gameData.annieCurrentEnergy += healing;
				itemUsed = true;
			}
			else if(player == "EMIL" && gameData.emilCurrentLife > 0 && gameData.emilCurrentEnergy < gameData.emilMaxEnergy) {
				gameData.emilCurrentEnergy += healing;
				itemUsed = true;
			}
		}

		//Remove item
		if(itemUsed) {
			removeConsumableItem(index);
			return true;
		}
		else return false;
	}

}
