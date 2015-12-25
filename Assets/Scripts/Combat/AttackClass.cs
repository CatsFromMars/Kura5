using UnityEngine;
using System.Collections;
using System;

public class Attack {

	public int baseDamage;
	public Element element;
	public float multiplier = 1f;
	public bool superEffective = false;

	public Attack(int attackBase, Element attackElement, float attackMultiplier){
		baseDamage = attackBase;
		element = attackElement;
		multiplier = attackMultiplier;
	}

	public int calculateDamage(Element self) {
		int damage = baseDamage;
		if (element.name == self.opposite) {
			superEffective = true;
			damage = Mathf.RoundToInt(damage*1.5f);
		}
		else if (element.name == self.name) damage = Mathf.RoundToInt(damage/1.5f);
		damage = Mathf.RoundToInt(damage * multiplier);
		return damage;
	}

}
