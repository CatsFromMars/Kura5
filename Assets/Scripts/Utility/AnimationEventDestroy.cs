using UnityEngine;
using System.Collections;

public class AnimationEventDestroy : MonoBehaviour {

	public void destroyMe() {
		Destroy (this.gameObject);
	}

	public void destroyParent() {
		Destroy (transform.parent.gameObject);
		Instantiate (Resources.Load ("Effects/Pickup") as GameObject, transform.position, Quaternion.identity);
	}
}
