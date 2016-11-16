using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurificationController : MonoBehaviour {
	//Stats
	public int maxLife = 100;
	public int currentLife = 100;
	public int deathThresh = 25;
	protected int burnRate = 1;
	protected float burnCounter = 0f;
	protected float burnCounterTime = 50f;
	public bool complete = false;
	public WeatherSync w;
	public Generator[] generators;
	public bool inDarkMode = false;

	//GUI
	protected Slider lifeBar;
	protected TimedDissapear d;

	//Weather Stuff
	public int sunlight;

	//Behavior
	public bool began = false;
	private bool deathThroes = false;
	private GameData data;
	private Flags flags;

	//Visual or Audio
	public GameObject afterImage;
	public ShadowSeal shadowSeal;
	public GameObject redProjector;
	private MusicManager music;
	//public AudioClip purificationMusic;
	public AudioClip deathThroesMusic;
	public GameObject standardImageProjector;
	public GameObject throesImageProjector;
	public GameObject nightEndingCutscene;
	public GameObject dayEndingCutscene;
	public Animator ectoAnimator;
	public TextAsset dayCutscene;
	public TextAsset nightCutscene;
	public bool cutsceneEnded = false;
	public GameObject sunParticles;
	public GameObject darkParticles;
	public Projector purificationLighting;
	public AudioClip shutDown;
	public AudioClip startUp;
	public AudioClip biteSound;
	public AudioClip throesUgh;
	public GameObject burnZone;
	//public GameObject shadowZone;
	private GameObject zoneEffect;
	public GameObject[] heartParts;

	//Colors
	public Color brightLighting;
	public Color normalPurifyLighting;
	public Color dimLighting;
	public Color neutralLighting;
	public Color darkLighting;
	
	// Use this for initialization
	void Start () {
		music = GameObject.FindGameObjectWithTag ("Music").GetComponent<MusicManager>();
		redProjector.SetActive (false);
		w = GameObject.FindGameObjectWithTag ("Weather").GetComponent<WeatherSync>();
		purificationLighting.material.color = neutralLighting;
		data = GetUtil.getData();
		data.canSwapToEmil = false; //Emil's under the piledriver. Best to be Annie for the times being.
		GameObject.FindGameObjectWithTag ("PlayerSwapper").GetComponent<CharacterSwapper> ().forceSwitchToAnnie ();
		flags = GetUtil.getFlags();
		StartCoroutine(begin ());
	}

	public void takeSealDamage() {
		makeSound (biteSound);
		//sunlight and moonlight are under the same umbrella
		currentLife -= 40+(2*Mathf.RoundToInt(w.lightMax.GetValue()));
		if(currentLife <= 0 && !complete) Die();
	}

	public void takeGeneratorDamage() {
		burnCounter+=sunlight;
		if(burnCounter >= burnCounterTime&&Time.timeScale==1) {
			currentLife -= 1;
			burnCounter = 0f;
		}
		if(currentLife <= 0 && !complete) Die();
	}

	// Update is called once per frame
	void Update () {

		if(began) {
			//swap modes

			if(w.isNightTime) sunlight = 0;
			else sunlight = w.lightMax.GetValue();

			bool dark = sunlight==0 || w.isNightTime;
			bool light = sunlight > 0 && !w.isNightTime;

			if(dark&&!inDarkMode) {
				inDarkMode = true;
				StartCoroutine(switchToDark());
			}
			else if(inDarkMode&&light) {
				inDarkMode = false;
				StartCoroutine(switchToSun());
			}
			lighting();
			//Update Enemy HP Bar
			lifeBar.maxValue = maxLife;
			lifeBar.value = currentLife;

			if(!deathThroes&&currentLife<deathThresh) {
				Debug.Log("ITS HOT!");
				switchToThroes();
			}
		}
	}

	void lighting() {
		Color c;
		if(!inDarkMode) {
			if(sunlight < 3) c=dimLighting;
			else if(sunlight > 6) c=brightLighting;
			else c=normalPurifyLighting;
			purificationLighting.material.color = c;

			if(allGeneratorsDeActivated()) {
				burnZone.SetActive(false);
				purificationLighting.material.color = neutralLighting;
				shadowSeal.growingSeal.SetActive(true);
			}
			else {
				burnZone.SetActive(true);
				shadowSeal.growingSeal.SetActive(false);
			}
		}
	}

	bool allGeneratorsDeActivated() {
		foreach(Generator g in generators) {
			if(g.state == Generator.generatorState.ACTIVATED) return false;
			burnZone.SetActive(true);
		}
		return true;
		burnZone.SetActive(false);
	}

	void Die() {
		complete = true;
		d.enabled = true;
		Debug.Log ("PURIFICATION COMPLETE!");
		StartCoroutine(Complete());
	}

	IEnumerator Complete() {
		//if (!inDarkMode) afterImage.SetActive (true);
		ectoAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
		CameraFlash.flashCamera();
		music.stopMusic();
		GameObject effect = Resources.Load("Effects/PurificationComplete") as GameObject;
		Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane+1)); 
		GameObject e = Instantiate(effect, pos, Quaternion.identity) as GameObject;
		e.transform.parent = Camera.main.transform;
		data.canSwapToEmil = true;
		if(inDarkMode) {
			//flags.setQuickFlag("LaLupesHeartEaten");
			nightEndingCutscene.SetActive(true);
		}
		else {
			dayEndingCutscene.SetActive(true);
		}
		yield return null;
	}

	void switchToThroes() {
		deathThroes = true;
		StartCoroutine(throes());
	}

	public void toggleGenerators(Generator.generatorState state) {
		foreach(Generator g in generators) {
			g.state = state;
		}
	}

	public IEnumerator begin(){
		while(Time.timeScale==0) yield return null;
		yield return new WaitForSeconds (0.5f);
		sunlight = w.lightMax.GetValue();
		if(w.isNightTime || sunlight == 0) inDarkMode=true;
		else inDarkMode = false;
		Debug.Log ("Begin: "+sunlight);

		if(!began) {
			while(Time.timeScale==0) yield return null;
			lifeBar = GameObject.Find ("EnemyLife").GetComponent<Slider>();
			d=lifeBar.transform.FindChild("EnemyLifebarEmpty").GetComponent<TimedDissapear>();
			d.enabled = false;
			yield return new WaitForSeconds (0.2f);
			if(inDarkMode) yield return StartCoroutine(switchToDark());
			else yield return StartCoroutine(switchToSun());
		}
	}

	IEnumerator switchToSun() {
		makeSound (startUp);
		CameraFlash.flashCamera();
		if(dayCutscene!=null&&!cutsceneEnded) yield return StartCoroutine(DisplayDialogue.Speak(dayCutscene));
		cutsceneEnded = true;
		//shadowSeal.gameObject.SetActive (false);
		toggleGenerators (Generator.generatorState.ACTIVATED);
		yield return new WaitForSeconds (0.5f);
		darkParticles.SetActive (inDarkMode);
		burnZone.SetActive (!inDarkMode);
		sunParticles.SetActive (!inDarkMode);
		lighting ();
		began = true;
	}

	IEnumerator switchToDark() {
		Debug.Log ("Switching to Dark");
		makeSound (shutDown);
		CameraFlash.flashCamera();
		//shadowSeal.gameObject.SetActive (true);
		shadowSeal.ring.transform.localScale = shadowSeal.smallScale;
		purificationLighting.material.color = darkLighting;
		darkParticles.SetActive (inDarkMode);
		burnZone.SetActive (!inDarkMode);
		sunParticles.SetActive (!inDarkMode);
		if(nightCutscene!=null&&!cutsceneEnded) yield return StartCoroutine(DisplayDialogue.Speak(nightCutscene));
		cutsceneEnded = true;
		began = true;
	}

	IEnumerator throes() {
		CameraFlash.flashCamera();
		music.stopMusic();
		makeSound(throesUgh);
		yield return new WaitForSeconds (0.4f);
		music.changeMusic(deathThroesMusic);
		music.startMusic ();
		redProjector.SetActive(true);
		throesImageProjector.SetActive (true);
		standardImageProjector.SetActive (false);
	}

	void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
	}
}
