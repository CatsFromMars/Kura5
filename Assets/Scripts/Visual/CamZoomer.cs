using UnityEngine;
using System.Collections;

public class CamZoomer : MonoBehaviour {
	
	public void zoomTo(float orthoSize) {
		StartCoroutine (zoom (orthoSize));
	}

	// Update is called once per frame
	IEnumerator zoom(float orthoSize) {
		float orig = Camera.main.orthographicSize;
		while(Camera.main.orthographicSize != orthoSize) {
			Camera.main.orthographicSize = Mathf.Lerp(orig, orthoSize, Time.unscaledTime*0.5f);
			yield return null;
		}
	}
}
