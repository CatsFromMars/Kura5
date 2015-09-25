using UnityEngine;
using System.Collections;

public class TextBlink : MonoBehaviour {
	
	GUIText flashingText;
	public string displayText;
	
	void Start(){
		//get the Text component
		flashingText = GetComponent<GUIText>();
		//Call coroutine BlinkText on Start
		StartCoroutine(BlinkText());
	}
	
	//function to blink the text 
	public IEnumerator BlinkText(){
		//blink it forever. You can set a terminating condition depending upon your requirement
		while(true){
			//set the Text's text to blank
			flashingText.text = displayText;
			//display blank text for 0.5 seconds
			yield return new WaitForSeconds(.5f);
			//display “I AM FLASHING TEXT” for the next 0.5 seconds
			flashingText.text = "";
			yield return new WaitForSeconds(.5f);
		}
	}
	
}