using UnityEngine;
using System.Collections;

public class weatherTemplateSet : MonoBehaviour {
	public int weatherTemplate = 0;
	public TextAsset cutsceneClearFlag;
	// Use this for initialization
	void Start () {
		if(!GetUtil.getFlags().CheckCutsceneFlag(cutsceneClearFlag.name)) {
			GetUtil.getWeather().setWeatherTemplate (weatherTemplate);
		}
		else GetUtil.getWeather().setWeatherTemplate (0);
	}
}
