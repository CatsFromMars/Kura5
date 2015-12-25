using UnityEngine;
using System.Collections;

public class UnscaledTimeParticle : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
		if (Time.timeScale < 0.01f && particleSystem.IsAlive())
		{
			particleSystem.Simulate(Time.unscaledDeltaTime, true, false);
		}
	}
}