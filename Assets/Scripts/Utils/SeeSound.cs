using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SeeSound : MonoBehaviour 
{
	AudioSource[] audioSourceArray;

	void OnEnable ()
	{
		audioSourceArray = GameObject.FindObjectsOfType<AudioSource>();
	}

  void OnDrawGizmos() 
  {
  	if (audioSourceArray != null) {
			Gizmos.color = Color.yellow;
			for (int i = 0; i < audioSourceArray.Length; ++i) {
				AudioSource audioSource = audioSourceArray[i];
				if (audioSource != null) {
					Gizmos.DrawWireSphere(audioSource.transform.position, audioSource.maxDistance);
				}
			}
		}
	}
}