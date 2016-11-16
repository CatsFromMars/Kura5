using UnityEngine;
using System.Collections;

public class TitleMenu : MenuClass {
	public SceneTransition fader;

	public override void ChooseOption() {
		if(!exiting) {
			if (index == 0) {
				//PlayGame
				fader.gotoScene("LoadGame");
			}
			else if(index == 1) {
				//Options
				fader.gotoScene("Options");
			}
			else if(index == 2) {
				//Quit
				Application.Quit();
			}
		}
	}
}
