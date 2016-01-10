using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public World spawnWorld;
	[HideInInspector] public World currentWorld;

	RaycastHit hit;
	Interface ui;
	Vector3 rayDirection;
	Transition transition;

	void Start () {
		currentWorld = spawnWorld;
		this.transform.position = currentWorld.transform.position;
		rayDirection = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
		ui = GetComponent<Interface>();
		transition = GetComponent<Transition>();
	}
	
	void Update () 
	{
		rayDirection.x = Input.mousePosition.x;
		rayDirection.y = Input.mousePosition.y;
		Ray ray = Camera.main.ScreenPointToRay(rayDirection);

		if (transition.isInTransition) {
			ui.SetCursorType(Interface.CursorType.None);

		} else if (Physics.Raycast(ray, out hit, 10f)) {
			Interaction interaction = hit.transform.GetComponent<Interaction>();

			if (interaction != null) {

				bool click = Input.GetMouseButtonDown(0);

				// Default Interaction
				if (click) {
					interaction.Interact();
				}

				// Door
				if (interaction.GetType() == typeof(Door)) {
					Door door = (Door)interaction;

					// Door Open
					if (door.isOpen) {
						ui.SetCursorType(Interface.CursorType.Use);

					// Door Close
					}	else {
						ui.SetCursorType(Interface.CursorType.Plus);
					}

				// Gate
				} else if (interaction.GetType() == typeof(Gate)) {
					ui.SetCursorType(Interface.CursorType.Step);

					// Click on Gate
					if (click) {
						Gate gate = (Gate)interaction;
						StartCoroutine(transition.Goto(gate));
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
