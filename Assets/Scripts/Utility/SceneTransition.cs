using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
	public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
	public SpriteRenderer ren;
	public GameData data;
	public Transform playerContainer;
	private IEnumerator coroutine;
	public GameObject loadingIcon;
	private PlayerContainer player;
	private GameObject playerGO;
	public bool loadingScene = false;
	public CharacterSwapper swapper;


	void Awake ()
	{
		ren = GetComponent<SpriteRenderer>();
		// Set the texture so that it is the the size of the screen and covers it.
		//guiTexture.pixelInset = new Rect(0f, Screen.height*-1f, Screen.width, Screen.height);
	}
	
	void FadeToClear ()
	{
		// Lerp the colour of the texture between itself and transparent.
		ren.color = Color.Lerp(ren.color, Color.clear, fadeSpeed * Time.unscaledDeltaTime);
	}

	void FadeToWhite (float speed=1.5f)
	{
		// Lerp the colour of the texture between itself and black.
		ren.color = Color.Lerp(ren.color, Color.white, speed * Time.unscaledDeltaTime);
	}
	
	void FadeToBlack ()
	{
		// Lerp the colour of the texture between itself and black.
		ren.color = Color.Lerp(ren.color, Color.black, fadeSpeed * Time.unscaledDeltaTime);
	}

	public IEnumerator whiteFlash() {
		// Make sure the texture is enabled.
		ren.enabled = true;
		// Start fading towards black.
		while(ren.color.a < 0.95f) { 
			yield return null;
			FadeToWhite();
		}
		yield return CoroutineUtil.WaitForRealSeconds (0.5f);
		
		while(ren.color.a > 0.001f) {
			FadeToClear();
			yield return null;
		}
		ren.color = Color.clear;
		ren.enabled = false;
	}

	public IEnumerator fadeOut() {
		ren.enabled = true;
		while(ren.color.a < 0.95f) { 
			yield return null;
			FadeToBlack();
		}
	}

	public IEnumerator EndScene (string scene, bool stopTime=true, bool setCheckpoint = true, bool stopTimeOnLoad=false)
	{
		loadingScene = true;
		//AudioListener.volume = 0;
		if(stopTime) Time.timeScale = 0;
		playerGO = GameObject.FindWithTag ("Player");
		if(playerGO!=null){
			player = playerGO.GetComponent<PlayerContainer> ();
			player.playerInControl = false;
		}
		if(setCheckpoint) markCheckpoint (scene);

		// Make sure the texture is enabled.
		ren.enabled = true;
		if(loadingIcon!=null) loadingIcon.SetActive (true);
		// Start fading towards black.
		while(ren.color.a < 0.95f) { 
			yield return null;
			FadeToBlack();
		}

		AsyncOperation async = Application.LoadLevelAsync(scene); //Load scene
		yield return async;
		if(loadingIcon!=null) loadingIcon.SetActive (false);
		// If the texture is almost clear...
		while(ren.color.a > 0.001f) {
			FadeToClear();
			yield return null;
		}
		ren.color = Color.clear;
		ren.enabled = false;
		if(playerGO!=null) player.playerInControl = true;
		if(!stopTimeOnLoad)Time.timeScale = 1;
		loadingScene = false;
		AudioListener.volume = 1;
	}

	public void gotoScene(string scene, bool stopTime=true, bool markCheckpoint=true, bool stopTimeOnLoad=false) {
		if(coroutine != null) StopCoroutine (coroutine);
		coroutine = EndScene (scene, stopTime, markCheckpoint, stopTimeOnLoad);
		StartCoroutine (coroutine);
	}


	public IEnumerator cutsceneArrange(bool displayBothPlayers=false) {
		// Make sure the texture is enabled.
		ren.enabled = true;
		// Start fading towards black.
		while(ren.color.a < 0.95f) { 
			yield return null;
			FadeToBlack();
		}
		yield return CoroutineUtil.WaitForRealSeconds (2f);
		//make players appear next to each other
		if(displayBothPlayers) swapper.displayBoth();

		while(ren.color.a > 0.001f) {
			FadeToClear();
			yield return null;
		}
		ren.color = Color.clear;
		ren.enabled = false;
	}

	public IEnumerator deArrange() {
		
		ren.enabled = true;
		// Start fading towards black.
		while(ren.color.a < 0.95f) { 
			yield return null;
			FadeToBlack();
		}
		yield return CoroutineUtil.WaitForRealSeconds (2f);
		//make players appear next to each other
		swapper.hideInactivePlayer();
		
		while(ren.color.a > 0.001f) {
			FadeToClear();
			yield return null;
		}
		ren.color = Color.clear;
		ren.enabled = false;
	}

	public void markCheckpoint(string sceneName) {
		//Saves players location. To be loaded later.
		if(data!=null) data.sceneName = sceneName;
		if(data!=null) data.lastCheckpoint = playerContainer.position;
	}
}
