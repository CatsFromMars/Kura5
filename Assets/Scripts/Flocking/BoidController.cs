using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// these define the flock's behavior
/// </summary>
public class BoidController : MonoBehaviour
{
	public float minVelocity = 5;
	public float maxVelocity = 20;
	public float randomness = 1;
	public int flockSize = 20;
	public BoidFlocking prefab;
	public Transform target;
	public bool respawnDestroyedFlock = false;

	internal Vector3 flockCenter;
	internal Vector3 flockVelocity;

	List<BoidFlocking> boids = new List<BoidFlocking>();

	void Start()
	{
		for (int i = 0; i < flockSize; i++)
		{
			BoidFlocking boid = Instantiate(prefab, transform.position, transform.rotation) as BoidFlocking;
			boid.transform.parent = transform;
			boid.transform.localPosition = new Vector3(
							Random.value * collider.bounds.size.x,
							Random.value * collider.bounds.size.y,
							Random.value * collider.bounds.size.z) - collider.bounds.extents;
			boid.controller = this;
			boids.Add(boid);
		}
	}

	void updateBoidCount() {
		for(int i=0; i<boids.Count; i++)
		{
			if(boids[i]==null) {
				BoidFlocking b = Instantiate(prefab, transform.position, transform.rotation) as BoidFlocking;
				b.transform.parent = transform;
				b.transform.localPosition = new Vector3(
					Random.value * collider.bounds.size.x,
					Random.value * collider.bounds.size.y,
					Random.value * collider.bounds.size.z) - collider.bounds.extents;
				b.controller = this;
				boids.Remove(boids[i]);
				boids.Add(b);
			}
		}
	}

	void updateBoids() {
		Vector3 center = Vector3.zero;
		Vector3 velocity = Vector3.zero;
		foreach (BoidFlocking boid in boids)
		{
			if(boid!=null) {
				center += boid.transform.localPosition;
				velocity += boid.rigidbody.velocity;
			}
		}
		flockCenter = center / flockSize;
		flockVelocity = velocity / flockSize;
	}

	void Update()
	{
		if(respawnDestroyedFlock) updateBoidCount ();
		updateBoids ();
	}
}