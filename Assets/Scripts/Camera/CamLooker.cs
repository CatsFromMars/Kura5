using UnityEngine;
using System.Collections;

public class CamLooker : MonoBehaviour {

	public Transform lookerSelf;
	public Transform currentLook;
	private IEnumerator coroutine;

	void Awake() {

	}

	public void zoomToTarget(Transform target, float speed=8f) {
		if(coroutine != null) StopCoroutine (coroutine);
		coroutine = lookAtTarget (target, speed);
		StartCoroutine (coroutine);
		currentLook = target;
	}

	public IEnumerator lookAtTarget(Transform target, float speed=8f) {
		while(transform.position != target.position) {
			float step = speed * Time.unscaledDeltaTime;
			transform.position = Vector3.MoveTowards(transform.position, target.position, step);
			yield return null;
		}
	}
}
