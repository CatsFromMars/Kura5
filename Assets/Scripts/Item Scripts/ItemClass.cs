using UnityEngine;
using System.Collections;
using System;

public class Item : IComparable<Item> {

	public int idNumber;
	public string name;
	public string description;
	public string effectName;
	public Texture2D icon;
	public ItemType type;
	public element elem;

	public enum ItemType {
		Consumable,
		Quest,
		Upgrade,
	}

	public ItemEffect effect;

	public enum element {
		Sol,
		Dark,
		Fire,
		Cloud,
		Frost,
		Earth,
		None,
	}

	public enum ItemEffect {
		None,
		RestoreSomeLife,
		RestoreSomeEnergy,
	}

	//ITEM CLASS
	public Item(int itemID, string itemName, string itemDescription, ItemType itemType, ItemEffect itemEffect, element itemElem)
	{
		idNumber = itemID;
		name = itemName;
		description = itemDescription;
		type = itemType;
		effect = itemEffect;
		elem = itemElem;
		icon = Resources.Load<Texture2D>("Item Icons/" + name);
	}

	public int CompareTo(Item other)
	{
		if(other == null)
		{
			return 1;

		}

		//SORTS BY ID NUMBER
		return idNumber - other.idNumber;

	}

	public Item()
	{
		icon = Resources.Load<Texture2D>("Item Icons/blank");
	}

}
