using UnityEngine;
using System.Collections;

public class AlwaysFaceCamera : MonoBehaviour {
	GameObject mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.LookAt(mainCamera.transform.position);
	
	}
}
