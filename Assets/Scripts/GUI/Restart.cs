using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R))
						Application.LoadLevel (Application.loadedLevel);
	}
}
