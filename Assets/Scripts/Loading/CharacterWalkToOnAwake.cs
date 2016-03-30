using UnityEngine;
using System.Collections;

public class CharacterWalkToOnAwake : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerContainer p = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContainer>();
		StartCoroutine(p.characterWalkTo(this.transform.position));
	}
}
