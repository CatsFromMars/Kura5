using UnityEngine;
using System.Collections;

public class ResetGame : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown("r")) {
			Application.LoadLevel ("TestScene"); 
			
		}
	
	}
}
