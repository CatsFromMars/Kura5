using UnityEngine;
using System.Collections;

public class PerlinWobble : MonoBehaviour {

    public Quaternion start;
    public Vector3 Speeds;
    public Vector3 Multipliers;

	// Use this for initialization
	void Start () {
        start = transform.localRotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        var offset = Quaternion.Euler((Mathf.PerlinNoise(Time.time * Speeds.x, 1) - 0.5f) * Multipliers.x, (Mathf.PerlinNoise(Time.time * Speeds.y, 2) - 0.5f) * Multipliers.y, (Mathf.PerlinNoise(Time.time * Speeds.z, 3) - 0.5f) * Multipliers.z);
        transform.localRotation = start * offset;
	}
}
