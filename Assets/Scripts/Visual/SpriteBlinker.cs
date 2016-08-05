using UnityEngine;
using System.Collections;

public class SpriteBlinker : MonoBehaviour {
	public float blinkInterval = 5f;
	private float blinkTimer = 0f;
	private SpriteRenderer r;

	void Awake() {
		r = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {
		blinkTimer += Time.unscaledDeltaTime;
		if (blinkTimer >= blinkInterval) {
			r.enabled = !r.enabled;
			blinkTimer = 0;
		}
	}
}
