﻿using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

public class DisplayDialogue : MonoBehaviour {
	private static GameObject Controller;
	private static Dialogue dialogue;
	private static string[] dialogueSpeech;
	private static TextAsset text;
	private static Sprite leftSprite;
	private static Sprite rightSprite;

	public static void startCutscene() {
		//Enters cutscene mode
		//Finds player game objects, if they exist
		Animator annieAnimator=null;
		Animator emilAnimator=null;
		GameObject annie = GameObject.Find("AnniePlayer");
		GameObject emil = GameObject.Find("EmilPlayer");
		//Put players' animation controllers in cutscene mode
		if(annie!=null) annieAnimator = annie.GetComponent<Animator>();
		if(emil!=null) emilAnimator = emil.GetComponent<Animator>();
		if(annieAnimator!=null) annieAnimator.SetBool(Animator.StringToHash("CutsceneMode"), true);
		if(emilAnimator!=null) emilAnimator.SetBool(Animator.StringToHash("CutsceneMode"), true);
	}

	public static void endCutscene() {
		//Enters cutscene mode
		//Finds player game objects, if they exist
		Animator annieAnimator=null;
		Animator emilAnimator=null;
		GameObject annie = GameObject.Find("AnniePlayer");
		GameObject emil = GameObject.Find("EmilPlayer");
		//Put players' animation controllers in cutscene mode
		if(annie!=null) annieAnimator = annie.GetComponent<Animator>();
		if(emil!=null) emilAnimator = emil.GetComponent<Animator>();
		if(annieAnimator!=null) annieAnimator.Rebind();
		if(emilAnimator!=null) emilAnimator.Rebind();
	}
	
	public static AudioClip getSound(string txt) {
		string name = getSoundName(txt);
		string filepath = "Voice/"+name;
		AudioClip clip = Resources.Load<AudioClip>(filepath);
		return clip;
	}

	public static string getSoundName(string txt) {
		string s = txt.Replace("S=","");
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		s = rgx.Replace(s, ""); //filter out alphanumeric characters
		return s;
	}

	public static string getSongName(string txt) {
		string s = txt.Replace("M=","");
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		s = rgx.Replace(s, ""); //filter out alphanumeric characters
		return s;
	}
	
	public static Sprite getPortrait(string txt) {
		string name = getName(txt);
		string number = getNumber(txt);
		string filepath = "Portraits/"+name+"/"+number;
		Sprite portrait = Resources.Load<Sprite>(filepath);
		return portrait;
	}

	public static void playAnim(string txt) {
		string number = getNumber(txt);
		string name = txt.Replace("<A=","").Replace("_"+number+">","");
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		name = rgx.Replace(name, ""); //filter out alphanumeric characters
		GameObject go = GameObject.Find (name);
		Animator a;
		if(go!=null) {
			a = go.GetComponent<Animator>();
			a.SetInteger (Animator.StringToHash("CutsceneAction"), int.Parse(number));
		}
		else Debug.LogError("No gameobject called "+name+" found!");
	}

	public static void rotateTowards(string txt) {
		string number = getNumber(txt);
		string name = txt.Replace("<O=","").Replace("_"+number+">","");
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		name = rgx.Replace(name, ""); //filter out alphanumeric characters
		Debug.Log (name);
		GameObject go = GameObject.Find (name);
		GameObject posGo = GameObject.Find ("Rotate" + number);
		Debug.Log (go+" + "+posGo);
		if(go!=null && posGo!=null) {
			Debug.Log("Rotated "+go.name+" towards "+posGo.name+".");
			go.transform.LookAt(posGo.transform);
		}
		else Debug.LogError("No gameobject found!");
	}

	public static void moveTo(string txt) {
		string number = getNumber(txt);
		string name = txt.Replace("<T=","").Replace("_"+number+">","");
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		name = rgx.Replace(name, ""); //filter out alphanumeric characters
		GameObject go = GameObject.Find (name);
		GameObject posGo = GameObject.Find ("Position" + number);
		if(go!=null && posGo!=null) {
			//Debug.Log("Moved "+go.name+" to "+posGo.name+".");
			go.transform.position = posGo.transform.position;
		}
		else Debug.LogError("No gameobject found!");
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
		return name;
	}

	public static void changeScene(string txt) {
		txt = txt.Replace("<GOTO_SCENE=","");
		string name = txt.Replace(">","");
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		name = rgx.Replace(name, ""); //filter out alphanumeric characters
		GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>().gotoScene(name);
	}

	public static GameObject getLookTarget(string txt) {
		txt = txt.Replace("<CAM_GOTO_POINT=","");
		string name = txt.Replace(">","");
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		name = rgx.Replace(name, ""); //filter out alphanumeric characters
		GameObject target = GameObject.Find(name);
		return target;
	}

	public static void changeMusic(string txt) {
		MusicManager music = GameObject.FindGameObjectWithTag ("Music").GetComponent<MusicManager>();
		string name = getSongName(txt);
		string filepath = "Music/"+name;
		Debug.Log (filepath);
		AudioClip clip = Resources.Load<AudioClip>(filepath);
		music.changeMusic(clip);
		music.startMusic();
	}

	public static void toggleRaven() {
		//Intended to spawn and despawn Emil as a raven from Annie's presence
		GameData data = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameData>();
		if (data.currentPlayer == GameData.player.Annie) {
			if(!data.emilRaven.gameObject.activeSelf) data.emilRaven.gameObject.SetActive(true);
			else data.emilRaven.gameObject.GetComponent<Animator>().SetTrigger(Animator.StringToHash("Exit"));
		}
	}

	public static void displayEmoticon(string txt) {
		string name = getName(txt);
		string asset = getNumber(txt);
		GameObject go = GameObject.Find (name);
		if(go!=null) {
			Vector3 offset = new Vector3(0,5,0);
			string filepath = "Emotions/"+asset;
			GameObject emoticon = Resources.Load<GameObject>(filepath);
			Debug.Log(filepath);
			Instantiate(emoticon, go.transform.position+offset+(go.transform.forward*-1), Quaternion.identity);
		}
		else Debug.LogError("No gameobject found!");
	}

	public static void finishDialogue() {
		Controller = GameObject.FindGameObjectWithTag("GameController");
		dialogue = Controller.GetComponent<Dialogue>();
		dialogue.Hide();
		if(dialogue.canvas != null) dialogue.canvas.SetActive (true);
		Time.timeScale = 1;
	}

	public static IEnumerator Speak(TextAsset text, bool isLabel=false) {
		startCutscene ();
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
			//comments. Skip them
			if(dialogueSpeech[index].Contains("##")) {
				nullSpeech = false;
				index++;
				continue;
			}
			//Music!
			if(dialogueSpeech[index].Contains("M=")) {
				nullSpeech = false;
				changeMusic(dialogueSpeech[index]);
				index++;
				continue;
			}
			//Move transform to a numbered marker
			if(dialogueSpeech[index].Contains("<T=")) {
				nullSpeech = false;
				moveTo(dialogueSpeech[index]);
				index++;
				continue;
			}
			//Display Emoticon over head of character
			if(dialogueSpeech[index].Contains("<E=")) {
				nullSpeech = false;
				displayEmoticon(dialogueSpeech[index]);
				index++;
				continue;
			}
			//Play animation
			if(dialogueSpeech[index].Contains("<A=")) {
				nullSpeech = false;
				playAnim(dialogueSpeech[index]);
				index++;
				continue;
			}
			//Rotate Objects
			if(dialogueSpeech[index].Contains("<O=")) {
				nullSpeech = false;
				rotateTowards(dialogueSpeech[index]);
				index++;
				continue;
			}
			//Get Portraits
			if(dialogueSpeech[index].Contains("<L=")) {
				nullSpeech = false;
				leftSprite = getPortrait(dialogueSpeech[index]);
				Debug.Log(leftSprite);
				index++;
				continue;
			}

			if(dialogueSpeech[index].Contains("<R=")) {
				nullSpeech = false;
				rightSprite = getPortrait(dialogueSpeech[index]);
				index++;
				continue;
			}

			//Get Sounds
			if(dialogueSpeech[index].Contains("S=")) {
				dialogue.makeSound(dialogue.tickAudio, getSound(dialogueSpeech[index]));
				index++;
				continue;
			}
			//Camera Look At
			if(dialogueSpeech[index].Contains("<CAM_GOTO_POINT=")) {
				GameObject s = getLookTarget(dialogueSpeech[index]);
				GameObject.FindGameObjectWithTag("CamFollow").GetComponent<CamLooker>().zoomToTarget(s.transform);
				index++;
				continue;
			}

			//Camera Shake
			if(dialogueSpeech[index].Contains("SHAKE_CAMERA")) {
				Camera.main.GetComponent<Animator>().SetTrigger(Animator.StringToHash("Shake"));
				index++;
				continue;
			}

			//Change Scene
			if(dialogueSpeech[index].Contains("<GOTO_SCENE=")) {
				changeScene(dialogueSpeech[index]);
				index++;
				continue;
			}
			
			//Little Bird Buddy Spawning
			if(dialogueSpeech[index].Contains("<TOGGLE_RAVEN>")) {
				toggleRaven();
				index++;
				continue;
			}

			if(index < dialogueSpeech.Length) {
				string speech = dialogueSpeech[index];
				speech = speech.Replace("<n>", "\n");
				Debug.Log(leftSprite);
				dialogue.Show(speech, leftSprite, rightSprite, isLabel);
				rightSprite = null;
				leftSprite = null;
				bool push = Input.GetButtonDown("Charge") || Input.GetButtonDown("Confirm");
				if(!isLabel) while(!dialogue.isFinished) yield return null;
				index++;
				continue;
			}
		}
		//if(!nullSpeech) dialogue.leftAnimator.SetTrigger(Animator.StringToHash("Leave"));
		//if(!nullSpeech) dialogue.rightAnimator.SetTrigger(Animator.StringToHash("Leave"));
		if(!isLabel) {
			if(dialogue.canvas != null) dialogue.canvas.SetActive (true);
			Time.timeScale = 1;
		}
		endCutscene ();
	}
}
