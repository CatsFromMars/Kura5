using UnityEngine;
using System.Collections;

public class CharacterExclude : MonoBehaviour {
	public bool blockEmil=true; //set to false blocks to annie in the future. Or add an enum.
	GameData data;
	// Use this for initialization
	void OnEnable() {
		data = GetUtil.getData();
		if(data!=null&&blockEmil&&data.currentPlayer==GameData.player.Emil) {
			Debug.Log("Swapping to Annie...");
			SceneTransition transition = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>();
			transition.swapper.forceSwitchToAnnie();
		}
		data.canSwapToEmil=false;
	}
	
	void OnDestroy() {
		data.canSwapToEmil=true;
	}
}
