using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Vibration : MonoBehaviour {

	private IEnumerator coroutine;

	public void vibrate(float intensity, float time) {
		if(coroutine != null) StopCoroutine (coroutine);
		coroutine = rumble(intensity, time);
		StartCoroutine (coroutine);
	}

	public IEnumerator rumble(float intensity, float time) {
		PlayerIndex playerIndex = (PlayerIndex)0;
		GamePad.SetVibration(playerIndex, intensity, intensity);
		float timer = 0;
		while (timer < time) {
			timer += Time.unscaledDeltaTime;
			yield return null;
		}
		GamePad.SetVibration(playerIndex, 0, 0);
	}

	public static PlayerIndex getPlayerIndex() {
		return 0;
	}
}
