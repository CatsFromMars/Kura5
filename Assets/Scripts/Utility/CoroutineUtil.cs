using UnityEngine;
using System.Collections;

public static class CoroutineUtil
{
	public static IEnumerator WaitForRealSeconds(float time)
	{
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time)
		{
			yield return null;
		}
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