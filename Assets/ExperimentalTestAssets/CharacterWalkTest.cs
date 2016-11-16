using UnityEngine;
using System.Collections;

public class CharacterWalkTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerContainer player;
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerContainer> ();
		//StartCoroutine(player.characterWalkTo (this.transform.position));
	}
}
