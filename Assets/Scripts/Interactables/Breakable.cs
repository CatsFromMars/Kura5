using UnityEngine;
using System.Collections;

public class Breakable : MonoBehaviour {

	public void Shatter() {
		//To be called by Emil's sword raycast.
		Instantiate (Resources.Load ("Effects/Debris") as GameObject, transform.position, transform.rotation);
		Destroy (this.gameObject);
	}
}
