using UnityEngine;
using System.Collections;

public class EmilShadow : MonoBehaviour {
	//Dumb workaround for issue with Emil's shadow
	public Transform bone;
	private float y;
	void Awake() {
		y = transform.localPosition.y;
	}

	// Update is called once per frame
	void Update () {
		transform.position = bone.transform.position;
		Vector3 pos = transform.localPosition;
		pos.y = y;
		transform.localPosition = pos;
	}
}
