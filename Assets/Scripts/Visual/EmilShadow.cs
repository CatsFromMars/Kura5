using UnityEngine;
using System.Collections;

public class EmilShadow : MonoBehaviour {
	//Dumb workaround for issue with Emil's shadow
	public Transform bone;

	// Update is called once per frame
	void Update () {
		this.transform.position = bone.transform.position;
	}
}
