using UnityEngine;
using System.Collections;

public class playerWeaponClass {
	public int power; //How much damage a weapon config does
	public int combo; //How many bullets can be on screen and 
	public float stun; //How long an enemy is stunned when hit by weapon
	public float speed; //How fast the animation is.
	public int eff; //How efficient a gun is
	public int energyCost; //How much it costs to use weapon
	public string element; //currentElem
	public string specialAttack; //Special attack, such as charge

	public int damage;

	public playerWeaponClass(int p, int c, float st, float spe, int eff, string e, string speAtk="None") {
		//Editable Stats
		power = p;
		combo = c;
		stun = st;
		speed = spe;
		energyCost = 12-(eff*2);

		//Subtle Stats
		damage = power * 10;
		element = e;
		specialAttack = speAtk;
	}

	public int Power {
		get {return power;}
		set {power = value;}
	}

	public int Combo {
		get {return combo;}
		set {combo = value;}
	}

	public float Stun {
		get {return stun;}
		set {stun = value;}
	}

	public float Speed {
		get {return speed;}
		set {speed = value;}
	}

	public int Efficiency {
		get {return eff;}
		set {eff = value;}
	}

	public string Element {
		get
		{
			return element;
		}
		set
		{
			element = value;
		}
	}

	public string SpecialAttack {
		get
		{
			return specialAttack;
		}
		set
		{
			specialAttack = value;
		}
	}
}
