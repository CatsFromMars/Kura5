using UnityEngine;
using System.Collections;

public class ShadowSeal : MonoBehaviour {
	public GameObject ring;
	public float shrinkSpeed = 1;
	private float initialScale = 27.5f;
	public float targetScale = 9f;

	public GameObject danger;
	public Ectoplasm ectoplasm;
	public Vector3 smallScale;
	private Vector3 normalScale;
	public GameObject effect;
	public ParticleSystem breakEffect;
	public AudioSource breakSound;
	public MeshRenderer r;
	private bool finished;
	//public Animator vformAnimator;
	public GameObject growingSeal;
	//public GameObject vformContainer;
	public GameObject lunabug;
	public bool lockedIn = false;

	public bool isActivated = false;
	public bool ableToCharge = true;

	// Use this for initialization
	void Start() {
		smallScale = new Vector3(targetScale, targetScale, targetScale);
		normalScale = new Vector3(initialScale,initialScale,initialScale);
		StartCoroutine (startSeal ());
	}

	void Update() {
		if(ectoplasm.purification.complete&&!finished) {
			finished = true;
			StopAllCoroutines();
			breakSeal();
			//vformAnimator.SetBool(Animator.StringToHash("DarkMode"),false);
		}
		else {
			danger.SetActive(!ableToCharge);
		}
	}

	void getShrinkSpeed() {
		if(ectoplasm.purification.inDarkMode) {
			//Shadow seal shrinks faster in strong moonlight
			//shrinkSpeed = 0.25f*(ectoplasm.purification.w.lightMax.GetValue()/2f);
			shrinkSpeed = 0.3f;
		}
		else shrinkSpeed = 0.1f;
	}

	IEnumerator startSeal() {
		while(!ectoplasm.purification.began) yield return null;
		getShrinkSpeed ();
		growingSeal.SetActive(true);
		//vformAnimator.SetBool(Animator.StringToHash("DarkMode"),ectoplasm.purification.inDarkMode);
		r.enabled = true;
		while(!Mathf.Approximately(ring.transform.localScale.x,smallScale.x)) {
			shrinkRing();
			yield return null;
		}
		isActivated = true;
		//vformAnimator.SetBool(Animator.StringToHash("ShadowSealActive"),true);
		ectoplasm.state = Ectoplasm.ectoState.SEALED;
		effect.SetActive (true);
		float timer = 0;
		float waitTime = 6;
		while(timer<waitTime) {
			timer+=Time.deltaTime;
			if(isActivated == false) break;
			yield return null;
		}
		if(!ectoplasm.purification.complete&&ectoplasm.purification.inDarkMode) Instantiate(lunabug, breakEffect.transform.position, Quaternion.identity);
		if(isActivated&&!lockedIn) {
			ectoplasm.state = Ectoplasm.ectoState.WANDER;
			breakSeal();
		}
		yield return null;
	}

	public void breakSeal() {
		if(gameObject.activeSelf) {
			r.enabled = false;
			growingSeal.SetActive (false);
			//vformAnimator.SetBool(Animator.StringToHash("ShadowSealActive"),false);
			//vformAnimator.SetTrigger(Animator.StringToHash("Hurt"));
			ableToCharge = true;
			isActivated = false;
			effect.SetActive(false);
			breakEffect.Play();
			breakSound.Play();
			ring.transform.localScale = normalScale;
			if(!ectoplasm.purification.complete&&ectoplasm.purification.inDarkMode) Instantiate(lunabug, breakEffect.transform.position, Quaternion.identity);
			StartCoroutine (startSeal());
		}
	}

	void shrinkRing() {
		if(ectoplasm.state != Ectoplasm.ectoState.SEALED && ableToCharge) ring.transform.localScale = Vector3.Lerp(ring.transform.localScale, new Vector3(targetScale, targetScale, targetScale), Time.deltaTime*shrinkSpeed);
		//And now a delta, because floating point numbers suck
		if (ring.transform.localScale.x - targetScale < 0.5f) ring.transform.localScale = smallScale;
	}
}
