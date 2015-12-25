using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class DisplayDialogue : MonoBehaviour {
	private static GameObject Controller;
	private static Dialogue dialogue;
	private static string[] dialogueSpeech;
	private static TextAsset text;
	private static Sprite leftSprite;
	private static Sprite rightSprite;

	public static Sprite getPortrait(string txt) {
		string name = getName(txt);
		string number = getNumber(txt);
		string filepath = "Portraits/"+name+"/"+number;
		Debug.Log (filepath);
		Sprite portrait = Resources.Load<Sprite>(filepath);
		Debug.Log (portrait.name);
		return portrait;
	}

	public static string getNumber(string txt) {
		int length = txt.Length;
		char[] text = txt.ToCharArray ();
		string v = "";
		foreach(char c in text) {
			if(char.IsDigit(c)) {
				v = c.ToString();
				break;
			}
		}
		Debug.Log (v);
		return v;
	}
	
	public static string getName(string txt){
		int length = 0;
		txt = txt.Substring(2);
		char[] text = txt.ToCharArray ();
		foreach (char c in text) {
			if(c.Equals("_")) break;
			else length++;
		}
		string name = txt.Substring(1, length-5);
		Debug.Log (name);
		return name;
	}
	
	public static IEnumerator Speak(TextAsset text) {
		bool nullSpeech = true; //Is this a segment that involves NO portraits?
		Time.timeScale = 0; //Pause
		//Data get 
		Controller = GameObject.FindGameObjectWithTag("GameController");
		//Split up the dialogue
		dialogueSpeech = text.text.Split('\n');
		dialogue = Controller.GetComponent<Dialogue>();
		//Disable GUI
		if(dialogue.canvas != null) dialogue.canvas.SetActive(false);
		int index = 0;
		while(index < dialogueSpeech.Length) {
			string speech = dialogueSpeech[index];

			//Get Portraits
			if(speech.Contains("<L=")) {
				nullSpeech = false;
				leftSprite = getPortrait(speech);
				index++;
			}
			else leftSprite = null;

			if(speech.Contains("<R=")) {
				nullSpeech = false;
				rightSprite = getPortrait(speech);
				index++;
			}
			else rightSprite = null;

			speech = dialogueSpeech[index];
			speech = speech.Replace("<n>", "\n");
			dialogue.Show(speech, leftSprite, rightSprite);
			bool push = Input.GetButtonDown("Charge") || Input.GetButtonDown("Confirm");
			while(!dialogue.isFinished) yield return null;
			index++;
		}
		//if(!nullSpeech) dialogue.leftAnimator.SetTrigger(Animator.StringToHash("Leave"));
		//if(!nullSpeech) dialogue.rightAnimator.SetTrigger(Animator.StringToHash("Leave"));
		if(dialogue.canvas != null) dialogue.canvas.SetActive (true);
		Time.timeScale = 1;
	}
}
