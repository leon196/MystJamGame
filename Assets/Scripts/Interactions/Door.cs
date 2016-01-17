using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : Interaction {

	public bool isOpen = false;

	void Start () 
	{
		render.enabled = isOpen;
		collision.enabled = true;
	}

	public override void Interact () {
		if (isOpen) {
			collision.enabled = false;
			base.Interact();
		} else {
			isOpen = true;
			render.enabled = true;
		}
	}
}
