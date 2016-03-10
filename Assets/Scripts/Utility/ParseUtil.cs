using UnityEngine;
using System.Collections;

public class ParseUtil : MonoBehaviour {

	public static int numberParse(string s) {
		float temp;
		float.TryParse(s, out temp); //parse the temperature
		return Mathf.RoundToInt(temp);
	}
}
