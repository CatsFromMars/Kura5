using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

	//Portraits
	public Transform rightPortrait;
	public Transform leftPortrait;
	private SpriteRenderer rightSprite;
	private SpriteRenderer leftSprite;
	public Animator rightAnimator;
	public Animator leftAnimator;
	public GameObject canvas; //For hiding GUI
	public Text displayText;
	public GameObject displayBox;
	public GameObject arrow;

	public GUISkin skin;
	public float textRate = 10; // how fast the text appears
	public AudioClip tickSound; // audio
	public AudioClip finishSound;
	public int border=5, height=300; // size of dialog 
	
	float textCounter = 0;
	Texture2D leftImage, rightImage; // images
	string theText = null; // the text (null means hidden)
	Rect mainRect; // rectangle for box

	//audiosources
	public AudioSource tickAudio;
	public AudioSource finishAudio;
	
	public bool isFinished { get { return theText==null; } }

	void Awake() {
		AudioSource[] sources = GetComponents<AudioSource> ();
		tickAudio = sources [0];
		finishAudio = sources [1];
		leftSprite = leftPortrait.GetComponent<SpriteRenderer>();
		leftAnimator = leftPortrait.GetComponent<Animator>();
		rightSprite = rightPortrait.GetComponent<SpriteRenderer>();
		rightAnimator = rightPortrait.GetComponent<Animator>();

		// if sound add the audio
		if (tickSound!=null)
		{
			audio.clip=tickSound;
		}
	}

	public void Show(string txt, Sprite left, Sprite right)
	{
		arrow.SetActive (false);
		displayBox.SetActive (true);
		theText = txt;
		if(left != null) {
			leftSprite.sprite = left;
			if(!leftAnimator.IsInTransition(0))leftAnimator.SetTrigger(Animator.StringToHash("Transition"));
		}
		if(right != null) {
			rightSprite.sprite = right;
			if(!rightAnimator.IsInTransition(0))rightAnimator.SetTrigger(Animator.StringToHash("Transition"));
		}
		textCounter = 0;
	}
	
	public void Hide()
	{
		theText=null;
		displayText.text = theText;
		displayBox.SetActive (false);
	}

	
	// Update is called once per frame
	void Update () 
	{
		if(isFinished) {
			leftSprite.sprite = null;
			rightSprite.sprite = null;
			return;
		}
		
		int oldCounter=Mathf.FloorToInt(textCounter); // for use later in audio
		if (Input.anyKey)   // any key makes it faster

			textCounter += Time.unscaledDeltaTime * textRate*4;
		else
			textCounter += Time.unscaledDeltaTime * textRate;
		// tick sound when displaying
		if (tickSound!=null && textCounter < theText.Length)
		{
			if (oldCounter!=Mathf.FloorToInt(textCounter)) // if new text
			{
				if(!audio.isPlaying) {
					makeSound(tickAudio, tickSound);
				}
			}
		}
		// if finished & space bar
		if (textCounter >= theText.Length)
		{   
			arrow.SetActive(true);
			bool push = Input.GetButtonDown("Charge") || Input.GetButtonDown("Confirm") || Input.GetButton("Attack");
			if (push) {
				Hide();
				makeSound(finishAudio, finishSound);
			}
		}

		//Write out the text
		if (theText != null)
		{
			theText = theText.Replace("#USER",System.Environment.UserName);
			string s = theText;
			if ((int)textCounter < theText.Length)
				s = getDisplayText(theText.Substring(0, (int)textCounter));
			else s = getDisplayText(theText);
			displayText.text = s;
		}
	}

	string getDisplayText(string txt) {
		bool red = false;
		string s = "";
		bool stopNext = false;
		foreach (char letter in txt.ToCharArray()) {
			if(stopNext) {
				red=false;
				stopNext=false;
			}
			if(letter.ToString().Equals("[")) {
				red = true;
				//make the bracket itself red
			}
			if(letter.ToString().Equals("]")) {
				stopNext = true;
			}

			string section = "";

			if(red) {
				section = "<color=\"red\">"+letter+"</color>";
			}
			else section = letter.ToString();

			s += section;
		}

		return s;
	}

	public void makeSound(AudioSource s, AudioClip clip) {
		if(!audio.isPlaying) {
			s.clip = clip;
			s.Play ();
		}
	}
}
