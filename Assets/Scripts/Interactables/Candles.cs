using UnityEngine;
using System.Collections;

public class Candles : MonoBehaviour {
	public ParticleSystem candle1;
	public ParticleSystem candle2;
	public ParticleSystem candle3;
	public Light light1;
	public Light light2;
	public Light light3;
	public bool candlesLit = false;

	// Update is called once per frame
	void Update () {
		if(candlesLit) {
			candle1.Play ();
			candle2.Play ();
			candle3.Play ();
			light1.intensity = 8;
			light2.intensity = 8;
			light3.intensity = 8;
		}
		else {
			candle1.Stop ();
			candle2.Stop ();
			candle3.Stop ();
			light1.intensity = 0;
			light2.intensity = 0;
			light3.intensity = 0;
		}
	}

	public void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "FireBullet") {
			if(!candlesLit) {
				audio.Play();
				candlesLit = true;
			}
		}
		
		
	}
	
}
