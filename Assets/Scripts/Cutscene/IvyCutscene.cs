using UnityEngine;
using System.Collections;

public class IvyCutscene : MonoBehaviour {
	private bool playedIntroCutscene = false;
	private bool playedTrapCutscene = false;
	private bool playedIvyCutscene = false;
	private bool activatedIvy = false;
	private bool ivyDied = false;
	private bool startedBossFight = false;
	private Flags flags;

	public Transform inisibleWall;
	private PlayerContainer player;
	public Vaquero vaquero;
	public Transform walkTo1;
	public Transform walkTo2;
	public Trap trap;
	public Animator[] ivies;
	public GameObject[] ivyObjects;
	public Animator emil;
	private GameObject deadIvy;

	public TextAsset introCutscene;
	public TextAsset trapCutscene;
	public TextAsset ivyCutscene;

	void Start() {
		flags = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Flags>();
		emil.SetInteger (Animator.StringToHash("CutsceneAction"), 5); //Have Emil hide himself
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContainer>();
		StartCoroutine (startTrapCutscene());
		playedIntroCutscene = true;

	}
	// Update is called once per frame
	void Update () {
		if (!activatedIvy && trap.isTrapClear()) {
			setIviesActive();
			activatedIvy = true;
		}
		if(activatedIvy && !playedIvyCutscene && checkForIvyKill()) {
			playedIvyCutscene = true;
			StartCoroutine(startIvyCutscene()); //"What the heck is this stuff?!"
		}
		if(!startedBossFight&&trap.isTrapActivated()&&Time.timeScale == 1&&vaquero.appeared) {
			startedBossFight=true;
			vaquero.StartBossFight();
		}
	}

	void setIviesActive() {
		foreach(GameObject ivy in ivyObjects) {
			ivy.SetActive(true);
		}
	}

	bool checkForIvyKill() {
		foreach(Animator ivy in ivies) {
			if(ivy.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.Dying")) {
				ivy.transform.root.gameObject.name = "LookIvy";
				deadIvy = ivy.gameObject;
				return true;
			}
			//if(ivy.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base Layer.Revive") && ivyDied) return true;
		}
		return false;
	}

	IEnumerator startIvyCutscene() {
		yield return new WaitForSeconds(1);
		yield return StartCoroutine(DisplayDialogue.Speak(ivyCutscene));
		Destroy (deadIvy.transform.root.gameObject);
		flags.SetCutscene(ivyCutscene.name);
	}

	IEnumerator startTrapCutscene() {
		//be sure to disable trap trigger in-scene.
		yield return StartCoroutine(player.characterWalkTo(walkTo1.position));
		yield return StartCoroutine(DisplayDialogue.Speak(introCutscene)); //"Wait!"
		yield return StartCoroutine(player.characterWalkTo(walkTo2.position));
		emil.SetInteger (Animator.StringToHash("CutsceneAction"),8); //Emil appears
		trap.springTrap(player.transform);
	}
}
