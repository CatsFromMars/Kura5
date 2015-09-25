using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour {

	public ParticleSystem fire;
	public bool isLit;
	public float speedOrig = 40f;
	public float speed = 20f;
	public Transform[] waypoints;
	private int currentPoint = 0;
	float step;
	public Transform debrisSpawner;
	public Transform debris;

	Animator animator;
	GameObject gameData;
	//GameData Data;
	HashIDs hash;

	public AudioClip fwoosh;

	void Awake() {
		gameData = GameObject.FindGameObjectWithTag("GameController");
		animator = GetComponent<Animator>();
		//Data = gameData.GetComponent<GameData>();
		hash = gameData.GetComponent<HashIDs>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isLit) fire.Play();
		Move();
	}

	public void Move() {
		if(transform.position == waypoints[currentPoint].position) { //IF WE'RE AT A WAYPOINT...
			Debug.Log ("?");
			//INCREMENT WAYPOINT
			if(currentPoint == 1) animator.SetTrigger(hash.tapTrigger);

			if (currentPoint >= waypoints.Length - 1) currentPoint = 0;
			else currentPoint++;
			
		}
		
		else { //IF NOT AT A WAYPOINT...
			//...MOVE TOWARDS A WAYPOINT!
			Vector3 target = waypoints[currentPoint].position;
			float distance = Vector3.Distance(transform.position, target);
				if (animator.GetCurrentAnimatorStateInfo(0).nameHash == hash.stillState) {
				transform.position = Vector3.Lerp (transform.position, target, 
				                                   speed/distance * Time.deltaTime);
			}
		}
		
	}

	public void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "FireBullet") {
			if(!isLit) {
				isLit = true;
				makeSound(fwoosh);
			}
		}

		if (other.gameObject.tag == "Candles" && isLit) {
			Candles candles = other.GetComponent<Candles>();
			if(!candles.candlesLit)
			{
				makeSound(fwoosh);
			    candles.candlesLit = true;
			}
		}

		
	}

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
		
	}

	void spawnDebris() {
		Instantiate (debris, debrisSpawner.position, Quaternion.Euler(-90,0,0));
	}

}
