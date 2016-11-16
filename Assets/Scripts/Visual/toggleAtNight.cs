using UnityEngine;
using System.Collections;

public class toggleAtNight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer>().enabled = (GetUtil.getWeather().isNightTime);
	}

}
