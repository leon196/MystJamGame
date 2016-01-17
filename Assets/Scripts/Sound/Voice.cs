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

	public void Surprise () {
		Play(surprise);
	}

	public void Yawn () {
		Play(yawn);
	}

	public void Laugh () {
		Play(laugh);
	}

	public void Curiosity () {
		Play(curiosity);
	}

	public void Fear () {
		Play(fear);
	}

	public void Cough () {
		Play(cough);
	}

	public void Sigh () {
		Play(sigh);
	}

	public void Satisfaction () {
		Play(satisfaction);
	}
}