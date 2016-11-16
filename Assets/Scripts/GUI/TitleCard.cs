using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleCard : MonoBehaviour {
	public Text title;
	public Text status;
	public Animator animator;

	public void playTitleCard(string t, string s) {
		title.text = t;
		status.text = s;
		StartCoroutine (play());
	}

	IEnumerator play() {
		while(Time.timeScale == 0) yield return null;
		animator.SetTrigger(Animator.StringToHash("Show"));
	}
}
