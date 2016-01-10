using UnityEngine;
using System.Collections;

public class Page : Interaction {

	public bool isRootPage = false;
	public Page nextPage;

	public override void Interact () {
		Disable();
		
		if (nextPage) {
			nextPage.Show();
			nextPage.Enable();
		}
	}

	public void Close () {
		Hide();
		Disable();
	}

	public void Reset () {
		Show();
		Enable();
	}

  void OnDrawGizmos() {
  	if (nextPage) {
	  	Gizmos.color = Color.green;
	  	Gizmos.DrawLine(transform.position, nextPage.transform.position);
	  }
  }
}
