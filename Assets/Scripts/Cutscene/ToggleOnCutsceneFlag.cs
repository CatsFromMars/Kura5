using UnityEngine;
using System.Collections;

public class ToggleOnCutsceneFlag : MonoBehaviour {
	public TextAsset cutscene;
	Flags flags;
	public enum option {ENABLE, DISABLE};
	public string swapTagTo;
	public option setGameObjectTo;

	void Awake() {
		GameObject c = GameObject.FindGameObjectWithTag ("GameController");
		flags = c.GetComponent<Flags>();
		if(flags.CheckCutsceneFlag(cutscene.name)) {
			if(setGameObjectTo==option.ENABLE) gameObject.SetActive(true);
			else if(setGameObjectTo==option.DISABLE) gameObject.SetActive(false);
			if(swapTagTo!=null) this.gameObject.tag = swapTagTo;
		}
	}

}
