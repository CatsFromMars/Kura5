using UnityEngine;
using System.Collections;

public class Switch : Activatable {
	public bool setFlag;
	public string flag;
	private Flags flags;

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "Bullet") {
			if(setFlag) {
				Debug.Log("Set flag at: "+flag);
				flags.SetOther(flag);
			}
			if(audio!=null) audio.Play();
			Activate();
		}
	}
}
