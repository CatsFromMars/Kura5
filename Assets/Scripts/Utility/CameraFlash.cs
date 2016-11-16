using UnityEngine;
using System.Collections;

public class CameraFlash : MonoBehaviour {

	public static void flashCamera() {
		GameObject.Find ("CutsceneFade").GetComponent<Animator> ().SetTrigger (Animator.StringToHash("Flash"));;
	}
}
