using UnityEngine;
using System.Collections;

public class GameOverHandler : MonoBehaviour {

	public GameData data;
	public GameObject HUD;
	public SceneTransition sceneManager;
	public PlayerContainer annie;
	public PlayerContainer emil;
	public MusicManager music;
	private AudioClip priorMusic;

	public void Update() {
		//Check for Game Over
		bool a = data.annieCurrentLife <= 0 || !data.canSwapToAnnie;
		bool e = data.emilCurrentLife <= 0 || !data.canSwapToEmil;
		if (a && e && !data.isGameOver) {
			setGameOver();
		}
	}

	public void setGameOver() {
		data.isGameOver = true;
		HUD.SetActive (false);
		StartCoroutine(go());
	}

	IEnumerator go() {
		//Instantiate(Resources.Load ("Effects/GameOver"), gameOverSpawner.position, Quaternion.Euler(45,0,0));
		priorMusic = music.audio.clip;
		music.stopMusic();
		yield return sceneManager.fadeOut();
		Application.LoadLevel("GameOver");
	}
	
	public void Restart() {
		data.isGameOver = false;
		data.annieCurrentLife = data.annieMaxLife;
		data.annieCurrentEnergy = data.annieMaxEnergy;
		data.emilCurrentLife = data.emilMaxLife;
		data.emilCurrentEnergy = data.emilMaxEnergy;
		annie.revive();
		emil.revive();
		GameObject.FindGameObjectWithTag ("Player").transform.position = data.lastCheckpoint;
		sceneManager.gotoScene (data.sceneName, true, false);
		music.changeMusic(priorMusic);
		HUD.SetActive (true);
	}
}
