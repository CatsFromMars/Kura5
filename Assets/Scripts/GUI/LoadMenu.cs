using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class LoadMenu : MenuClass {
	public GameObject newGame;
	public GameObject loadGame;
	public GameObject confirmObj;
	public TextMesh top;
	public TextMesh bottom;
	public GameObject loadText;
	public GameObject annieText;
	public GameObject emilText;
	private bool inConfirmState = false;
	public SceneTransition fader;

	// Use this for initialization
	void Start () {
		if(File.Exists(Application.dataPath + "/save.bok")) {
			loadGame.SetActive(true);
			newGame.SetActive(false);
			//used to display data
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.dataPath + "/save.bok", FileMode.Open);
			GameState displayState = (GameState)bf.Deserialize(file); //Set the game state
			file.Close();
			//Is Annie there?
			if(annieText!=null) annieText.SetActive(displayState.canSwapToAnnie);
			//Is Emil there?
			if(emilText!=null) emilText.SetActive(displayState.canSwapToEmil);
		}
		else {
			newGame.SetActive(true);
			loadGame.SetActive(false);
		}
	}
	
	public override void ChooseOption() {
		if(!exiting) {
			if (index == 0) {
				//Start Game
				if(!inConfirmState) {
					if(File.Exists(Application.dataPath + "/save.bok")) fader.gotoScene("LoadingScene");
					else fader.gotoScene("Intro");
				}
				else {
					ExitMenu();
				}
			}
			else if(index == 1) {
				if(!inConfirmState) {
					inConfirmState = true;
					top.text = "NO";
					bottom.text = "YES";
					loadText.SetActive(false);
					confirmObj.SetActive(true);
					index = 0;
				}
				else if(inConfirmState&&File.Exists(Application.dataPath + "/save.bok")) {
					File.Delete(Application.dataPath + "/save.bok");
					fader.gotoScene("Intro");
				}
			}
		}
	}

	public override void ExitMenu() {
		makeSound(deny);
		if(inConfirmState) {
			inConfirmState = false;
			top.text = "LOAD";
			bottom.text = "NEW GAME";
			loadText.SetActive(true);
			confirmObj.SetActive(false);
		}
		else Application.LoadLevel("MenuScene");
	}
}
