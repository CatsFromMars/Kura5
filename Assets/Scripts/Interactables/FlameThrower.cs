using UnityEngine;
using System.Collections;

public class FlameThrower : SwitchActivated {

	public Transform hitbox;
	public ParticleEmitter p;
	
	// Update is called once per frame
	void FixedUpdate () {
		if (activated) {
			p.emit = true;
			hitbox.gameObject.SetActive(true);
		}
		else {
			p.emit = false;
			hitbox.gameObject.SetActive(false);
		}
	}
	
}
