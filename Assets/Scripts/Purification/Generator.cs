using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	public PurificationController purification;
	public enum generatorState {ACTIVATED, DEACTIVATED, DANGER, COOLDOWN}
	public generatorState state = generatorState.DEACTIVATED;
	private Animator animator;
	private bool canBeActivated = true;
	public GameObject dangerProjector;
	private GameObject flash;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		flash = transform.FindChild("Flasher").gameObject;
		StartCoroutine(mainLoop());
	}

	void OnCollisionEnter(Collision collision) {
		
		if (collision.collider.gameObject.tag == "Bullet") {
			StartCoroutine(flashWhite());
			if(!purification.inDarkMode&&(state==generatorState.DANGER||state==generatorState.DEACTIVATED)) state = generatorState.ACTIVATED;
		}
		
	}

	IEnumerator mainLoop() {
		while(!purification.complete) {
			yield return StartCoroutine(state.ToString());
		}
	}

	IEnumerator flashWhite() {
		flash.SetActive (true);
		yield return new WaitForSeconds (0.2f);
		flash.SetActive (false);
	}

	public virtual IEnumerator ACTIVATED() {
		//if(Time.timeScale!=0) StartCoroutine(purification.begin());
		dangerProjector.SetActive (false);
		animator.SetBool(Animator.StringToHash("Activated"),true);
		animator.SetBool(Animator.StringToHash("Danger"),false);
		yield return new WaitForSeconds(0.2f);
		while (state==generatorState.ACTIVATED) {
			if(purification.sunlight==0) state = generatorState.DEACTIVATED;
			else purification.takeGeneratorDamage();
			yield return null;
		}
	}

	public virtual IEnumerator DEACTIVATED() {
		animator.SetBool(Animator.StringToHash("Activated"),false);
		animator.SetBool(Animator.StringToHash("Cooldown"),false);
		canBeActivated = false;
		while (state==generatorState.DEACTIVATED) {
			yield return null;
		}
		yield return null;
	}

	public virtual IEnumerator DANGER() {
		dangerProjector.SetActive (true);
		animator.SetBool(Animator.StringToHash("Danger"),true);
		while (state==generatorState.DANGER) {
			purification.takeGeneratorDamage();
			yield return null;
		}
		yield return null;
	}

	public virtual IEnumerator COOLDOWN() {
		dangerProjector.SetActive (false);
		animator.SetBool(Animator.StringToHash("Activated"),false);
		animator.SetBool(Animator.StringToHash("Cooldown"),true);
		while (state==generatorState.COOLDOWN) {
			yield return new WaitForSeconds(1f);
			state = generatorState.DEACTIVATED;
			yield return null;
		}
		yield return null;
	}

}
