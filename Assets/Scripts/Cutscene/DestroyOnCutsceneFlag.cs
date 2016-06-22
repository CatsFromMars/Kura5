using UnityEngine;
using System.Collections;

public class DestroyOnCutsceneFlag : MonoBehaviour {
	public TextAsset cutscene;
	Flags flags;
	private bool isDestroyed = false;
	// Use this for initialization

	void Awake() {
		GameObject c = GameObject.FindGameObjectWithTag ("GameController");
		flags = c.GetComponent<Flags>();
	}

	void Update() {
		if(Time.timeScale > 0 && !isDestroyed) {
			if(flags.CheckCutsceneFlag(cutscene.name)) {
				isDestroyed = true;
				Destroy(this.gameObject);
			}
		}
	}
}
