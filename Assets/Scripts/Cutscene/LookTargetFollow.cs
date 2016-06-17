using UnityEngine;
using System.Collections;

public class LookTargetFollow : MonoBehaviour {
	public string GO;
	private GameObject follow;

	void Start() {
		follow = GameObject.Find (GO);
	}

	// Update is called once per frame
	void Update () {
		if(follow!=null) this.transform.position = follow.transform.position;
	}
}
