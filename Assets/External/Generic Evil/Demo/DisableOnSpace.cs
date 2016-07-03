using UnityEngine;
using System.Collections;

public class DisableOnSpace : MonoBehaviour {

	public MonoBehaviour target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            target.enabled = !target.enabled;
        }
	}
}
