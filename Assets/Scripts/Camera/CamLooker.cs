using UnityEngine;
using System.Collections;

public class CamLooker : MonoBehaviour {

	public Transform lookerSelf;
	public Transform currentLook;
	private IEnumerator coroutine;

	void Awake() {

	}

	void OnLevelWasLoaded(int level) {
	}

	public void zoomToTarget(Transform target, float speed=21f) {
		if(coroutine != null) StopCoroutine (coroutine);
		coroutine = lookAtTarget (target, speed);
		StartCoroutine (coroutine);
		currentLook = target;
	}

	public IEnumerator lookAtTarget(Transform target, float speed=21f) {
		while(Vector3.Distance(transform.position, target.transform.position) > 0.1f) {
			float step = speed * Time.unscaledDeltaTime;
			transform.position = Vector3.MoveTowards(transform.position, target.position, step);
			yield return null;
		}
	}
}
