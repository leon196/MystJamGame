using UnityEngine;
using System.Collections;

public class FX : Sound 
{
	public AudioClip step;
	public AudioClip stepShoes;
	public AudioClip[] pageTakeArray;
	public AudioClip[] pageSwipeArray;
	public AudioClip pageScroll;
	public AudioClip match;
	public AudioClip bookOpen;
	public AudioClip bookFall;
	public AudioClip falling;

	void Start () {
		audioSource.volume = 1f;
	}

	public void Step (float delay = 0f) {
		Play(step, delay);
	}

	public void StepShoes (float delay = 0f) {
		Play(stepShoes, delay);
	}

	public void PageTake (float delay = 0f) {
		PlayRandom(pageTakeArray);
	}

	public void PageSwipe (float delay = 0f) {
		PlayRandom(pageSwipeArray);
	}

	public void PageScroll (float delay = 0f) {
		Play(pageScroll, delay);
	}

	public void Match (float delay = 0f) {
		Play(match, delay);
	}

	public void BookOpen (float delay = 0f) {
		Play(bookOpen, delay);
	}

	public void BookFall (float delay = 0f) {
		Play(bookFall, delay);
	}

	public void Falling (float delay = 0f) {
		Play(falling, delay);
	}
}
