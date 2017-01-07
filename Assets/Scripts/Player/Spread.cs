using UnityEngine;
using System.Collections;

public class Spread : MonoBehaviour {
	public string element = "Sol";
	public int damage = 2;
	public float meleeRange = 5.5f; //range of current weapon
	public float meleeAngle = 160f;
	public Transform player;

	void Update() {
		//Raycast
		Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, meleeRange);
		
		for(int i=0; i<hitColliders.Length; i++) {
			Transform target = hitColliders[i].transform;
			Vector3 targetDir = (target.position - player.transform.position).normalized;
			if(!hitColliders[i].isTrigger && Vector3.Angle(transform.forward, targetDir) < meleeAngle/2) {
				float dist = Vector3.Distance(player.transform.position, target.position);
				RaycastHit hit;
				if(Physics.Raycast (player.transform.position, targetDir, out hit, dist)) {
					if(hit.collider.gameObject.tag == "Enemy") {
						hitEnemy(hit);
						if(checkForBossSegment(hit)) break;
					}
				}
			}
		}
	}

	bool checkForBossSegment(RaycastHit hit) {
		return hit.collider.GetComponent<EnemyClass>() != null;
	}

	void hitEnemy(RaycastHit hit) {
		int m = 1;
		EnemySegment b = null;
		//Ordinary Enemies
		EnemyClass enemy = hit.collider.GetComponent<EnemyClass>();
		//if it's a boss segment...
		if(enemy == null) { 
			b = hit.collider.GetComponent<EnemySegment>();
			b.hitWithSword();
			enemy = b.enemyParent;
			m = b.damageMultiplier;
		}
		else {
			enemy.takeDamage (damage*m, element);
			//enemy.knockback (transform.forward*5);
		}

	}
}
