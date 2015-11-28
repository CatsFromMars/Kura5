using System;
using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GenericPattern : MonoBehaviour {
	//public int sunriseTime = 7;
	//public int sunsetTime = 18;
	

	public static int getSeason() {
		System.DateTime date = DateTime.Now;
		float value = (float)date.Month + date.Day / 100;   // <month>.<day(2 digit)>
		if (value < 3.21 || value >= 12.22) return 3;   // Winter
		if (value < 6.21) return 0; // Spring
		if (value < 9.23) return 1; // Summer
		return 2;   // Autumn
	}

	public static int getPercent() {
		int sunriseTime = 7;
		int sunsetTime = 18;
		System.DateTime dt = DateTime.Now;
		int currentMinutes = dt.Hour * 60;
		int sunsetMinutes = sunsetTime * 60;
		int sunriseMinutes = sunriseTime * 60;
		int minutesFromSunrise = currentMinutes - sunriseMinutes;
		int lengthOfDay = sunsetMinutes - sunriseMinutes;
		float percent = 100f - (minutesFromSunrise*1.0f / lengthOfDay*1.0f)*100;

		return Mathf.RoundToInt(percent);
	}

	public static int normalize(int percentage) {
		//Returns a number between 1 and 10
		float a = Mathf.Pow(percentage-50, 2);
		float power = (-1*a/600f);
		return Mathf.RoundToInt(10*Mathf.Exp(power));
	}

	public static int getHumidity() {
		int season = getSeason ();
		if(season == 3) return Mathf.RoundToInt(UnityEngine.Random.Range (10.0F, 90.0F));
		else if(season == 0) return Mathf.RoundToInt(UnityEngine.Random.Range (20.0F, 80.0F));
		else if(season == 1) return Mathf.RoundToInt(UnityEngine.Random.Range (30.0F, 70.0F));
		else if(season == 2) return Mathf.RoundToInt(UnityEngine.Random.Range (0.0F, 80.0F));
		else return 0; //Something is wrong if it reaches here
	}

	public static int getCloudiness() {
		int season = getSeason ();
		if(season == 3) return Mathf.RoundToInt(UnityEngine.Random.Range (20.0F, 90.0F));
		else if(season == 0) return Mathf.RoundToInt(UnityEngine.Random.Range (0.0F, 85.0F));
		else if(season == 1) return Mathf.RoundToInt(UnityEngine.Random.Range (30.0F, 100.0F));
		else if(season == 2) return Mathf.RoundToInt(UnityEngine.Random.Range (15.0F, 100.0F));
		else return 0; //Something is wrong if it reaches here
	}

	public static int getSunlight(int percent, int cloudiness) {
		Debug.Log (percent);
		int sun = Mathf.RoundToInt(normalize (percent) - cloudiness/10f);
		if(sun<0) sun = 0;
		return sun;
	}

	public static int getTemp(int percent) {
		int temp = 0; //in Celsius
		int low = 0;
		int high = 0;
		System.DateTime dt = DateTime.Now;
		int month = dt.Month;
		if(month == 1) { //January
			low = -2;
			high = 10;
		}
		if(month == 2) { //Feb
			low = 0;
			high = 13;
		}
		if(month == 3) { //March
			low = 4;
			high = 17;
		}
		if(month == 4) { //April
			low = 8;
			high = 22;
		}
		if(month == 5) { //May
			low = 12;
			high = 26;
		}
		if(month == 6) { //June
			low = 18;
			high = 30;
		}
		if(month == 7) { //July
			low = 21;
			high = 33;
		}
		if(month == 8) { //August
			low = 19;
			high = 30;
		}
		if(month == 9) { //September
			low = 16;
			high = 27;
		}
		if(month == 10) { //October
			low = 9;
			high = 22;
		}
		if(month == 11) { //November
			low = 4;
			high = 17;
		}
		if(month == 12) { //December
			low = 0;
			high = 12;
		}

		//Add random variation
		low += Mathf.RoundToInt(UnityEngine.Random.Range (-2.0F, 2.0F));
		high += Mathf.RoundToInt(UnityEngine.Random.Range (-2.0F, 2.0F));

		int delta = high - low;
		float n = normalize(percent)/10f;
		return low + Mathf.RoundToInt(n*delta);
	}
}
