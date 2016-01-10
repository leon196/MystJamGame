using UnityEngine;
using System.Collections;

public class Page : Interaction {

	public void Close () {
		Hide();
		Disable();
	}

	public void Reset () {
		Show();
		Enable();
	}
}
