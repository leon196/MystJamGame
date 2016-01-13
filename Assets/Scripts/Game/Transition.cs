using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour {

	Player player;
	MouseLook mouseLook;
	Interface ui;

	FilterUI filterUI;
	FilterPlanet filterPlanet;
	FilterEquirectangle filterEquirectangle;

	[HideInInspector] public bool isInTransition = false;
	[HideInInspector] public float transitionRatio = 0f;
	[HideInInspector] public float transitionTime = 0f;

	void Start () 
	{
		player = GameObject.FindObjectOfType<Player>();
		mouseLook = GameObject.FindObjectOfType<MouseLook>();
		ui = GameObject.FindObjectOfType<Interface>();

		filterUI = GameObject.FindObjectOfType<FilterUI>();
		filterPlanet = GameObject.FindObjectOfType<FilterPlanet>();
		filterEquirectangle = GameObject.FindObjectOfType<FilterEquirectangle>();

		Shader.SetGlobalFloat("_InterpolationRatio", 0f);
		Shader.SetGlobalFloat("_IsUniverseTransition", 0f);
		Shader.SetGlobalVector("_HoleDirection", Vector3.up);
	}

	// void Update () 
	// {
	// 	if (Input.GetKeyDown(KeyCode.H)) {
	// 		Shader.SetGlobalTexture("_Cubemap", currentWorld.cubemap);
	// 		filterEquirectangle.enabled = !filterEquirectangle.enabled;
	// 	}
	// }

	public IEnumerator Goto (Gate gate) {

		World world = null;
		
		if (gate.anotherGate) {
			world = gate.anotherGate.GetComponentInParent<World>();
		}

		if (world != null) {

			// Start

			isInTransition = true;
			player.currentWorld.SetupMaterial(world);
			Shader.SetGlobalVector("_HoleDirection", -Camera.main.transform.forward);
			Shader.SetGlobalTexture("_Cubemap", world.cubemap);
			Shader.SetGlobalFloat("_InterpolationRatio", 0f);

			bool isUniverseTransition = gate.transitionType == Gate.TransitionType.Sphere;
			Shader.SetGlobalFloat("_IsUniverseTransition", isUniverseTransition ? 1f : 0f);

			ui.cameraItemsTransition.transform.position = world.transform.position;
			ui.cameraItemsTransition.transform.rotation = Camera.main.transform.rotation;
			ui.cameraItemsTransition.fieldOfView = Camera.main.fieldOfView;

			// Update

			transitionTime = 0;
			float delay = isUniverseTransition ? 14f : 1f;

			while (transitionTime < delay) {

				transitionRatio = transitionTime / delay;
				Shader.SetGlobalFloat("_InterpolationRatio", transitionRatio);
				ui.cameraItemsTransition.transform.rotation = Camera.main.transform.rotation;
				ui.cameraItemsTransition.fieldOfView = Camera.main.fieldOfView;

				transitionTime += Time.deltaTime;
				yield return 0;
			}

			// OnComplete

			transitionRatio = 0f;
			transitionTime = 0f;
			Shader.SetGlobalFloat("_InterpolationRatio", 0f);
			Shader.SetGlobalFloat("_IsUniverseTransition", 0f);

			player.currentWorld.CloseAll();
			player.currentWorld = world;
			player.transform.position = player.currentWorld.transform.position;

			isInTransition = false;

		} else {
			yield return 0;
		}
	}

	public IEnumerator Fall (Portal portal) {
		
		World world = portal.anotherWorld;
		if (world != null) {

			// Start

			isInTransition = true;
			filterPlanet.enabled = true;

			player.currentWorld.SetupMaterial(world);
			Shader.SetGlobalVector("_HoleDirection", -Camera.main.transform.forward);
			Shader.SetGlobalTexture("_Cubemap", world.cubemap);
			Shader.SetGlobalFloat("_InterpolationRatio", 0f);
			Shader.SetGlobalFloat("_IsUniverseTransition", 0f);
			Shader.SetGlobalTexture("_Equirectangle", portal.equirectangle);

			ui.cameraItemsTransition.transform.position = world.transform.position;
			ui.cameraItemsTransition.transform.rotation = Camera.main.transform.rotation;
			ui.cameraItemsTransition.fieldOfView = Camera.main.fieldOfView;

			// Update

			transitionTime = 0;
			float delay = 10f;

			while (transitionTime < delay) {

				transitionRatio = transitionTime / delay;
				Shader.SetGlobalFloat("_InterpolationRatio", transitionRatio);
				ui.cameraItemsTransition.transform.rotation = Camera.main.transform.rotation;
				ui.cameraItemsTransition.fieldOfView = Camera.main.fieldOfView;

				transitionTime += Time.deltaTime;
				yield return 0;
			}

			// OnComplete

			transitionRatio = 0f;
			transitionTime = 0f;
			Shader.SetGlobalFloat("_InterpolationRatio", 0f);

			player.currentWorld = world;
			player.transform.position = player.currentWorld.transform.position;

			filterPlanet.enabled = false;
			isInTransition = false;

		} else {
			yield return 0;
		}
	}
}