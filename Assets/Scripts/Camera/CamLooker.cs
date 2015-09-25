using UnityEngine;
using System.Collections;

public class CamLooker : MonoBehaviour {
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(player == null) player = GameObject.FindGameObjectWithTag ("Player");
		transform.position = player.transform.position;
	}
}
