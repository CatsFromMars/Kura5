using UnityEngine;
using System.Collections;

public class ParticlesUnscaledTime : MonoBehaviour
{
	ParticleSystem p;
	bool timed = true;
	int counter = 0;
	public int waitTime = 20;

	void Awake() {
		p = GetComponent<ParticleSystem>();
	}
	// Update is called once per frame
	void Update()
	{
		counter++;
		if(counter <= waitTime) {
			p.Simulate(Time.unscaledDeltaTime, true, false);
			//p.Emit(1);
		}
	}
}