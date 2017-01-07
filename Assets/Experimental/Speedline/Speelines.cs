using UnityEngine;
using System.Collections;

public class Speelines : MonoBehaviour {
	public SpriteRenderer ren;
	public Sprite[] sprites;
	// Use this for initialization
	void Start () {
		StartCoroutine (speedlines());
	}
	
	IEnumerator speedlines() {
		while (true)
		{
			foreach(Sprite s in sprites) {
				yield return new WaitForSeconds(0.05f);
				ren.sprite = s;
			}
		}
	}
}
