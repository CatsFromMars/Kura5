using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {
	//DESTROYS GAME OBJECT AFTER A SET TIMER. USEFUL FOR DESPAWNING BOK GOOP.

	public int destroyTime;

	private int timer;
	
	// Update is called once per frame
	void Update () {

		timer++;

		if(timer >= destroyTime) Destroy (this.gameObject);
	
	}
}
