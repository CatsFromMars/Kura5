using UnityEngine;
using System.Collections;

public class UnscaledTimeParticle : MonoBehaviour
{
	public bool alwaysSimulate = false;
	// Update is called once per frame
	void Update()
	{
		if ((Time.timeScale < 0.01f||alwaysSimulate) && particleSystem.IsAlive())
		{
			particleSystem.Simulate(Time.unscaledDeltaTime, true, false);
		}
	}
}