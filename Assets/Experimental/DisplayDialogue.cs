using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class DisplayDialogue : MonoBehaviour {
	public GameObject Controller;
	private Dialogue dialogue;
	string[] dialogueSpeech;
	public TextAsset text;
	public Texture2D testPortrait;

	void Awake() {
		dialogueSpeech = text.text.Split('\n');
		StartCoroutine (Speak ());
	}
	
	IEnumerator Speak() {
		Time.timeScale = 0; //Pause
		dialogue = Controller.GetComponent<Dialogue>();
		for(int i = 0; i < dialogueSpeech.Length; i++) {
			string speech = dialogueSpeech[i];
			int j = speech.IndexOf(":");
			if(j != -1) speech = speech.Insert(j+2, "\n");
			dialogue.Show(speech, testPortrait, null);
			bool push = Input.GetButtonDown("Charge") || Input.GetButtonDown("Confirm");
			while(!dialogue.isFinished && !push) yield return null;
		}
		Time.timeScale = 1;
	}
}
