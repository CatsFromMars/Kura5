using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour {
	//NOT MINE
	//TAKEN FROM http://codethegame.blogspot.com/2013/09/the-programmers-rpg.html
	//MODIFIED

	public GUISkin skin;
	public float textRate = 10; // how fast the text appears
	public AudioClip tickSound; // audio
	public int border=5, height=300; // size of dialog 
	
	float textCounter = 0;
	Texture2D leftImage, rightImage; // images
	string theText = null; // the text (null means hidden)
	Rect mainRect; // rectangle for box
	
	public bool isFinished { get { return theText==null; } }

	public void Show(string txt, Texture2D left, Texture2D right)
	{
		theText = txt;
		leftImage = left;
		rightImage = right;
		textCounter = 0;
	}
	
	public void Hide()
	{
		theText=null;
	}
	
	void Start()
	{
		// if sound add the audio
		if (tickSound!=null)
		{
			AudioSource aud=gameObject.AddComponent<AudioSource>();
			aud.clip=tickSound;
		}
		
		// put the box at bottom of screen
		float left = Screen.width/6f;
		float top = Screen.height - (Screen.height/3.3f);
		float width = Screen.width/1.5f;
		mainRect = new Rect(left, top, width, height);

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isFinished) return;
		
		int oldCounter=Mathf.FloorToInt(textCounter); // for use later in audio
		if (Input.anyKey)   // any key makes it faster
			textCounter += Time.deltaTime * textRate*4;
		else
			textCounter += Time.deltaTime * textRate;
		// tick sound when displaying
		if (tickSound!=null && textCounter < theText.Length)
		{
			if (oldCounter!=Mathf.FloorToInt(textCounter)) // if new text
			{
				audio.clip=tickSound; // make sure it stays as a tick
				if(!audio.isPlaying) audio.Play();
			}
		}
		// if finished & space bar
		if (textCounter >= theText.Length)
		{         
			if (Input.GetKeyDown(KeyCode.Space))
				Hide();
		}
	}
	
	void OnGUI()
	{
		if (isFinished) return;
		// set the skin
		GUISkin oldskin = GUI.skin;
		GUI.skin = skin;
		GUI.Box(mainRect, ""); // draw the box
		// inner rect is where we must display the text
		float left = Screen.width/4f + border;
		float top = (Screen.height - (Screen.height/5f));
		float width = Screen.width/2f - border;
		Rect innerRect = new Rect(left+border, top, width-5*border, height - 2*border);
		// draw images as needed
		if (leftImage != null)
		{
			GUI.DrawTexture(new Rect(innerRect.x, innerRect.y, leftImage.width, leftImage.height), leftImage);
			innerRect.x += leftImage.width + border;
			innerRect.width -= leftImage.width + border;
		}
		if (rightImage != null)
		{
			GUI.DrawTexture(new Rect(innerRect.xMax - rightImage.height - border, innerRect.y + (innerRect.height - rightImage.height) / 2, rightImage.width, rightImage.height), rightImage);
			innerRect.width -= rightImage.width + border * 2;
		}
		// draw the text
		if (theText != null)
		{
			theText = theText.Replace("#USER",System.Environment.UserName);
			string s = theText;
			if ((int)textCounter < theText.Length)
				s = theText.Substring(0, (int)textCounter);
			GUI.Label(innerRect, s);
		}
		// restore old skin
		GUI.skin = oldskin;
	}
}
