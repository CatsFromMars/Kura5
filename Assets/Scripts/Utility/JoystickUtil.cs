using UnityEngine;
using System.Collections;

public class JoystickUtil : MonoBehaviour {

	public static string[] getControlScheme() {
		string[] joysticks = Input.GetJoystickNames();
		string[] c;
		string[] kc = new string[]{"[W Key]","[A Key]","[S Key]","[D Key]","[Q Key]","[Arrow Keys]","[Space Bar]","[Enter Button]"};
		string[] jc = new string[]{"[LB Button]","[B Button]","[A Button]","[X Button]","[RB Button]","[Joystick]","[Back/Select Button]","[Start Button]"};
		string[] px = new string[]{"[L1 Button]","[Circle Button]","[X Button]","[Square Button]","[R1 Button]","[Joystick]","[Back/Select Button]","[Start Button]"};
		if(joysticks.Length > 0) {
			if(joysticks[0].Contains("Playstation")) c = px;
			else if (joysticks[0].Contains("Xbox")) c = jc;
			else c = jc;
		}
		else c = kc;
		return c;
	}
}
