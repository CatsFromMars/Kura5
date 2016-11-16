using UnityEngine;
using System.Collections;

public class EndTransition : MonoBehaviour {
	private PlayerContainer player;
	public Transform walkTo1;


	void OnTriggerEnter (Collider other) {
		if(other.tag == "Player") {
			player = other.GetComponent<PlayerContainer>();
			StartCoroutine(startCutscene());
		}
	}

	IEnumerator startCutscene() {
		GameObject.Find ("HUD").SetActive (false);
		IEnumerator c = player.characterWalkTo(walkTo1.position);
		StartCoroutine(c);
		yield return new WaitForSeconds(1f);
		GameObject.Find("CutsceneFade").GetComponent<Animator>().SetInteger(Animator.StringToHash("CutsceneAction"),1);
		yield return new WaitForSeconds(2f);
		StopCoroutine(c);
		player.gameObject.SetActive (false);
		while(AudioListener.volume > 0) {
			AudioListener.volume -= 3*Time.unscaledDeltaTime;
			yield return null;
		}
		Application.LoadLevel ("ViolinistInBetween");
	}
}
