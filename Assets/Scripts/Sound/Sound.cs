using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

	protected AudioSource audioSource;

	void Awake () {
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.loop = false;
		audioSource.spatialBlend = 0;
	}

	public void Play (AudioClip clip, float delay = 0f) {
		if (delay == 0f) {
			audioSource.clip = clip;
			audioSource.Play();
		} else {
			StartCoroutine(PlayDelayed(clip, delay));
		}
	}

	IEnumerator PlayDelayed (AudioClip clip, float delay = 0f) {
		float t = 0f;
		while (t < delay) {
			t += Time.deltaTime;
			yield return 0;
		}
		audioSource.clip = clip;
		audioSource.Play();
	}

	public void PlayRandom (AudioClip[] clipArray) {
		Play(clipArray[(int)Mathf.Floor(Random.Range(0f, clipArray.Length))]);
	}
}