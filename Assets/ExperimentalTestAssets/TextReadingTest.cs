using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class TextReadingTest : MonoBehaviour {
	public GameObject Controller;
	public Dialogue dialogue;
	FileInfo theSourceFile = null;
	StringReader reader = null;
	string[] dialogueSpeech;
	TextAsset song;

	void Awake() {
		song = (Resources.Load("Dialogue/WrongNumberSong") as TextAsset);
		dialogueSpeech = song.text.Split('\n');
		StartCoroutine (Speak ());
	}

	IEnumerator Speak() {
		dialogue = Controller.GetComponent<Dialogue>();
		for(int i = 0; i < dialogueSpeech.Length; i++) {
			dialogue.Show(dialogueSpeech[i], null, null);
			while(!dialogue.isFinished) yield return null;
		}
	}
}
