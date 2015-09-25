using UnityEngine;
using System.Collections;
using System;

public class Attack {

	public int baseDamage;
	public Element element;
	public int multiplier = 1;

	public Attack(int attackBase, Element attackElement, int attackMultiplier){
		baseDamage = attackBase;
		element = attackElement;
		multiplier = attackMultiplier;
	}

	public int calculateDamage(Element self) {
		int damage = baseDamage;
		if (element.name == self.opposite) damage *= 2;
		damage *= multiplier;
		return damage;
	}

}
