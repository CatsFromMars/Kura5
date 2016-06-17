using UnityEngine;
using System.Collections;

public class DestroyOnCutsceneFlag : MonoBehaviour {
	public TextAsset cutscene;
	Flags flags;
	// Use this for initialization
	void Start() {
		GameObject c = GameObject.FindGameObjectWithTag ("GameController");
		flags = c.GetComponent<Flags>();
		if(flags.CheckCutsceneFlag(cutscene.name)) Destroy(this.gameObject);
	}
}
