using UnityEngine;
using System.Collections;

public class GunEffectHandler : MonoBehaviour {

	public static void spawnGunHitEffect(Vector3 pos, string element, bool hiPow=false, bool hiSpeed=false) {
		//Instantiate gun effect based on element, power, and speed
		string path = "Effects/BulletHits/";
		string pow = "";
		string spd = "";
		if(hiPow) pow = "_Max";
		if(hiSpeed) spd = "_Fast";
		string bullet = element+pow+spd;
		Instantiate(Resources.Load(bullet) as GameObject, pos, Quaternion.identity);
	}
}
