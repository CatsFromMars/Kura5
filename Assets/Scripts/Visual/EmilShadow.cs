using UnityEngine;
using System.Collections;

public class EmilShadow : MonoBehaviour {
	//Dumb workaround for issue with Emil's shadow
	public Transform bone;

	// Update is called once per frame
	void Update () {
		Vector3 pos = new Vector3 (bone.transform.position.x, this.transform.position.y, bone.transform.position.z);
		this.transform.position = pos;
	}
}
