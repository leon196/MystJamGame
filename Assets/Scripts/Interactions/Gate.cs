using UnityEngine;
using System.Collections;

public class Gate : Interaction {

	public Gate anotherGate;

	void Start () {
		if (render) {
			render.enabled = false;
		}
		if (anotherGate) {
			if (anotherGate.anotherGate == null) {
				anotherGate.anotherGate = this;
			}
		}
	}

	public override void Interact () {
	}

  void OnDrawGizmos() {
  	if (anotherGate) {
	  	Gizmos.color = Color.red;
	  	Gizmos.DrawLine(transform.position, anotherGate.transform.position);
	  }
  }
}
