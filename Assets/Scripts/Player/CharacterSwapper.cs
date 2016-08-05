using UnityEngine;
using System.Collections;

public class CharacterSwapper : MonoBehaviour {
	public ElementSwapping ele;
	GameObject globalData;
	GameData data;
	public Transform annie;
	public Transform emil;
	private Animator annieAnimator;
	private Animator emilAnimator;
	private float delta = 0.5f;
	private bool switching = false;
	public Transform HUD;

	// Use this for initialization
	void Awake() {
		globalData = GameObject.FindGameObjectWithTag("GameController");
		data = globalData.GetComponent<GameData>();
		annieAnimator = annie.GetComponent<Animator>();
		emilAnimator = emil.GetComponent<Animator>();
	}
	void Start() {
		hideInactivePlayer ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Switch")) {
			PlayerContainer player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerContainer>();
			bool inControl = player.playerInControl;
			bool canSwitch = player.currentAnim(player.hash.idleState) && !player.inCoffin && Time.timeScale!=0;
			if(inControl&&canSwitch) switchPlayers();
		}
		if(!switching) updatePosition ();
	}

	void updatePosition() {
		Vector3 pos;
		if (data.currentPlayer == GameData.player.Annie) {
			pos = annie.position;
		}
		else {
			pos = emil.position;
			pos.y -= delta;
		}

		transform.position = pos;
	}

	public void displayBoth() {
		//displays both characters at once
		emil.active = true;
		annie.active = true;
		if(data.currentPlayer == GameData.player.Annie) emil.transform.position = emil.transform.position+(emil.transform.forward*2);
		if(data.currentPlayer == GameData.player.Emil) annie.transform.position = annie.transform.position+(annie.transform.forward*2);
	}

	public void hideInactivePlayer() {
		if(data.currentPlayer == GameData.player.Annie && data.emilCurrentLife > 0) {
			Vector3 spawnPoint = annie.transform.position;
			emil.transform.position = spawnPoint;
			annie.active = true;
			emil.active = false;
		}
		else if(data.currentPlayer == GameData.player.Emil && data.annieCurrentLife > 0) {
			Vector3 spawnPoint = emil.transform.position;
			annie.transform.position = spawnPoint;
			emil.active = true;
			annie.active = false;
		}
	}

	IEnumerator deactivateOnAnimEnd(Animator animator) {
		switching = true;
		HUD.gameObject.SetActive (false);
		Time.timeScale = 0;
		while (animator.GetCurrentAnimatorStateInfo(0).nameHash != (Animator.StringToHash ("Base Layer.Leave")) || animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f || animator.IsInTransition(0)) {
			yield return null;
		}
		animator.gameObject.SetActive(false);
		Time.timeScale = 1;
		switching = false;
		HUD.gameObject.SetActive (true);
	}

	public void switchPlayers() {
		if(data.canSwapToEmil && data.currentPlayer == GameData.player.Annie && data.emilCurrentLife > 0) {
			audio.Play();
			emil.transform.rotation = Quaternion.identity;
			annie.transform.rotation = Quaternion.identity;
			Vector3 spawnPoint = annie.transform.position;
			emil.transform.position = spawnPoint;
			//annie.active = false;
			emil.active = true; //Activate
			annieAnimator.SetTrigger (Animator.StringToHash ("Leave"));
			StartCoroutine(deactivateOnAnimEnd(annieAnimator)); //Deactivate
			data.currentPlayer = GameData.player.Emil;
		}
		else if(data.canSwapToAnnie && data.currentPlayer == GameData.player.Emil && data.annieCurrentLife > 0) {
			audio.Play();
			emil.transform.rotation = Quaternion.identity;
			annie.transform.rotation = Quaternion.identity;
			Vector3 spawnPoint = emil.transform.position;
			annie.transform.position = spawnPoint;
			//emil.active = false;
			annie.active = true; //Activate
			emilAnimator.SetTrigger (Animator.StringToHash ("Leave"));
			StartCoroutine(deactivateOnAnimEnd(emilAnimator)); //Deactivate

			data.currentPlayer = GameData.player.Annie;
		}

		if(ele!=null) ele.updateList();
		
	}
}
