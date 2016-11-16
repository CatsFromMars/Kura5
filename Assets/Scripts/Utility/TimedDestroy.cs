using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {
	//DESTROYS GAME OBJECT AFTER A SET TIMER. USEFUL FOR DESPAWNING BOK GOOP.

	public int destroyTime;
	public GameObject effect;
	private float timer;
	public int effectDelta = 20;
	private bool effectSpawned = false;
	public bool useRealTime=false;
	
	// Update is called once per frame
	void Update () {
		if(useRealTime) timer+=Time.unscaledDeltaTime;
		else timer++;
		if ((timer >= destroyTime - effectDelta) && !effectSpawned) {
			if(effect!=null) spawnEffect(effect.transform);
			effectSpawned = true;
		}
		if(timer >= destroyTime) {
			Destroy (this.gameObject);
		}
	}

	protected void spawnEffect(Transform effect) {
		Instantiate(effect, transform.position, transform.rotation);
	}
}
