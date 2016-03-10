using UnityEngine;
using System.Collections;

public class Broadcaster : MonoBehaviour {

	public static void BroadcastAll(string fun, System.Object msg=null) {
		GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in gos) {
			if (go && go.transform.parent == null) {
				go.gameObject.BroadcastMessage(fun, msg, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
