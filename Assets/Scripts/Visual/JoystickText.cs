using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JoystickText : MonoBehaviour {
	public int controlSchemeIndex = 0;
	public string addOnString;
	public enum useType {TEXTMESH, UITEXT}
	public useType type = useType.UITEXT;
	// Use this for initialization
	void Start () {
		string[] c = JoystickUtil.getControlScheme();
		if(type == useType.UITEXT) GetComponent<Text>().text = c[controlSchemeIndex]+addOnString;
		else GetComponent<TextMesh>().text = c[controlSchemeIndex]+addOnString;
	}
}
