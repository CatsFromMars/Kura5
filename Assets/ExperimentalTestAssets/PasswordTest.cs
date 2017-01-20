using UnityEngine;
using System.Collections;

public class PasswordTest : MonoBehaviour {

	public BoktaiDSPassword pass;
	// Use this for initialization
	void Start () {
		BoktaiDSPassword data;
		string password = "xTDyGunFea8I4Sa7bE4rHenROavcPBcnfCv2GOnB";
		bool loaded = BoktaiDSPassword.Load(password, out data);
		Debug.Log("Password Data for: "+data.DjangoName+" and "+data.SabataName);
		Debug.Log("This password's favorite sword is: "+data.FavoriteSword);
		Debug.Log("This password's favorite gun is: "+data.FavoriteGun);
	}
}
