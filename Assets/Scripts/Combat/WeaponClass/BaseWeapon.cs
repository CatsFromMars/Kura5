using UnityEngine;
using System.Collections;

public class BaseWeapon {
	public string name;
	public int power;
	public int maxCombo;
	public int consumption;
	//AudioClip fireSound;
	AudioClip hitSound;
	//Add animation laaater

	public BaseWeapon(string weaponName, int weaponPower, int weaponCombo, int weaponConsumption, AudioClip weaponSound, AudioClip weaponHit){
		name = weaponName;
		power = weaponPower;
		maxCombo = weaponCombo;
		consumption = weaponConsumption;
		//fireSound = weaponSound;
	}
}
