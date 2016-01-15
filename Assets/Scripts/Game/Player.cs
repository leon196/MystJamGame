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

				bool click = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(0);

				// Gate
				if (interaction.GetType() == typeof(Gate)) {
					Gate gate = (Gate)interaction;

					// Not close enough of gate
					if (gate.transitionType != Gate.TransitionType.Fade && Camera.main.fieldOfView > mouseLook.maxFOV / 2f) {
						ui.SetCursorType(Interface.CursorType.Plus);
						if (click) { 
							mouseLook.Zoom(40f); 
						}

					// Close enough of gate
					} else if (gate.isEnabled && (gate.anotherGate != null || gate.anotherWorld != null)) {

						// Sphere transition
						switch (gate.transitionType) {
							case Gate.TransitionType.Fall : ui.SetCursorType(Interface.CursorType.Portal); break;
							case Gate.TransitionType.Sphere : ui.SetCursorType(Interface.CursorType.Lock); break;
							default : ui.SetCursorType(Interface.CursorType.Step); break;
						}

						// Click on Gate
						if (click) {
							StartCoroutine(transition.Goto(gate));

							switch (gate.transitionType) {
								case Gate.TransitionType.Sphere : sound.PlaySoundPortal(); break;
								case Gate.TransitionType.Fade : sound.PlaySoundStep(); break;
							}
						}
					} 

				// Book
				} else if (interaction.GetType() == typeof(Book)) {
					// Close enough of book
					if (Camera.main.fieldOfView < mouseLook.maxFOV / 2f) {
						ui.SetCursorType(Interface.CursorType.Use);
						// Click on book
						if (click) {
							interaction.Interact();
							sound.PlaySoundPage();
						}
					// Not close enough of book
					} else {
						ui.SetCursorType(Interface.CursorType.Plus);
						// Zoom
						if (click) {
							mouseLook.Zoom(40f);
						}
					}

				// Page
				} else if (interaction.GetType() == typeof(Page)) {
					// Close enough of page
					if (Camera.main.fieldOfView < mouseLook.maxFOV / 2f) {
						ui.SetCursorType(Interface.CursorType.Use);
						// Click on page
						if (click) {
							Page page = (Page)interaction;
							StartCoroutine(page.Attach(Camera.main.transform));
							sound.PlaySoundPage();
							mouseLook.Zoom(80f);
						}
					// Not close enough of page
					} else {
						ui.SetCursorType(Interface.CursorType.Plus);
						// Zoom
						if (click) {
							mouseLook.Zoom(40f);
						}
					}

				// Default
				} else {
					ui.SetCursorType(Interface.CursorType.Use);
				}

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
