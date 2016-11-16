using UnityEngine;
using System.Collections;

public class DisableGOAnim : MonoBehaviour {
	public Transform go;
	//AnimationEvent toggle gameobject

	public void toggleActive() {
		go.gameObject.SetActive (false);
	}

	public void Enable() {
		go.gameObject.SetActive (true);
	}
}
