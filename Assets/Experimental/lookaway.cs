using UnityEngine;
using System.Collections;

public class lookaway : MonoBehaviour {
	Transform target;
	// Use this for initialization
	void Start () {
		target = GameObject.Find ("ShadeMan").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.LookRotation(transform.position - target.position);
	}
}
