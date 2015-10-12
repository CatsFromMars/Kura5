using UnityEngine;
using System.Collections;
using System;

public class Item : IComparable<Item> {

	public int id;
	public string name;
	public string description;
	public GameObject model;

	public enum ItemEffect {
		None,
		RestoreSomeLife,
		RestoreSomeEnergy,
	}

	//ITEM CLASS
	public Item(int itemID, string itemName, string itemDescription)
	{
		id = itemID;
		name = itemName;
		description = itemDescription;
		model = Resources.Load ("Items/"+itemName) as GameObject;
	}

	public int CompareTo(Item other)
	{
		if(other == null)
		{
			return 1;

		}

		//SORTS BY ID NUMBER
		return id - other.id;

	}

	public Item() {
		model = null;
	}

}
