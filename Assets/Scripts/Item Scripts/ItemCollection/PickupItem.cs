using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour {
	public int itemID = 0; //Check database for which item this should be.
	public Transform itemModel;
	private GameObject global;
	private ItemDatabase database;
	private Inventory inventory;
	public Transform container;
	public Animator itemAnimator;
	private bool collected = false;

	void Awake() {
		global = GameObject.FindGameObjectWithTag("GameController");
		database = global.GetComponent<ItemDatabase>();
		inventory = global.GetComponent<Inventory>();

	}

	void spawnInnerItem() {
		Transform item = Instantiate(itemModel, transform.position, Quaternion.identity) as Transform;
		item.transform.position = container.position;
		item.transform.parent = container;
		item.localScale = new Vector3 (2.5f, 2.5f, 2.5f);
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			if(!collected) {
				if(inventory.AddConsumable(itemID) != false) {
					spawnInnerItem ();
					itemAnimator.SetTrigger (Animator.StringToHash("Spawn"));
					collected = true;
				}
			}
		}
	}

}
