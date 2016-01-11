using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {
	public AudioClip soundPage;
	public AudioClip soundStep;
	public AudioClip soundPortal;
	AudioSource audioSource;

	void Start () {
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.loop = false;
		audioSource.spatialBlend = 0;
	}

	public void Play (AudioClip clip) {
		audioSource.clip = clip;
		audioSource.Play();
	}

	public void PlaySoundStep () {
		audioSource.clip = soundStep;
		audioSource.Play();
	}

	public void PlaySoundPage () {
		audioSource.clip = soundPage;
		audioSource.Play();
	}

	public void PlaySoundPortal () {
		audioSource.clip = soundPortal;
		audioSource.Play();
	}
}