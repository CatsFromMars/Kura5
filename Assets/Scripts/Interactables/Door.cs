using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	Animator animator;
	public enum doorType {REGULAR, TRIANGLE, CIRCLE, SQUARE};
	public doorType type;
	private bool doorLocked = true;
	private bool inRange;
	int keyItemID;
	private TextAsset lockedDoorPrompt;
	private TextAsset openedDoorPrompt;
	private Inventory inventory;
	private GameData data;
	public BoxCollider doorCollider;
	private SphereCollider col;
	public float triggerRadius = 5f;
	public float lockedRadius = 2.7f;

	void Awake() {
		GameObject global = GameObject.FindGameObjectWithTag ("GameController");
		inventory = global.GetComponent<Inventory>();
		animator = GetComponent<Animator>();
		data = global.GetComponent<GameData>();

		//doorCollider = transform.FindChild("door1").GetComponent<BoxCollider>();
		col = GetComponent<SphereCollider>();

		if(type == doorType.REGULAR) {
			doorLocked = false;
			col.radius = triggerRadius;
			doorCollider.enabled = false;
		}
		else if(type == doorType.TRIANGLE) {
			lockedDoorPrompt = Resources.Load("Dialogue/TriangleDoor") as TextAsset;
			keyItemID = 0;
			col.radius = lockedRadius;
			doorCollider.enabled = true;
		}
		else if(type == doorType.CIRCLE) {
			lockedDoorPrompt = Resources.Load("Dialogue/CircleDoor") as TextAsset;
			keyItemID = 3;
			col.radius = lockedRadius;
			doorCollider.enabled = true;
		}
		else if(type == doorType.SQUARE) {
			lockedDoorPrompt = Resources.Load("Dialogue/SquareDoor") as TextAsset;
			keyItemID = 4;
			col.radius = lockedRadius;
			doorCollider.enabled = true;
		}

		openedDoorPrompt = Resources.Load("Dialogue/OpenedDoor") as TextAsset;
	}

	void Update() {
		if (doorLocked && inRange && Input.GetButtonDown("Charge")) {
			int hasKey = inventory.checkForKeyItem(keyItemID);
			Debug.Log(hasKey);
			if(hasKey != -1) {
				doorLocked = false;
				inventory.removeKeyItem(hasKey);
				animator.SetBool(Animator.StringToHash("PlayerInRange"), true);
				col.radius = triggerRadius;
				doorCollider.enabled = false;
				StartCoroutine(DisplayDialogue.Speak(openedDoorPrompt));
				data.nearInteractable = false;
			}
			else {
				StartCoroutine(DisplayDialogue.Speak(lockedDoorPrompt));
			}
			inRange = false;
		}

	}

	void OnTriggerEnter(Collider other) {

		if(!doorLocked) {
			if(other.tag == "Player" || other.tag == "EnemyWeapon") 
				animator.SetBool(Animator.StringToHash("PlayerInRange"), true);
		}
		else inRange = true;

		if(doorLocked) data.nearInteractable = true;
	}

	void OnTriggerExit(Collider other) {
		if(!doorLocked) {
			if(other.tag == "Player" || other.tag == "EnemyWeapon") 
				animator.SetBool(Animator.StringToHash("PlayerInRange"), false);
		}
		else inRange = false;

		if(doorLocked) data.nearInteractable = false;
	}
}
