using UnityEngine;
using System.Collections;

public class DialogueAnimationEvent : MonoBehaviour {

	void Speak(TextAsset text) {
		StartCoroutine (SpeakCoroutine(text));
	}
	
	IEnumerator SpeakCoroutine(TextAsset text) {
		yield return StartCoroutine(DisplayDialogue.Speak(text));
		Broadcaster.BroadcastAll("OnDialogueEnd");
	}
}
