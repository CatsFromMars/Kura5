using UnityEngine;
using System.Collections;

public class DustAnimEvent : MonoBehaviour {
	public ParticleSystem p;
	// Use this for initialization
	void playParticle() {
		p.Simulate(Time.unscaledDeltaTime, true, false);
		p.Emit(15);
	}
}
