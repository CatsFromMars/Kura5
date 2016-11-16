using UnityEngine;
using System.Collections;

public class PauseMenu : MenuClass {

	public MenuManager manager;
	private string confirmString = "ARE YOU SURE?";
	public TextMesh promptMesh;
	public TextMesh yes;
	public TextMesh no;
	private bool inConfirmMode = false;
	
	public override void ChooseOption() {
		if(!exiting) {
			if (index == 0) {
				//Resume
				if(inConfirmMode) Application.Quit();
				else ExitMenu();
			}
			else if(index == 1) {
				if(inConfirmMode) unConfirm();
				else swapToConfirm();
			}
		}
	}

	void swapToConfirm() {
		inConfirmMode = true;
		promptMesh.text = confirmString;
		yes.text = "YES";
		no.text = "NO";
	}

	void unConfirm() {
		inConfirmMode = false;
		promptMesh.text = "";
		yes.text = "RESUME";
		no.text = "QUIT GAME";
	}

	public override void ExitMenu() {
		makeSound (deny);
		if(inConfirmMode) unConfirm();
		else manager.closePauseMenu();
	}
}
