using UnityEngine;
using System.Collections;

public class RaycastBullet : MonoBehaviour {
	public string element = "Sol";
	public int damage = 5;
	public float stunTime = 0.5f;
	private float range = 20f;

	public Transform hitEffect;
	public Transform maxHitEffect;
	playerWeaponClass wep;

	Vector3 targetpoint = Vector3.zero;


	private float delay; //how long it takes for bullet to hit enemy

	// Use this for initialization
	void OnEnable() {
		element = GameData.annieWeaponConfig.element;
		wep = GameData.annieWeaponConfig;
		damage = wep.damage;
		Fire();
	}
	
	void OnDrawGizmos() {
		// global forward
		DrawHelperAtCenter(this.transform.forward, Color.white, 30f);
	}
	
	private void DrawHelperAtCenter(
		Vector3 direction, Color color, float scale)
	{
		Gizmos.color = color;
		Vector3 destination = transform.position + direction * scale;
		Gizmos.DrawLine(transform.position, destination);
	}

	// Update is called once per frame
	void Fire () {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit))
		{
			Collider target = hit.collider; // What did I hit?
			float distance = hit.distance; // How far out?
			Vector3 location = hit.point; // Where did I make impact?
			GameObject targetGameObject = hit.collider.gameObject; // What's the GameObject?
			targetpoint = location;
			Debug.Log(target.name);
		}
	}

	void hitEnemy(Vector3 pos) {
		Debug.Log("H-hit!");
		SpawnEffect(pos);
		gameObject.Recycle();
	}

	void SpawnEffect(Vector3 t) {
		if(GameData.annieWeaponConfig.power > 4) {
			Instantiate(maxHitEffect, t, Quaternion.identity);
		}
		else Instantiate(hitEffect, t, Quaternion.identity);
	}

	bool checkForBossSegment(RaycastHit hit) {
		return hit.collider.GetComponent<EnemyClass>() != null;
	}
}
