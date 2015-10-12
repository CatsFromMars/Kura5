using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
	public Transform target;
	int moveSpeed = 8; //move speed
	int rotationSpeed = 5; //speed of turning
	float range = 30f;
	float range2 = 30f;
	float stop = 3;
	float origY;
	// Use this for initialization
	void Start () {
		origY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
				//rotate to look at the player
				Vector3 targetPos = new Vector3(target.position.x, target.position.y+origY, target.position.z);
				float distance = Vector3.Distance (transform.position, targetPos);
				if (distance <= range2 && distance >= range) {
						transform.rotation = Quaternion.Slerp (transform.rotation,
				                                        Quaternion.LookRotation (target.position - transform.position), rotationSpeed * Time.deltaTime);
				} else if (distance <= range && distance > stop) {
				
						//move towards the player
						transform.rotation = Quaternion.Slerp (transform.rotation,
				                                        Quaternion.LookRotation (target.position - transform.position), rotationSpeed * Time.deltaTime);
						transform.position += transform.forward * moveSpeed * Time.deltaTime;
						Vector3 pos = transform.position;
						pos.y = origY;
						transform.position = pos;
				} else if (distance <= stop) {
						transform.rotation = Quaternion.Slerp (transform.rotation,
				                                        Quaternion.LookRotation (target.position - transform.position), rotationSpeed * Time.deltaTime);
				}
		}
}
