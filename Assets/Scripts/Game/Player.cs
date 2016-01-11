using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public World spawnWorld;
	[HideInInspector] public World currentWorld;

	RaycastHit hit;
	Interface ui;
	Vector3 rayDirection;
	Transition transition;
	Filter filter;
	Sound sound;

	void Start () {
		currentWorld = spawnWorld;
		this.transform.position = currentWorld.transform.position;
		// World[] worldArray = GameObject.FindObjectsOfType<World>();
		// for (int i = 0; i < worldArray.Length; ++i) {
		// 	World world = worldArray[i];
		// 	if (world != currentWorld) {
		// 		world.Hide();
		// 	}
		// }

		rayDirection = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
		ui = GetComponent<Interface>();
		transition = GetComponent<Transition>();
		filter = GetComponentInChildren<Filter>();
		sound = GetComponent<Sound>();
	}
	
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.H)) {
			Shader.SetGlobalTexture("_Cubemap", currentWorld.cubemap);
			filter.isEquirectangle = !filter.isEquirectangle;
		}

		rayDirection.x = Input.mousePosition.x;
		rayDirection.y = Input.mousePosition.y;
		Ray ray = Camera.main.ScreenPointToRay(rayDirection);

		if (transition.isInTransition) {
			ui.SetCursorType(Interface.CursorType.Load);

		} else if (Physics.Raycast(ray, out hit, 10f)) {
			Interaction interaction = hit.transform.GetComponent<Interaction>();

			if (interaction != null) {

				bool click = Input.GetMouseButtonDown(0);

				// Default Interaction
				if (click) {
					interaction.Interact();
					if (interaction.audioClip != null) {
						sound.Play(interaction.audioClip);
					}
					if (interaction.GetType() == typeof(Page)) {
						sound.PlaySoundPage();
					}
				}

				// Page
				if (interaction.GetType() == typeof(Page)) {
					// Page page = (Page)interaction;
					ui.SetCursorType(Interface.CursorType.Plus);

				// Door
				} else if (interaction.GetType() == typeof(Door)) {
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
					
					Gate gate = (Gate)interaction;
					if (gate.isAnotherUniverse) {
						ui.SetCursorType(Interface.CursorType.Lock);
					} else {
						ui.SetCursorType(Interface.CursorType.Step);
					}

					// Click on Gate
					if (click) {
						StartCoroutine(transition.Goto(gate));
						if (gate.isAnotherUniverse) {
							sound.PlaySoundPortal();
						} else {
							sound.PlaySoundStep();
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
