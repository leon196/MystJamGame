using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public World spawnWorld;
	[HideInInspector] public World currentWorld;

	RaycastHit hit;
	Interface ui;
	Vector3 rayDirection;
	
	MouseLook mouseLook;
	Transition transition;
	Sound sound;

	void Start () 
	{
		currentWorld = spawnWorld;
		this.transform.position = currentWorld.transform.position;
		rayDirection = new Vector3(Input.mousePosition.x, Input.mousePosition.y);

		ui = GetComponent<Interface>();
		
		transition = GameObject.FindObjectOfType<Transition>();
		mouseLook = GameObject.FindObjectOfType<MouseLook>();
		sound = GameObject.FindObjectOfType<Sound>();
	}
	
	void Update () 
	{
		rayDirection.x = Input.mousePosition.x;
		rayDirection.y = Input.mousePosition.y;
		Ray ray = Camera.main.ScreenPointToRay(rayDirection);

		if (transition.isInTransition) {
			ui.SetCursorType(Interface.CursorType.Load);

		} else if (Physics.Raycast(ray, out hit, 10f)) {
			Interaction interaction = hit.transform.GetComponent<Interaction>();

			if (interaction != null) {

				bool click = Input.GetMouseButtonDown(0);

				// Close enough of interaction
				// if (Camera.main.fieldOfView < mouseLook.maxFOV / 2f) {

					// Page
					if (interaction.GetType() == typeof(Page)) {
						// Close enough of page
						if (Camera.main.fieldOfView < mouseLook.maxFOV / 2f) {
							ui.SetCursorType(Interface.CursorType.Use);
							// Click on page
							if (click) {
								interaction.Interact();
								sound.PlaySoundPage();
							}
						// Not close enough of page
						} else {
							ui.SetCursorType(Interface.CursorType.Plus);
							// Zoom
							if (click) {
								mouseLook.Zoom(40f);
							}
						}

					// Gate
					} else if (interaction.GetType() == typeof(Gate)) {
						Gate gate = (Gate)interaction;

						// Not close enough of gate
						if (gate.transitionType == Gate.TransitionType.Sphere && Camera.main.fieldOfView > mouseLook.maxFOV / 2f) {
							ui.SetCursorType(Interface.CursorType.Plus);
							if (click) { 
								mouseLook.Zoom(40f); 
							}

						// Close enough of gate
						} else {
							// Sphere transition
							if (gate.transitionType == Gate.TransitionType.Sphere) {
								ui.SetCursorType(Interface.CursorType.Lock);
							} else {
								ui.SetCursorType(Interface.CursorType.Step);
							}

							// Click on Gate
							if (click) {
								StartCoroutine(transition.Goto(gate));
								// Sound
								if (gate.transitionType == Gate.TransitionType.Sphere) {
									sound.PlaySoundPortal();
								} else {
									sound.PlaySoundStep();
								}
							}
						} 

					// Portal
					} else if (interaction.GetType() == typeof(Portal)) {

						// Close enough of portal
						if (Camera.main.fieldOfView < mouseLook.maxFOV / 2f) {
							Portal portal = (Portal)interaction;
							if (portal.transitionType == Gate.TransitionType.Sphere) {
								ui.SetCursorType(Interface.CursorType.Lock);
							} else {
								ui.SetCursorType(Interface.CursorType.Portal);
							}
							// Click on Portal
							if (click) {
								StartCoroutine(transition.Fall(portal));
							}
						// Not close enough of portal
						} else {
							if (click) {
								mouseLook.Zoom(30f);
							}
							ui.SetCursorType(Interface.CursorType.Plus);
						}
					// Default
					} else {
						ui.SetCursorType(Interface.CursorType.Use);
					}

				// Not close enough of interaction
				// } else {
				// 	ui.SetCursorType(Interface.CursorType.Look);
					// Click to Zoom
				// 	if (click) {
				// 		mouseLook.Zoom();
				// 	}
				// }

			// No interaction
			} else {
				ui.SetCursorType(Interface.CursorType.Look);
			}

		// No interaction and no collision
		} else {
			ui.SetCursorType(Interface.CursorType.Look);
		}
	}
}
