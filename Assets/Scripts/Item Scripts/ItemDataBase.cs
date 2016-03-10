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

		//Key Items
		n = "Key";
		desc = "A key held together by the\ndark magic of an Immortal.";
		KeyItem key = new KeyItem (0, n, desc, false, "UNLOCKS_DOORS");
		key.model = Resources.Load ("Items/Key") as GameObject;
		keyItems.Add(key);

		//Lens
		n = "Sol Lens";
		desc = "A [SOLAR GUN LENS] containing the Sol Property";
		Lens sol = new Lens (0, n, desc, "ANNIE", "Sol");
		lens.Add(sol);

		n = "Dark Lens";
		desc = "A [DARK SWORD LENS] containing the Dark Property";
		Lens dark = new Lens (1, n, desc, "EMIL", "Dark");
		lens.Add(dark);

		n = "Fire Lens";
		desc = "A [SOLAR GUN LENS] containing the Fire Property";
		Lens fire = new Lens (2, n, desc, "ANNIE", "Fire");
		lens.Add (fire);

		n = "Frost Lens";
		desc = "A [DARK SWORD LENS] containing the Frost Property";
		Lens frost = new Lens (3, n, desc, "EMIL", "Frost");
		lens.Add (frost);

		n = "Earth Lens";
		desc = "A [SOLAR GUN LENS] containing the Earth Property";
		Lens earth = new Lens (4, n, desc, "ANNIE", "Earth");
		lens.Add (earth);

		n = "Cloud Lens";
		desc = "A [DARK SWORD LENS] containing the Cloud Property";
		Lens cloud = new Lens (5, n, desc, "EMIL", "Cloud");
		lens.Add (cloud);

		n = "Luna Lens";
		desc = "AN [ALL PURPOSE LENS] containing the Luna Property. Uses no energy, but...";
		Lens luna = new Lens (6, n, desc, "ANNIE", "Luna");
		lens.Add (cloud);

		n = "Astro Lens";
		desc = "A [ALL PURPOSELENS] containing the Cloud Property";
		Lens astro = new Lens (7, n, desc, "EMIL", "Astro");
		lens.Add (astro);

		n = "Empty Lens";
		desc = "A [DARK SWORD LENS] that contains no Property. Uses no energy.";
		Lens empty = new Lens (8, n, desc, "EMIL", "Null");
		lens.Add (empty);


		//Sort database by itemID
		//consumableItems.Sort();
		//keyItems.Sort();
		//lens.Sort();
	}
}
