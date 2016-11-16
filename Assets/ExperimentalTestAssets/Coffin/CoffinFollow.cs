using UnityEngine;
using System.Collections;

public class CoffinFollow : MonoBehaviour {
	public Transform player;
	private NavMeshAgent agent;
	GameObject gameData;
	GameData data;
	HashIDs hash;
	private LightLevels lightLevels;
	private bool inRange = false;
	private Animator playerAnimator;
	public Transform ropeEndpoint;
	public RopeScript rope;
	public GameObject snowTrail;
	public ParticleSystem dust;
	private bool snowing;
	private SkylightWeather s;
	public AudioClip chainNoise;
	public AudioClip dragNoise;

	void Awake() {
		gameData = GameObject.FindGameObjectWithTag("GameController");
		agent = GetComponent<NavMeshAgent>();
		data = gameData.GetComponent<GameData>();
		hash = gameData.GetComponent<HashIDs>();
		lightLevels = GameObject.FindGameObjectWithTag("LightLevels").GetComponent<LightLevels>();
		GameObject wb = GameObject.Find ("Weatherbox");
		if(wb!=null) s = wb.GetComponent<SkylightWeather>();
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Player" && !inRange) {
			inRange = true;
			other.GetComponent<PlayerContainer>().nearCoffin = true;
		}
		
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Player" && inRange) {
			inRange = false;
		}
	}

	void Update () {
		if(s!=null) snowing = s.snowActive;

		if (Input.GetButtonDown ("Charge") && inRange) {
			data.nearInteractable = true;
			player = GameObject.FindGameObjectWithTag("Player").transform;
			playerAnimator = player.GetComponent<Animator>();
			transform.parent = gameData.transform; //So that it doesn't get destroyed when a level loads
			Transform hand = getLeftHand();
			ropeEndpoint.position = hand.position;
			rope.BuildRope();
			ropeEndpoint.parent = hand.transform;

			audio.clip = chainNoise;
			audio.Play();
		}

		if(Input.GetButtonUp("Charge")) {
			transform.parent = null;
			if(!inRange && player!=null) player.GetComponent<PlayerContainer>().nearCoffin = false;
			data.nearInteractable = false;
			ropeEndpoint.parent = rope.transform;
			rope.DestroyRope();
		}

		if(Input.GetButton("Charge") && playerAnimator!=null) {
			if(playerAnimator.GetCurrentAnimatorStateInfo(0).nameHash == hash.pullingState) {
				agent.SetDestination(player.position);
				agent.Resume();
				agent.updateRotation = true;

				if(agent.velocity.sqrMagnitude > 2 && !audio.isPlaying) {
					audio.clip = dragNoise;
					audio.Play();
					if(snowing) {
						snowTrail.SetActive(true);
					}
					else {
						snowTrail.SetActive(false);
						if(!dust.isPlaying) dust.Play();
					}
				}
			}
			else {
				agent.velocity = Vector3.zero;
				agent.Stop();
				agent.updateRotation = false;

			}
		}
	}

	Transform getLeftHand() {
		Transform[] allChildren = player.GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			if(child.name.Contains("CoffinChain")) return child;
		}
		return null;
	}
}
