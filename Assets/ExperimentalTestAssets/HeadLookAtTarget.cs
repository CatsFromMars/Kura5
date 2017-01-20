using UnityEngine;
using System.Collections;

public class HeadLookAtTarget : MonoBehaviour {
	public bool followOtherTarget = false;
	public Transform target;
	private Vector3 startRot;
	//public float clampX = 45f;
	public float clampY = 45f;
	public float clampZ = 45f;

	//How to use
	//Create empty GameObject and put it in the same position as your character's "head" bone
	//Make sure said empty GameObject is rotated such that the blue arrow is facing forward
	//Attach this script onto it
	//Make the GameObject a parent of the head bone, but a child to the armature

	void Awake() {
		startRot = transform.root.eulerAngles;
		if(!followOtherTarget) target = GetUtil.getPlayer().transform;
	}

	void LateUpdate()
	{
		transform.LookAt(target.position);
		Vector3 rot = transform.rotation.eulerAngles;
		//rot.x = Mathf.Clamp(transform.eulerAngles.x, startRot.x-clampX, startRot.x+clampX);
		rot.y = Mathf.Clamp(transform.eulerAngles.y, startRot.y-clampY, startRot.y+clampY);
		rot.z = Mathf.Clamp(transform.eulerAngles.z, startRot.z-clampZ, startRot.z+clampZ);
		transform.rotation = Quaternion.Euler(rot);
	}
}
