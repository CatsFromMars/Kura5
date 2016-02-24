using UnityEngine;
using System.Collections;

public class ObjectBlinker : MonoBehaviour {
	MeshRenderer m;
	// Use this for initialization
	void Start () {
		m=GetComponent<MeshRenderer>();
		StartCoroutine (blink());
	}
	
	IEnumerator blink() {
		while (this.gameObject.activeSelf) {
			yield return new WaitForSeconds(0.5f);
			m.enabled = false;
			yield return new WaitForSeconds(0.5f);
			m.enabled = true;

		}
	}
}
