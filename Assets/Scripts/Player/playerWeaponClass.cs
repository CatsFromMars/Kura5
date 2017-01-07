using UnityEngine;
using System.Collections;

public class playerWeaponClass {
	public int power; //How much damage a weapon config does
	public int combo; //How many bullets can be on screen and 
	public float stun; //How long an enemy is stunned when hit by weapon
	public float speed; //How fast the animation is.
	public int energyCost; //How much it costs to use weapon
	public string element; //currentElem
	public string specialAttack; //Special attack, such as charge

	public int damage;

	public playerWeaponClass(int p, int c, float st, float spe, int eff, string e, string speAtk="None") {
		power = p;
		combo = c;
		stun = st;
		speed = spe;
		energyCost = 12-(eff*2);
		damage = power * 10;
		element = e;
		specialAttack = speAtk;
	}

	public string Element {
		get
		{
			//Some other code
			return element;
		}
		set
		{
			//Some other code
			element = value;
		}
	}

	public string SpecialAttack {
		get
		{
			//Some other code
			return specialAttack;
		}
		set
		{
			//Some other code
			specialAttack = value;
		}
	}
}
