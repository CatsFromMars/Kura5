using UnityEngine;
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
		GameObject[] emotions = GameObject.FindGameObjectsWithTag ("Emotion");
		if(emotions!=null) {
			foreach(GameObject e in emotions) Destroy(e);
		}
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

	public static void camZoomTo(string txt) {
		txt = txt.Replace("<CAM_ZOOM_TO=","");
		string no = txt.Replace(">","");
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		no = rgx.Replace(no, ""); //filter out alphanumeric characters
		Debug.Log (no);
		int z = int.Parse(no);
		Camera.main.GetComponent<CamZoomer>().zoomTo(z);
	}

	public static void disableGO(string txt) {
		txt = txt.Replace("DISABLE_GO=","");
		string name = txt.Replace(">","");
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		name = rgx.Replace(name, ""); //filter out alphanumeric characters
		GameObject target = GameObject.Find(name);
		target.SetActive(false);
	}

	public static GameObject getLookTarget(string txt) {
		txt = txt.Replace("<CAM_GOTO_POINT=","");
		string name = txt.Replace(">","");
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		name = rgx.Replace(name, ""); //filter out alphanumeric characters
		GameObject target = GameObject.Find(name);
		return target;
	}

	public static int getWaitTime(string txt) {
		txt = txt.Replace("<PAUSE_FOR=","");
		string no = txt.Replace(">","");
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		no = rgx.Replace(no, ""); //filter out alphanumeric characters
		Debug.Log (no);
		int z = int.Parse(no);
		return z;
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
			data.canSwapToEmil=!data.emilRaven.gameObject.activeSelf;
		}
	}

	public static void showBothCharacters() {
		SceneTransition transition = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>();
		transition.showBothPlayers();
	}

	public static void hideInactiveCharacter() {
		SceneTransition transition = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>();
		transition.swapper.hideInactivePlayer();
	}

	public static void toggleCharacter(string txt) {
		//Really dumb but we're only checking two characters here
		GameData data = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameData>();
		if (txt.Contains ("EMIL")) {
			if(txt.Contains("FALSE")) data.canSwapToEmil = false;
			else data.canSwapToEmil = true;
		}
		else if (txt.Contains ("ANNIE")) {
			if(txt.Contains("FALSE")) data.canSwapToAnnie = false;
			else data.canSwapToAnnie = true;
		}
	}

	public static void displayEmoticon(string txt) {
		string name = getName(txt);
		string asset = getNumber(txt);
		GameObject go = GameObject.Find (name);
		if(go!=null) {
			Vector3 offset = new Vector3(0,3,0);
			string filepath = "Emotions/"+asset;
			GameObject emoticon = Resources.Load<GameObject>(filepath);
			GameObject emoticonInstance = (Instantiate(emoticon, go.transform.position+offset+(go.transform.forward*-1), Quaternion.identity)) as GameObject;
			emoticonInstance.transform.parent = go.transform;
			emoticonInstance.name = "Emoticon";
		}
		else Debug.LogError("No gameobject found!");
	}

	public static void clearEmoticon(string txt) {
		txt = txt.Replace("<CLEAR_EMOJI=","");
		string name = txt.Replace(">","");
		Regex rgx = new Regex("[^a-zA-Z0-9 -]");
		name = rgx.Replace(name, ""); //filter out alphanumeric characters
		GameObject go = GameObject.Find(name);
		if(go!=null) {
			Transform emotion = go.transform.Find("Emoticon");
			if(emotion!=null) Destroy(emotion.gameObject);
		}
		else Debug.LogError("No gameobject found!");
	}

	public static void finishDialogue(bool stopTime=false) {
		Controller = GameObject.FindGameObjectWithTag("GameController");
		dialogue = Controller.GetComponent<Dialogue>();
		dialogue.Hide();
		if(dialogue.canvas != null) dialogue.canvas.SetActive (true);
		if(!stopTime) Time.timeScale = 1;
	}

	public static IEnumerator Speak(TextAsset text, bool isLabel=false, bool canSkip=true) {
		if(GameObject.Find("DialogueBox") != null) DisplayDialogue.finishDialogue(); //only one instance of dialogue can be displayed at a time
		startCutscene();
		bool skip = false;
		MusicManager music = null;
		if(canSkip) music = GameObject.FindGameObjectWithTag ("Music").GetComponent<MusicManager>();

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
			//Disable GameObject
			if(dialogueSpeech[index].Contains("<DISABLE_GO=")) {
				nullSpeech = false;
				disableGO(dialogueSpeech[index]);
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
			//Clear Emoticon
			if(dialogueSpeech[index].Contains("<CLEAR_EMOJI=")) {
				nullSpeech = false;
				clearEmoticon(dialogueSpeech[index]);
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
			//Camera zoom
			if(dialogueSpeech[index].Contains("<CAM_ZOOM_TO=")) {
				camZoomTo(dialogueSpeech[index]);
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
			if(dialogueSpeech[index].Contains("TOGGLE_CHARACTER")) {
				toggleCharacter(dialogueSpeech[index]);
				index++;
				continue;
			}

			if(dialogueSpeech[index].Contains("SHOW_BOTH_CHARACTERS")) {
				showBothCharacters();
				index++;
				continue;
			}

			if(dialogueSpeech[index].Contains("HIDE_INACTIVE_CHARACTER")) {
				hideInactiveCharacter();
				index++;
				continue;
			}

			if(dialogueSpeech[index].Contains("<PAUSE_FOR=") &&!skip) {
				int time = getWaitTime(dialogueSpeech[index]);
				float counter = 0;
				while (counter<time) {
					counter += Time.unscaledDeltaTime;
					yield return null;
				}
				index++;
				continue;
			}

			Animator fader = null;
			if(index < dialogueSpeech.Length) {
				string speech = dialogueSpeech[index];
				//Debug.Log("Displaying Dialogue...");
				speech = speech.Replace("<n>", "\n");
				//Debug.Log(leftSprite);
				dialogue.Show(speech, leftSprite, rightSprite, isLabel);
				rightSprite = null;
				leftSprite = null;
				bool push = Input.GetButtonDown("Charge") || Input.GetButtonDown("Confirm");
				if(!isLabel&&!skip) while(!dialogue.isFinished) {
					//Skip the cutscene here
					if(Input.GetButtonDown("Inventory")&&canSkip) {
						skip=true;
						//dialogue.Hide();
						break;
						//Debug.Log("Attempting Skip");
					}
					yield return null;
				}
				else if(isLabel) while(!dialogue.isFinished) yield return null;
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
		Debug.Log ("We're done here folks");
		dialogue.Hide();
		endCutscene();
	}
}
