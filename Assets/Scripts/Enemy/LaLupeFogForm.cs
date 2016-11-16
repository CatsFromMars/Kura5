using UnityEngine;
using System.Collections;

public class LaLupeFogForm : MonoBehaviour {
	private Transform player;
	private NavMeshAgent agent;
	public WadjetBoss wadjet;
	public ParticleSystem head;
	public ParticleSystem tail;
	public SunDetector sunDetector;
	private bool burned = false;
	private bool returning = false;
	public Color orig;
	public Color burnColor;
	private ShakeScreenAnimEvent shaker;
	public AudioClip burn;
	private float minSpeed = 7f;
	private float minTime = 10f;
	private float maxSpeed = 12f;
	private float maxTime = 12f;
	private float fleeSpeed = 12f;
	public SphereCollider col;
	public GameObject smoke;

	// Use this for initialization
	void OnEnable() {
		player = GetUtil.getPlayer().transform;
		agent = GetComponent<NavMeshAgent>();
		tail.startColor = orig;
		head.startColor = orig;
		burned = false;
		returning = false;
		shaker = GetComponent<ShakeScreenAnimEvent>();
		StartCoroutine (timed());
		tail.Play();
		head.Play();
		col.enabled = true;
		smoke.SetActive (false);
	}

	IEnumerator timed() {
		float percent = 1-(wadjet.currentLife / wadjet.maxLife);
		agent.speed = 0;
		yield return new WaitForSeconds (1f);
		agent.speed = minSpeed + (maxSpeed * percent);
		float time = minTime + (maxTime * percent);
		yield return new WaitForSeconds (time);
		returning = true;
		StartCoroutine(flee());
	}

	IEnumerator scorch() {
		shaker.shakeScreen();
		smoke.SetActive (true);
		makeSound(burn);
		tail.startColor = burnColor;
		head.startColor = burnColor;
		burned = true;
		agent.speed = 0;
		yield return new WaitForSeconds (1f);
		StartCoroutine(flee());
	}

	IEnumerator flee() {
		agent.speed = fleeSpeed;
		yield return new WaitForSeconds (2f);
		tail.Stop();
		col.enabled = false;
		yield return new WaitForSeconds (4f);
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if(!burned && !returning) agent.SetDestination (player.position);
		else {
			agent.SetDestination(wadjet.transform.position);
			if(agent.remainingDistance <= agent.stoppingDistance) {
				head.Stop();
				tail.Stop();
			}
		}

		if(sunDetector.sunlight.GetValue() > 0 && !burned) {
			//wadjet.takeDamage(sunDetector.sunlight.GetValue());
			StartCoroutine(scorch());
		}
	}

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
	}

}
