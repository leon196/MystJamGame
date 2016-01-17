using UnityEngine;
using System.Collections;

public class Voice : Sound 
{
	public AudioClip surprise;
	public AudioClip yawn;
	public AudioClip laugh;
	public AudioClip curiosity;
	public AudioClip fear;
	public AudioClip cough;
	public AudioClip sigh;
	public AudioClip satisfaction;
	public AudioClip wow;

	void Start () {
		// audioSource.volume = 0.5f;
	}

	public void Surprise (float delay = 0f) {
		Play(surprise, delay);
	}

	public void Yawn (float delay = 0f) {
		Play(yawn, delay);
	}

	public void Laugh (float delay = 0f) {
		Play(laugh, delay);
	}

	public void Curiosity (float delay = 0f) {
		Play(curiosity, delay);
	}

	public void Fear (float delay = 0f) {
		Play(fear, delay);
	}

	public void Cough (float delay = 0f) {
		Play(cough, delay);
	}

	public void Sigh (float delay = 0f) {
		Play(sigh, delay);
	}

	public void Satisfaction (float delay = 0f) {
		Play(satisfaction, delay);
	}

	public void Wow (float delay = 0f) {
		Play(wow, delay);
	}
}