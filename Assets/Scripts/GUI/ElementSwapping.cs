using UnityEngine;
using System.Collections;

public class ElementSwapping : MonoBehaviour {

	public Inventory inventory;
	public GameData data;
	public SpriteRenderer sprite;
	public Sprite sol;
	public Sprite flame;
	public Sprite dark;

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("ToggleLens")) toggleLens();
		updateDisplay ();
	}

	void updateDisplay() {
		if (data.currentPlayer == GameData.player.Annie) {
			if(data.annieCurrentElem == GameData.elementalProperty.Sol) sprite.sprite = sol;
			if(data.annieCurrentElem == GameData.elementalProperty.Fire) sprite.sprite = flame;
		}
		else if (data.currentPlayer == GameData.player.Emil) {
			sprite.sprite = dark;
		}
	}

	void toggleLens() {
		if (data.currentPlayer == GameData.player.Annie) {
			int fire = inventory.checkForLens(2);
			int sol = inventory.checkForLens(0);
			if(data.annieCurrentElem == GameData.elementalProperty.Sol && fire!=-1) inventory.EquipLens(fire);
			else inventory.EquipLens(sol);

		}
	}
}
