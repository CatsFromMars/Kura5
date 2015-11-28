using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class DialogueDisplay : MonoBehaviour {
	private static GameObject Controller;
	private static Dialogue dialogue;
	private static string[] dialogueSpeech;
	private static TextAsset text;
	private static DialogueDisplay instance;
	
	public static IEnumerator DisplaySpeech(string fileName) {
		Time.timeScale = 0; //Pause
		//Data get
		Controller = GameObject.FindGameObjectWithTag("GameController");
		text = (Resources.Load ("Dialogue/" + fileName) as TextAsset);
		dialogueSpeech = text.text.Split('\n');

		dialogue = Controller.GetComponent<Dialogue>();
		for(int i = 0; i < dialogueSpeech.Length; i++) {
			string speech = dialogueSpeech[i];
			int j = speech.IndexOf(":");
			if(j != -1) speech = speech.Insert(j+2, "\n");
			dialogue.Show(speech, null, null);
			while(!dialogue.isFinished) yield return null;
		}
		Time.timeScale = 1;
	}
}
