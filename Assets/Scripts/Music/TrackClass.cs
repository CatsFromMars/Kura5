using UnityEngine;
using System.Collections;
using System;

public class Track : IComparable<Track> {
	//Music Track Class
	public int trackNumber;
	public string trackName;
	public AudioClip trackIntro;
	public AudioClip trackMain;

	public Track(int number, string name, AudioClip intro, AudioClip main) {
		trackNumber = number;
		trackName = name;
		trackIntro = intro;
		trackMain = main;
	}

	public int CompareTo(Track other)
	{
		if(other == null)
		{
			return 1;	
		}
		//SORTS BY ID NUMBER
		return trackNumber - other.trackNumber;
		
	}
}
