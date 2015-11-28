using System;
using UnityEngine;
using System.Collections;

public class MoonPhase : MonoBehaviour {

	//Taken from 
	public static int calculatePhase(int year,int month,int day)
	{
		//15 = full
		//0 = new moon
		DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
		double now = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		double newmoon = (new DateTime(1970, 1, 7, 20, 35, 0) - epochStart).TotalMilliseconds;
		int lp = 2551443;
		float difference = (float)(now - newmoon);
		float phase = (difference/1000) % lp;
		int result = Mathf.RoundToInt(phase /(24*3600)) + 1;
		return result;
	}

	public static int moonlightPercent(DateTime date) {
		//returns the decimal percent of moonlight illumination, with a full moon being 100%.
		//Assumes no clouds.
		int year = date.Year;
		int month = date.Month;
		int day = date.Day;
		int phase = calculatePhase (year,month,day);
		float delta = 0;
		if(phase > 15) {
			phase = 30-phase;
		}
		Debug.Log ("Moonphase = "+phase);
		return Mathf.RoundToInt((phase / 15f)*10);
	}

}
