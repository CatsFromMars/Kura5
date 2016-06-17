using UnityEngine;
using System.Collections;

public class StraightProjectile : MonoBehaviour {
	public float elapsedTime;

	private Vector3 direction;
	public float velocity = 25f;
	public float killTime = 100f;

	void Awake() {
		direction = transform.forward;
	}

	// Update is called once per frame
	void FixedUpdate() {
		transform.position += direction * velocity * Time.deltaTime;
		elapsedTime++;
		if(elapsedTime > killTime) Destroy(this.gameObject);
	}
}
