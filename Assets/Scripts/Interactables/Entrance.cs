using UnityEngine;
using System.Collections;

public class Entrance : MonoBehaviour {
	public string nextScene;
	private bool canBeTriggered;
	private PlayerContainer player;
	public bool setPlayerPosition=false;
	public Vector3 positionSet;
	public bool setPlayerRotation=false;
	public float rotationSet;
	private MusicManager music;
	public bool swapsMusic=false;
	public AudioClip newMusic;
	public bool deleteKeys=false; //remove all keys from player inventory
	private Inventory inventory;

	void Awake() {
		if(deleteKeys) inventory = GameObject.FindGameObjectWithTag("GameController").GetComponent<Inventory>();
	}
	
	void OnTriggerEnter (Collider other) {
		if(other.tag == "Player") {
			if(deleteKeys) {
				inventory.removeKeyItem(inventory.checkForKeyItem(0));
				inventory.removeKeyItem(inventory.checkForKeyItem(3));
				inventory.removeKeyItem(inventory.checkForKeyItem(4));
			}

			startWalking();
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player") {
			GameObject.FindGameObjectWithTag ("Fader").GetComponent<SceneTransition>().ren.color = Color.clear;
			canBeTriggered = true;
		}
	}

	void startWalking() {
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerContainer> ();
		player.StopAllCoroutines ();
		StartCoroutine (walk());
	}

	IEnumerator walk() {
		Vector3 pos = player.transform.position + player.transform.forward;
		GameObject.FindGameObjectWithTag ("Fader").GetComponent<SceneTransition> ().gotoScene (nextScene);
		//yield return StartCoroutine(player.characterWalkTo(pos, this.transform));
		yield return null;
		if(setPlayerPosition) player.transform.position = positionSet;
		if (setPlayerPosition) {
			player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, rotationSet, player.transform.rotation.z);
		}
		if (swapsMusic) {
			MusicManager music = GameObject.FindGameObjectWithTag ("Music").GetComponent<MusicManager>();
			music.changeMusic(newMusic);
			music.startMusic();
		}
	}
}
