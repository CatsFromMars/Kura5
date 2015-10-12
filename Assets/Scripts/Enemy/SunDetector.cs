using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SunDetector : MonoBehaviour {
	//used for enemies
	private int upperBound = 6;
	private int lowerBound = 0;
	public int sunlight = 0;
	public int darkness = 0;
	
	private GameObject lightSource;
	private GameObject shadowSource;
	private LightLevels l;

	private int s1;
	private int d1;
	
	void Awake() {
		l = GameObject.FindGameObjectWithTag ("LightLevels").GetComponent<LightLevels>();
		lightSource = GameObject.FindGameObjectWithTag("Sunlight");
		shadowSource = GameObject.FindGameObjectWithTag("Shadow");
	}
	
	void Update() {
		calc();
	}
	
	void calc() {
		if(l.upperBound != upperBound) upperBound = l.upperBound;
		float distanceSun = Vector3.Distance(lightSource.transform.position, transform.position);
		float distanceDark = Vector3.Distance(shadowSource.transform.position, transform.position);
		s1 = Mathf.FloorToInt(upperBound - (distanceSun / upperBound));
		d1 = Mathf.FloorToInt(upperBound - (distanceDark / upperBound));
		if(s1 > upperBound) s1=upperBound;
		if(d1 > upperBound) d1=upperBound;
		if (s1 < 0) s1 = 0;
		if (d1 < 0) d1 = 0;
		sunlight = (s1 - d1); //+ Mathf.FloorToInt(lightSource.transform.position.y/10);
		darkness = (d1 - s1); //+ Mathf.FloorToInt(shadowSource.transform.position.y/10);
		if (sunlight < 0) sunlight = 0;
		if (darkness < 0) darkness = 0;
	}
}
