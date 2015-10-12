using UnityEngine;
using System.Collections;

public class continuousRotation : MonoBehaviour {

	public float rotationSpeed = 0.1f;

	void Update()
	{
		transform.Rotate(0, rotationSpeed, 0);
	}
}
