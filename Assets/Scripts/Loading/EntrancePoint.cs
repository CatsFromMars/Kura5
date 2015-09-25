using UnityEngine;
using System.Collections;

public class EntrancePoint : MonoBehaviour {
	GameObject player;
	Vector3 startingPos;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		startingPos = new Vector3 (transform.position.x, player.transform.position.y, transform.position.z);
		player.transform.position = startingPos;
		player.transform.rotation = transform.rotation;
	}
}
