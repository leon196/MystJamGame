using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : Interaction {

	public bool isOpen = false;

	void Start () 
	{
		render.enabled = isOpen;
		collision.enabled = true;
		if (child) {
			child.isEnabled = false;
			child.render.enabled = false;
		}
	}

	public override void Interact () {
		if (isOpen) {
			collision.enabled = false;
			base.Interact();
		} else {
			isOpen = true;
			render.enabled = true;
		}
		
		// this.Show();

		// if (child) {
		// 	child.Show();
		// 	child.Enable();
		// }
	}
}
