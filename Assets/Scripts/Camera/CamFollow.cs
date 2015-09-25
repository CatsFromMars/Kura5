using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

	Transform player;
	Vector3 currentpos;
	float distance = -30.0f;
	public float smoothTime = 0.3f; //FOR DAMPENING
	private Vector3 velocity = Vector3.zero; //FOR DAMPENING

	// Update is called once per frame
	void Start() {
		player = GameObject.FindWithTag("Player").transform;
	}

	void Update () {
		if(player == null) player = GameObject.FindWithTag("Player").transform;
		currentpos = transform.position;
		currentpos.x = player.position.x;
		currentpos.z = player.position.z + distance;
		transform.position = Vector3.SmoothDamp(transform.position, currentpos, ref velocity, smoothTime);
	
	}
}
