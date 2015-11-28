using UnityEngine;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
	private Animator a;

	void Awake() {
		a = GetComponent<Animator>();
	}


	void OnLevelWasLoaded() {
		a.SetTrigger(Animator.StringToHash("FadeIn"));
	}


}