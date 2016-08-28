using UnityEngine;
using System.Collections;

public class OutskirtsIntro : MonoBehaviour {
	private PlayerContainer player;
	public TextAsset introCutscene;
	public Transform walkTo1;
	private Flags flags;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContainer>();
		GameObject c = GameObject.FindGameObjectWithTag ("GameController");
		flags = c.GetComponent<Flags>();
		flags.AddCutsceneFlag(introCutscene.name);
		StartCoroutine (startCutscene());
	}
	
	IEnumerator startCutscene() {
		if(!flags.CheckCutsceneFlag(introCutscene.name)) {
			//be sure to disable trap trigger in-scene.
			IEnumerator c = player.characterWalkTo(walkTo1.position);
			StartCoroutine(c);
			yield return new WaitForSeconds(1.1f);
			StopCoroutine (c);
			player.playerInControl = true;
			yield return StartCoroutine(DisplayDialogue.Speak(introCutscene));
			flags.SetCutscene(introCutscene.name);
		}
	}
}
