using UnityEngine;
using System.Collections;

public class Gate : Interaction {

	public Gate anotherGate;
	public bool isAnotherUniverse = false;

	void Start () {
	}

	public override void Interact () {
	}

  void OnDrawGizmos() {
  	if (anotherGate) {
	  	Gizmos.color = Color.red;
	  	Vector3 midPos = Vector3.Lerp(transform.position, anotherGate.transform.position, 0.5f);
	  	Gizmos.DrawLine(transform.position, midPos);
	  	// Gizmos.color = Color.blue;
	  	// Gizmos.DrawLine(midPos, anotherGate.transform.position);
	  } else {
	  	Gizmos.color = Color.blue;
	  }
	  Gizmos.DrawWireSphere(transform.position, transform.localScale.x);
  }
}
