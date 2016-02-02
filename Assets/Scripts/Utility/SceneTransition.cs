using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
	public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
	public SpriteRenderer ren;
	public GameData data;
	public Transform playerContainer;
	private IEnumerator coroutine;

	void Awake ()
	{
		ren = GetComponent<SpriteRenderer>();
		// Set the texture so that it is the the size of the screen and covers it.
		//guiTexture.pixelInset = new Rect(0f, Screen.height*-1f, Screen.width, Screen.height);
	}
	
	void FadeToClear ()
	{
		// Lerp the colour of the texture between itself and transparent.
		ren.color = Color.Lerp(ren.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	
	void FadeToBlack ()
	{
		// Lerp the colour of the texture between itself and black.
		ren.color = Color.Lerp(ren.color, Color.black, fadeSpeed * Time.deltaTime);
	}

	IEnumerator EndScene (string scene)
	{
		markCheckpoint (scene);

		// Make sure the texture is enabled.
		ren.enabled = true;
		
		// Start fading towards black.
		while(ren.color.a < 0.95f) { 
			yield return null;
			FadeToBlack();
		}

		Application.LoadLevel(scene); //Load scene
		yield return new WaitForSeconds (0.3f);
		// If the texture is almost clear...
		while(ren.color.a > 0.001f) {
			FadeToClear();
			yield return null;
		}
		ren.color = Color.clear;
		ren.enabled = false;
	}

	public void gotoScene(string scene) {
		if(coroutine != null) StopCoroutine (coroutine);
		coroutine = EndScene (scene);
		StartCoroutine (coroutine);
	}

	public void markCheckpoint(string sceneName) {
		//Saves players location. To be loaded later.
		if(data!=null) data.sceneName = sceneName;
		if(data!=null) data.lastCheckpoint = playerContainer.position;
	}
}
