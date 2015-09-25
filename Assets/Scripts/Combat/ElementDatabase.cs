using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementDatabase : MonoBehaviour {

	public Element Null;
	public Element Sol;
	public Element Dark;
	public Element Cloud;
	public Element Earth;
	public Element Frost;
	public Element Fire;

	// Use this for initialization
	void Awake() {
		Null = (new Element(0, "Null", "NA", "NA"));
		Sol = (new Element(1, "Sol", "Purify", "Dark"));
		Dark = (new Element(2, "Dark", "Stun", "Sol"));
		Cloud = (new Element(3, "Cloud", "Confuse", "Earth"));
		Earth= (new Element(4, "Earth", "Poison", "Cloud"));
		Fire = (new Element(5, "Fire", "Burn", "Frost"));
		Frost = (new Element(6, "Frost", "Freeze", "Fire"));
	}
}
