using UnityEngine;
using System.Collections;

public class Book : Interaction {

	public bool isRootPage = false;
	public Interaction nextPage;

	void Start ()
	{
		if (!isRootPage) {
			Hide();
			Disable();
		}
		if (nextPage != null) {
			nextPage.Hide();
			nextPage.Disable();
		}
	}

	public override void Interact () {
		Disable();
		
		if (nextPage) {
			nextPage.Show();
			nextPage.Enable();
		}
	}

	public void Close () {
		if (isRootPage == false) {
			Hide();
			Disable();
		} else {
			Show();
			Enable();
		}
	}

  void OnDrawGizmos() {
  	if (nextPage) {
	  	Gizmos.color = Color.green;
	  	Gizmos.DrawLine(transform.position, nextPage.transform.position);
	  }
  }
}
