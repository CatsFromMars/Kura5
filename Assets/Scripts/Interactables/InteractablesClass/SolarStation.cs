using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SolarStation : MonoBehaviour {
	public int points = 500;
	private GameObject playerObj;
	public float range = 5f;
	private bool charging = false;
	public Text text;
	private GameData data;
	public GameObject glow;
	public AudioClip sunCharge;
	public AudioClip darkCharge;
	private AudioSource audio;

	// Use this for initialization
	void Start () {
		playerObj = GetUtil.getPlayer();
		data = GetUtil.getData ();
		text.text = points.ToString();
		glow.SetActive (false);
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!charging && Input.GetButtonDown("Charge") && Vector3.Distance(transform.position,playerObj.transform.position)<range) {
			charging = true;
			StartCoroutine(StartCharging());
		}
		points = data.bankSoll;
		text.text = points.ToString();
	}

	IEnumerator StartCharging() {
		Debug.Log ("Activated!");
		if(data.currentPlayer==GameData.player.Annie) audio.clip = sunCharge;
		else if(data.currentPlayer==GameData.player.Emil) audio.clip = darkCharge;
		PlayerContainer player;
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerContainer> ();
		Vector3 pos = transform.position + this.transform.forward*3f;
		yield return StartCoroutine(player.characterWalkTo(pos, this.transform));
		Animator animator = player.transform.GetComponent<Animator>();
		animator.SetBool(Animator.StringToHash("Station"),true);
		audio.Play();
		player.playerInControl = false;
		while(Input.GetButton("Charge")&&points>0) {
			glow.SetActive(!glow.activeSelf);
			if(data.currentPlayer==GameData.player.Annie) {
				if(data.annieCurrentEnergy < data.annieMaxEnergy) data.annieCurrentEnergy++;
				else break;
			}
			else if(data.currentPlayer==GameData.player.Emil) {
				if(data.emilCurrentEnergy < data.emilMaxEnergy) data.emilCurrentEnergy++;
				else break;
			}
			data.bankSoll--;
			yield return new WaitForSeconds(0.01f);
			yield return null;
		}
		animator.SetBool(Animator.StringToHash("Station"),false);
		player.playerInControl = true;
		charging = false;
		glow.SetActive (false);
		audio.Stop();
	}
}
