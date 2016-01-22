using UnityEngine;
using System.Collections;

public class SolarStation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StartCharging() {
		PlayerContainer player;
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerContainer> ();
		Vector3 pos = transform.position + this.transform.forward*2f;
		StartCoroutine(player.characterWalkTo(pos, this.transform));
	}
}
