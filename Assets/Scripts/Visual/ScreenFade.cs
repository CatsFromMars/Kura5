using UnityEngine;
using System.Collections;
//TAKEN FROM UNITY TUTORIAL WEBSITE.

public class ScreenFade : MonoBehaviour
{
	public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
	
	
	private bool sceneStarting = true;      // Whether or not the scene is still fading in.
	private bool sceneEnding = false;
	
	
	void Start ()
	{
		// Set the texture so that it is the the size of the screen and covers it.
		guiTexture.pixelInset = new Rect(Screen.width/-2f, Screen.height/-2f, Screen.width, Screen.height);
		this.transform.position = new Vector3(0.5f, 0.5f, 0);
	}
	
	
	void Update ()
	{
		// If the scene is starting...
		if(sceneStarting && !sceneEnding) StartScene();
	}
	
	
	void FadeToClear ()
	{
		// Lerp the colour of the texture between itself and transparent.
		guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	
	void FadeToBlack()
	{
		// Lerp the colour of the texture between itself and black.
		guiTexture.color = Color.Lerp(guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}
	
	
	void StartScene ()
	{
		// Fade the texture to clear.
		FadeToClear();
		
		// If the texture is almost clear...
		if(guiTexture.color.a <= 0.05f)
		{
			// ... set the colour to clear and disable the GUITexture.
			guiTexture.color = Color.clear;
			guiTexture.enabled = false;
			
			// The scene is no longer starting.
			sceneStarting = false;
		}
	}
	
	
	public void EndScene(string scene)
	{
		sceneEnding = true;
		// Make sure the texture is enabled.
		guiTexture.enabled = true;

		FadeToBlack();

		// If the screen is almost black...
		if(guiTexture.color.a >= 0.95f) {
			Application.LoadLevel(scene);
			sceneEnding = false;
		}
	}
}