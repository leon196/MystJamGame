using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour {

	Player player;
	MouseLook mouseLook;
	Interface ui;
	Transform audioListener;
	FilterPlanet filterPlanet;
	FX fx;
	Voice voice;
	public Material materialTitle;
	public Material materialThanks;

	Gate.TransitionType transitionType;

	[HideInInspector] public bool isInTransition = false;
	[HideInInspector] public float transitionRatio = 0f;
	[HideInInspector] public float transitionTime = 0f;

	void Start () 
	{
		player = GameObject.FindObjectOfType<Player>();
		mouseLook = GameObject.FindObjectOfType<MouseLook>();
		ui = GameObject.FindObjectOfType<Interface>();
		audioListener = GameObject.FindObjectOfType<AudioListener>().transform;
		filterPlanet = GameObject.FindObjectOfType<FilterPlanet>();

		materialTitle.SetFloat("_Alpha", 0f);
		materialThanks.SetFloat("_Alpha", 0f);

		Shader.SetGlobalFloat("_InterpolationRatio", 0f);
		Shader.SetGlobalFloat("_IsSphereTransition", 0f);
		Shader.SetGlobalVector("_HoleDirection", Vector3.up);

		audioListener.position = player.transform.position;
	}

	void Update ()
	{
    if (isInTransition && transitionTime < 1f && transitionType == Gate.TransitionType.Fade) {
      mouseLook.fieldOfView = transitionTime * 90f;
    }
	}

	public IEnumerator Intro (Gate gateSpawn) 
	{
		// Start

		fx = GameObject.FindObjectOfType<FX>();
		voice = GameObject.FindObjectOfType<Voice>();

		voice.Cough();
		yield return new WaitForSeconds(1f);
		
		fx.Match();
		yield return new WaitForSeconds(3f);

		// Update

		float timeElapsed = 0f;
		float timeRatio = 0f;
		float duration = 2f;
		while (timeElapsed < duration) {
			timeRatio = timeElapsed / duration;
			materialTitle.SetFloat("_Alpha", timeRatio);
			timeElapsed += Time.deltaTime;
			yield return 0;
		}

		yield return new WaitForSeconds(1f);

		timeElapsed = 0f;

		while (timeElapsed < duration) {
			timeRatio = timeElapsed / duration;
			materialTitle.SetFloat("_Alpha", 1f - timeRatio);
			timeElapsed += Time.deltaTime;
			yield return 0;
		}

		fx.BookFall();

		yield return new WaitForSeconds(1f);

		StartCoroutine(Goto(gateSpawn));

		player.started = true;
	}

	public IEnumerator Goto (Gate gate) {

		if (gate != null) {
			World world = null;
			
			if (gate.anotherGate) {
				world = gate.anotherGate.GetComponentInParent<World>();
			} else if (gate.anotherWorld) {
				world = gate.anotherWorld;
			}

			if (world != null) {

				// Start

				transitionType = gate.transitionType;
				transitionTime = 0;
				float delay = 1f;

				switch (transitionType) 
				{
					case Gate.TransitionType.Fall : {
						delay = 6f;
						filterPlanet.enabled = true;
						Shader.SetGlobalFloat("_IsSphereTransition", 0f);
						Shader.SetGlobalTexture("_Equirectangle", gate.equirectangle);
						break;
					}
					case Gate.TransitionType.Sphere : {
						delay = 10f;
						Shader.SetGlobalFloat("_IsSphereTransition", 1f);
						break;
					}
				}

				isInTransition = true;
				player.currentWorld.SetupMaterial(world);
				Shader.SetGlobalVector("_HoleDirection", -Camera.main.transform.forward);
				Shader.SetGlobalTexture("_Cubemap", world.cubemap);
				Shader.SetGlobalFloat("_InterpolationRatio", 0f);

				ui.cameraNextPanorama.transform.position = world.transform.position;
				ui.cameraNextPanorama.transform.rotation = Camera.main.transform.rotation;
				ui.cameraNextPanorama.fieldOfView = Camera.main.fieldOfView;
		
				Vector3 posFrom = player.transform.position;
				Vector3 posTo = world.transform.position;

				// Update

				while (transitionTime < delay) {

					transitionRatio = transitionTime / delay;
					Shader.SetGlobalFloat("_InterpolationRatio", transitionRatio);
					ui.cameraNextPanorama.transform.rotation = Camera.main.transform.rotation;
					ui.cameraNextPanorama.fieldOfView = Camera.main.fieldOfView;

					audioListener.position = Vector3.Lerp(posFrom, posTo, transitionRatio);

					if (transitionType == Gate.TransitionType.Fall && mouseLook.fieldOfView < mouseLook.maxFOV / 2f) {
						mouseLook.fieldOfView = 90f;
					}

					transitionTime += Time.deltaTime;
					yield return 0;
				}

				// OnComplete

				transitionRatio = 0f;
				transitionTime = 0f;
				Shader.SetGlobalFloat("_InterpolationRatio", 0f);
				Shader.SetGlobalFloat("_IsSphereTransition", 0f);

				player.currentWorld.CloseAll();
				player.currentWorld = world;
				player.transform.position = player.currentWorld.transform.position;

				if (transitionType == Gate.TransitionType.Fall) {
					filterPlanet.enabled = false;
				}

				isInTransition = false;

			} else {
				yield return 0;
			}
		}
	}
}