using UnityEngine;
using System.Collections;

public class AlwaysFaceCamera : MonoBehaviour {
	GameObject mainCamera;
	public bool rotOnly = false;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
	
	}
	
	// Update is called once per frame
	void Update () {

		if(!rotOnly) transform.LookAt(mainCamera.transform.position);
		else {
			transform.LookAt(mainCamera.transform.position);
			transform.localRotation = Quaternion.Euler(transform.localRotation.x,transform.localRotation.y,0);
		}
	}
}
