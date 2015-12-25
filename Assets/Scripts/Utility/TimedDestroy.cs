using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {
	//DESTROYS GAME OBJECT AFTER A SET TIMER. USEFUL FOR DESPAWNING BOK GOOP.

	public int destroyTime;
	public GameObject effect;
	private int timer;
	public int effectDelta = 20;
	private bool effectSpawned = false;
	
	// Update is called once per frame
	void Update () {

		timer++;
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
