using UnityEngine;
using System.Collections;

public class ToggleOnCutsceneFlag : MonoBehaviour {
	public TextAsset cutscene;
	Flags flags;
	public enum option {ENABLE, DISABLE};
	public string swapTagTo;
	public option setGameObjectTo;
	public GameObject[] gameObjects;

	void Awake() {
		GameObject c = GameObject.FindGameObjectWithTag ("GameController");
		flags = c.GetComponent<Flags>();
		if(flags.CheckCutsceneFlag(cutscene.name)) {
			if(setGameObjectTo==option.ENABLE) toggleObjects(true);
			else if(setGameObjectTo==option.DISABLE) toggleObjects(false);
			if(swapTagTo!=null) this.gameObject.tag = swapTagTo;
		}
	}

	void toggleObjects(bool isActive) {
		if(gameObjects.Length==0) gameObject.SetActive(isActive);
		else {
			foreach(GameObject go in gameObjects) {
				go.SetActive(isActive);
			}
		}
	}

}
