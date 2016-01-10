using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Gate : Interaction {

	public Gate anotherGate;
	public bool isAnotherUniverse = false;

	void Start () {
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
	  	Gizmos.DrawWireSphere(transform.position, transform.localScale.x);
	  }
  }
}
