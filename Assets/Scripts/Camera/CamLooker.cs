using UnityEngine;
using System.Collections;

public class CamLooker : MonoBehaviour {

	public Transform lookerSelf;
	public Transform currentLook;
	private IEnumerator coroutine;
	public GameObject defaultLook;
	public Transform playerPositionContainer;
	public Transform crosshair;
	public float range = 15f;

	void Awake() {
		//Debug.Log ("I am here: "+gameObject.name);
	}

	void OnLevelWasLoaded(int level) {
	}

	void Update() {
		if(currentLook==null) {
			zoomToTarget(defaultLook.transform);
		}
		if(currentLook==defaultLook.transform) transform.parent=defaultLook.transform;
	}

	public void zoomToTarget(Transform target, float speed=21f, bool limitRaduis=false) {
		//Debug.Log (target);
		if(coroutine != null) StopCoroutine (coroutine);
		coroutine = lookAtTarget (target, speed, limitRaduis);
		StartCoroutine (coroutine);
		currentLook = target;
	}

	public IEnumerator lookAtTarget(Transform target, float speed=21f, bool limitRadius=false) {
		while(Vector3.Distance(transform.position, target.transform.position) > 0.1f) {
			float step = speed * Time.unscaledDeltaTime;
			Vector3 moveTowards = Vector3.MoveTowards(transform.position, target.position, step);
			Vector3 allowedPos = moveTowards - playerPositionContainer.position;
			allowedPos = Vector3.ClampMagnitude(allowedPos, range);
			if(limitRadius) {
				Vector3 newPos = playerPositionContainer.position + allowedPos;
				newPos.y = transform.position.y;
				transform.position = newPos;
				yield return null;
			}
			else {
				transform.position = Vector3.MoveTowards(transform.position, target.position, step);
				yield return null;
			}
			yield return null;
		}
	}
}
