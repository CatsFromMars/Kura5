using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuTutorial : MonoBehaviour {

	private static string[] dialogueSpeech;
	public TextAsset text;
	public int interval = 5;
	Text uitext;
	public int index = 0;
	public GameObject beat;
	public Animator otenkoAnimator;
	public SceneTransition fader;
	bool transition = false;
	bool changingScene = false;
	public Animator a;
	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		dialogueSpeech = text.text.Split('\n');
		uitext = GetComponent<Text> ();
		uitext.text = dialogueSpeech [0];
		StartCoroutine(tutorial());
	}

	void updateAnims() {
		if (index == 0) {
			otenkoAnimator.SetBool(Animator.StringToHash("Appeared"), true);
		}
		if(index == 5){
			otenkoAnimator.SetTrigger(Animator.StringToHash("Wink"));
			beat.SetActive(true);
		}
		else beat.SetActive(false);

		if (index == 7) {
			otenkoAnimator.SetTrigger(Animator.StringToHash("Wink"));
		}
	}

	void Update() {
		if(Input.anyKey) Application.LoadLevel("MenuScene");
	}
	
	// Update is called once per frame
	IEnumerator tutorial() {
		foreach (string s in dialogueSpeech) {
			updateAnims();
			uitext.text = dialogueSpeech [index];
			a.SetTrigger(Animator.StringToHash("Next"));
			yield return new WaitForSeconds(interval);
			index++;
		}
		Application.LoadLevel("MenuScene");
	}
}
