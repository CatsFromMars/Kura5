using UnityEngine;
using System.Collections;

public class CharacterSwapper : MonoBehaviour {
	public ElementSwapping ele;
	GameObject globalData;
	GameData data;
	public Transform annie;
	public Transform emil;
	public Animator annieAnimator;
	public Animator emilAnimator;
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
		if(Input.GetButtonDown("Switch")&&Time.timeScale!=0) {
			PlayerContainer player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerContainer>();
			bool inControl = player.playerInControl;
			bool canSwitch = player.currentAnim(player.hash.idleState) && !player.inCoffin && Time.timeScale!=0;
			if(inControl&&canSwitch) switchPlayers();
		}
		if(!switching) updatePosition ();
	}

	public void forceSwitchToAnnie() {
		if(data.currentPlayer == GameData.player.Emil) {
			if(data.annieCurrentLife <= 0) {
				GameOverHandler go = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameOverHandler>();
				data.annieCurrentLife = data.annieMaxLife/2;
				go.annie.revive();
			}
			emil.transform.rotation = Quaternion.identity;
			annie.transform.rotation = Quaternion.identity;
			Vector3 spawnPoint = emil.transform.position;
			annie.transform.position = spawnPoint;
			annie.gameObject.SetActive (true);
			emil.gameObject.SetActive (false);
			data.currentPlayer = GameData.player.Annie;
		}
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
		emilAnimator.SetBool(Animator.StringToHash("CutsceneMode"), true);
		annieAnimator.SetBool(Animator.StringToHash("CutsceneMode"), true);
	}

	public void hideInactivePlayer() {
		if(data.currentPlayer == GameData.player.Annie) {
			Vector3 spawnPoint = annie.transform.position;
			emil.transform.position = spawnPoint;
			annie.active = true;
			emil.active = false;
			annie.GetComponent<PlayerContainer>().zoomToPlayer();
		}
		else if(data.currentPlayer == GameData.player.Emil) {
			Vector3 spawnPoint = emil.transform.position;
			annie.transform.position = spawnPoint;
			emil.active = true;
			annie.active = false;
			emil.GetComponent<PlayerContainer>().zoomToPlayer();
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

	public void switchPlayers(bool blink=false) {
		if(data.canSwapToEmil && data.currentPlayer == GameData.player.Annie && data.emilCurrentLife > 0) {
			audio.Play();
			emil.transform.rotation = Quaternion.identity;
			annie.transform.rotation = Quaternion.identity;
			Vector3 spawnPoint = annie.transform.position;
			emil.transform.position = spawnPoint;
			//annie.active = false;
			emil.active = true; //Activate
			emilAnimator.SetTrigger (Animator.StringToHash ("Switch"));
			annieAnimator.SetTrigger (Animator.StringToHash ("Leave"));
			StartCoroutine(deactivateOnAnimEnd(annieAnimator)); //Deactivate
			if(blink) emil.GetComponent<PlayerContainer>().startInvincibilityFrames();
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
			annieAnimator.SetTrigger (Animator.StringToHash ("Switch"));
			emilAnimator.SetTrigger (Animator.StringToHash ("Leave"));
			StartCoroutine(deactivateOnAnimEnd(emilAnimator)); //Deactivate
			if(blink) annie.GetComponent<PlayerContainer>().startInvincibilityFrames();
			data.currentPlayer = GameData.player.Annie;
		}

		if(ele!=null) ele.updateList();
		
	}
}
