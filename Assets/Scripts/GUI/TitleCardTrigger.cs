using UnityEngine;
using System.Collections;

public class TitleCardTrigger : MonoBehaviour {
	public string title;
	public string status;

	// Triggers title card
	void Start () {
		Flags flags = GetUtil.getFlags();
		flags.AddCutsceneFlag(title+status);

		if(!flags.CheckCutsceneFlag(title+status)) {
			TitleCard card = GameObject.Find("TitleCard").GetComponent<TitleCard>();
			card.playTitleCard (title,status);
			flags.SetCutscene(title+status);
		}
	}

}
