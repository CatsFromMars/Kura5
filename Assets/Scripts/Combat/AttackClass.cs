using UnityEngine;
using System.Collections;
using System;

public class Attack {

	public int baseDamage;
	public Element element;
	public float multiplier = 1f;

	public Attack(int attackBase, Element attackElement, float attackMultiplier){
		baseDamage = attackBase;
		element = attackElement;
		multiplier = attackMultiplier;
	}

	public int calculateDamage(Element self) {
		int damage = baseDamage;
		if (element.name == self.opposite) damage *= 2;
		else if (element.name == self.name) damage = Mathf.CeilToInt(damage/2f);
		damage = Mathf.FloorToInt(damage * multiplier);
		return damage;
	}

}
