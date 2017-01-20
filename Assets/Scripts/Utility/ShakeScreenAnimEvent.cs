using UnityEngine;
using System.Collections;

public class ShakeScreenAnimEvent : MonoBehaviour {

	// Use this for initialization
	public static void ShakeScreen() {
		Camera.main.GetComponent<Animator>().SetTrigger (Animator.StringToHash("Shake"));
	}

	public void shakeScreen() {
		//non static version
		Camera.main.GetComponent<Animator>().SetTrigger (Animator.StringToHash("Shake"));
	}

	public static void LittleShake() {
		Camera.main.GetComponent<Animator>().SetTrigger (Animator.StringToHash("LittleShake"));
	}

	public static void BigShake() {
		Camera.main.GetComponent<Animator>().SetTrigger (Animator.StringToHash("BigShake"));
	}

	public void flashScreen() {
		CameraFlash.flashCamera ();
	}
}
