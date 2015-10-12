using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {

	public List<Consumable> consumableItems = new List<Consumable>();
	public List<KeyItem> keyItems = new List<KeyItem>();
	public List<Lens> lens = new List<Lens>();

	public void initItems() {
		string n;
		string desc;

		//Consumable Items
		n = "Earth Fruit";
		desc = "Sweet and sour Solar Fruit.\nRestores Some Life.";
		Consumable apple = new Consumable (0, n, desc, "RESTORE_LIFE", "ANNIE", 30);
		apple.model = Resources.Load ("Items/Earth Fruit") as GameObject;
		consumableItems.Add(apple);

		n = "Tomato Juice";
		desc = "Bottle of blood red juice.\nRestores Some Life.";
		Consumable juice = new Consumable (1, n, desc, "RESTORE_LIFE", "EMIL", 30);
		juice.model = Resources.Load ("Items/Tomato Juice") as GameObject;
		consumableItems.Add(juice);

		n = "Blood Orange";
		desc = "Strange fruit that bleeds red ooze.\nRestores Some Energy.";
		Consumable orange = new Consumable (2, n, desc, "RESTORE_ENERGY", "EMIL", 30);
		orange.model = Resources.Load ("Items/Blood Orange") as GameObject;
		consumableItems.Add(orange);

		n = "Solar Fruit";
		desc = "Fruit from a Solar Tree.\nRestores Some Energy.";
		Consumable solarFruit = new Consumable (3, n, desc, "RESTORE_ENERGY", "ANNIE", 30);
		solarFruit.model = Resources.Load ("Items/Solar Fruit") as GameObject;
		consumableItems.Add (solarFruit);

		n = "Tasty Meat";
		desc = "Monster meat grilled to perfection.\nRestores a lot of Life.";
		Consumable meat = new Consumable (4, n, desc, "RESTORE_LIFE", "NONE", 50);
		meat.model = Resources.Load ("Items/Tasty Meat") as GameObject;
		consumableItems.Add (meat);

		//Sort database by itemID
		//consumableItems.Sort();
		//keyItems.Sort();
		//lens.Sort();
	}
}
