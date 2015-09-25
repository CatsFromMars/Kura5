using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDataBase : MonoBehaviour {

	public List<Item> items = new List<Item>();

	// Use this for initialization
	void Awake () 
	{
		//ITEM DEFINITIONS
		items.Add(new Item(0, "Apple", "A staple for undead slaying heroes. Restores Life. Toxic to Vampires.",Item.ItemType.Consumable,Item.ItemEffect.RestoreSomeLife,Item.element.Sol));
		items.Add(new Item(1, "TomatoJuice", "A blood-red juice to be begrudgingly consumed by a certain Vampire. Restores Life.",Item.ItemType.Consumable,Item.ItemEffect.RestoreSomeLife,Item.element.Dark));
		items.Add(new Item(2, "MagicDrink", "A weird drink made from unknown ingredients. Restores Energy.",Item.ItemType.Consumable,Item.ItemEffect.RestoreSomeEnergy,Item.element.None));

		//KEY ITEMS
		items.Add(new Item(3, "TriangleKey", "It's a key but it's a triangle! Unlocks doors with a 'Δ' lock.",Item.ItemType.Quest,Item.ItemEffect.None,Item.element.None));
		items.Add(new Item(12, "CircleKey", "A plainly shaped key. Unlocks doors with an 'O' lock.",Item.ItemType.Quest,Item.ItemEffect.None,Item.element.None));
		items.Add(new Item(10, "BlueOrb", "A crystal orb colored a deep ocean blue.",Item.ItemType.Quest,Item.ItemEffect.None,Item.element.None));
		items.Add(new Item(11, "GreenOrb", "A crystal orb colored a vibrant leafy green.",Item.ItemType.Quest,Item.ItemEffect.None,Item.element.None));

		//ELEMENTS
		items.Add(new Item(4, "fireElement", "Annie is affiliated with the element of roaring flame. Can be used to light torches.",Item.ItemType.Upgrade,Item.ItemEffect.None,Item.element.Fire));
		items.Add(new Item(5, "solElement", "Annie is affiliated with the element of beaming sunlight. Effective against darkness.",Item.ItemType.Upgrade,Item.ItemEffect.None,Item.element.Sol));
		items.Add(new Item(6, "darkElement", "Emil is affiliated with the element of creeping darkness. Paralyzes in shadow.",Item.ItemType.Upgrade,Item.ItemEffect.None,Item.element.Dark));
		items.Add(new Item(7, "cloudElement", "Emil is affiliated with the element of billowing wind. Breaks apart stone.",Item.ItemType.Upgrade,Item.ItemEffect.None,Item.element.Cloud));
		items.Add(new Item(8, "frostElement", "Emil is affiliated with the element of freezing ice. Freezes objects.",Item.ItemType.Upgrade,Item.ItemEffect.None,Item.element.Frost));
		items.Add(new Item(9, "earthElement", "Annie is affiliated with the element of thundering earth. Causes roots to grow.",Item.ItemType.Upgrade,Item.ItemEffect.None,Item.element.Earth));


		items.Sort();
	}

}
