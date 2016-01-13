using UnityEngine;
using System.Collections;

public class Gate : Interaction {

	public Gate anotherGate;
	public enum TransitionType { Fade, Sphere, Fall };
	public TransitionType transitionType = TransitionType.Fade;

	void Start () {
	}

	public override void Interact () {
	}

  void OnDrawGizmos() {
  	if (anotherGate) {
	  	Gizmos.color = Color.red;
	  	if (anotherGate.anotherGate) {
	  		Gizmos.DrawLine(transform.position, Vector3.Lerp(transform.position, anotherGate.transform.position, 0.5f));
  		} else {
	  		Gizmos.DrawLine(transform.position, anotherGate.transform.position);
  		}
	  	// Gizmos.color = Color.blue;
	  	// Gizmos.DrawLine(midPos, anotherGate.transform.position);
	  } else {
	  	Gizmos.color = Color.blue;
	  }
	  Gizmos.DrawWireSphere(transform.position, transform.localScale.x * 0.2f);
  }
}
