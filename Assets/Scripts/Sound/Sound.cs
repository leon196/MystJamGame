using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

	AudioSource audioSource;

	void Start () {
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.loop = false;
		audioSource.spatialBlend = 0;
	}

	public void Play (AudioClip clip, float delay = 0f) {
		audioSource.clip = clip;
		audioSource.PlayDelayed(delay);
	}

	public void PlayRandom (AudioClip[] clipArray) {
		Play(clipArray[(int)Mathf.Floor(Random.Range(0f, clipArray.Length))]);
	}
}