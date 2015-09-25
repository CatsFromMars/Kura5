using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

	Transform follow;
	Vector3 currentpos;
	float distance = -30.0f;
	public float smoothTime = 0.3f; //FOR DAMPENING
	private Vector3 velocity = Vector3.zero; //FOR DAMPENING

	// Update is called once per frame
	void Start() {
		follow = GameObject.FindWithTag("CamFollow").transform;
	}

	void Update () {
		currentpos = transform.position;
		currentpos.x = follow.position.x;
		currentpos.z = follow.position.z + distance;
		transform.position = Vector3.SmoothDamp(transform.position, currentpos, ref velocity, smoothTime);
	
	}
}
