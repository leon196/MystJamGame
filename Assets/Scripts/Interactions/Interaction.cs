using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interaction : MonoBehaviour {

	public bool isEnabled = true;
	public Interaction interactionToEnable;
	[HideInInspector] public Renderer render;
	[HideInInspector] public Collider collision;
	Gate[] gateArray;

	void Awake () 
	{
		gameObject.layer = 8; // Items

		gateArray = GetComponentsInChildren<Gate>();

		render = GetComponent<Renderer>();
		if (render != null) {
			render.enabled = this.isEnabled;
		}

		collision = GetComponentInChildren<Collider>();
		if (collision != null) {
			collision.enabled = this.isEnabled;
		}
	}

	public virtual void Interact () {
		Disable();

		if (interactionToEnable != null) {
			interactionToEnable.Enable();
			interactionToEnable.Show();
		}
	}

	public void Enable () {
		this.isEnabled = true;
		if (collision != null) {
			collision.enabled = true;
		}
		for (int i = 0; i < gateArray.Length; ++i) {
			gateArray[i].isEnabled = true;
		}
	}

	public void Disable () {
		this.isEnabled = false;
		if (collision != null) {
			collision.enabled = false;
		}
		for (int i = 0; i < gateArray.Length; ++i) {
			gateArray[i].isEnabled = false;
		}
	}

	public void Show () {
		if (render != null) {
			render.enabled = true;
		}
	}

	public void Hide () {
		if (render != null) {
			render.enabled = false;
		}
	}
}
