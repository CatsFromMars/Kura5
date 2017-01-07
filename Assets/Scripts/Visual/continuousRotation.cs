using UnityEngine;
using System.Collections;

public class continuousRotation : MonoBehaviour {

	public float rotationSpeed = 1f;
	public bool zaxis = false;

	void Update()
	{
		if(zaxis) transform.Rotate(0, 0, rotationSpeed*Time.unscaledDeltaTime);
		else transform.Rotate(0, rotationSpeed*Time.unscaledDeltaTime, 0);
	}
}
