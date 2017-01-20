using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
	public GameObject currentTarget;
	public Sprite active;
	public Sprite inactive;
	SpriteRenderer ren;
	public Transform player;
	public bool camLook = false;
	private float speed = 18f;
	private float horizontal;
	private float vertical;

	void OnEnable() {
		targetClosestEnemy ();
	}

	void Start() {
		ren = GetComponent<SpriteRenderer>();
	}

	void Update() {
		if(currentTarget==null) ren.sprite = inactive;
		else ren.sprite = active;
		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis ("Vertical");
		Vector3 move = new Vector3(horizontal, 0, vertical);
		float dir = Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical));

		if(currentTarget!=null && dir<0.3f) transform.position = currentTarget.transform.position;
		else transform.position += move * speed * Time.deltaTime;


		//Prevent from going off screen
		float widthRel = this.transform.localScale.y / (Screen.width) / 2; //relative width
		float heightRel = this.transform.localScale.x / (Screen.height) / 2; //relative height
		
		Vector3 viewPos = Camera.main.WorldToViewportPoint (this.transform.position);
		viewPos.x = Mathf.Clamp (viewPos.x, widthRel, 1 - widthRel);
		viewPos.y = Mathf.Clamp (viewPos.y, heightRel, 1 - heightRel);
		Vector3 newPos = Camera.main.ViewportToWorldPoint (viewPos);
		newPos.y = transform.position.y;
		transform.position = newPos;
	}

	void OnDisable() {
		currentTarget = null;
		ren.sprite = inactive;
	}


	public void OnTriggerStay(Collider other) {
		bool enemy = other.collider.tag == "Enemy" && !other.collider.isTrigger;
		if(enemy || other.collider.tag == "EnemyWeapon") {
			currentTarget = other.collider.gameObject;
		}
	}

	public void OnTriggerExit(Collider other) {
		bool enemy = other.collider.tag == "Enemy" && !other.collider.isTrigger;
		if(enemy || other.collider.tag == "EnemyWeapon") {
			currentTarget = null;
		}
	}

	void targetClosestEnemy() {
		//ONLY on screen enemies can be targeted
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject tMin = null;
		float minDist = Mathf.Infinity;
		Vector3 currentPos = transform.position;
		foreach (GameObject t in enemies)
		{
			float dist = Vector3.Distance(t.transform.position, currentPos);
			if (dist < minDist && enemyOnScreen(t))
			{
				tMin = t;
				minDist = dist;
			}
		}
		//Now lock onto tMin, the closest enemy
		if(tMin!=null) currentTarget = tMin;
		else transform.position = (player.position+(player.transform.forward*5));
	}

	bool enemyOnScreen(GameObject enemy) {
		Vector3 viewPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
		if(viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1) return true;
		else return false;
	}
	
}
