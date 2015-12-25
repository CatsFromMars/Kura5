using UnityEngine;
using System.Collections;

public class TimedDissapear : MonoBehaviour {
	private int counter = 0;
	private int waitTime = 100;

	// Update is called once per frame
	void Update () {
		counter++;
		if(counter>waitTime) {
			counter = 0;
			gameObject.SetActive(false);
		}
	}
}
