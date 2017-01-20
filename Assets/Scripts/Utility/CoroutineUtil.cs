using UnityEngine;
using System.Collections;

public static class CoroutineUtil
{
	public static IEnumerator WaitForRealSeconds(float time)
	{
		float counter = 0f;
		while(counter < time) {
			counter += Time.unscaledDeltaTime;
			yield return null;
		}
		yield return null;
	}
	
	public static IEnumerator WhilePlaying( this Animation animation )
		{
			do
			{
				yield return null;
			} while ( animation.isPlaying );
		}
		
		
	public static IEnumerator WhilePlaying( this Animation animation, 
		                                       string animationName )
		{
			animation.PlayQueued(animationName);
			yield return animation.WhilePlaying();
		}
}