using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour {

	[HideInInspector] public bool isInTransition = false;
	[HideInInspector] public float transitionRatio = 0f;
	[HideInInspector] public float transitionTimeRatio = 0f;
	Player player;

	void Start () {
		player = GameObject.FindObjectOfType<Player>();
	}

	public IEnumerator FallOn (World world) {
		if (world) {

			// Start

			isInTransition = true;

			// Update

			float t = 0;
			float delay = 10f;

			while (t < delay) {

				transitionRatio = t / delay;
				transitionTimeRatio = Mathf.Clamp(t, 0f, 1f);

				t += Time.deltaTime;
				yield return 0;
			}

			// OnComplete

			transitionRatio = 0f;
			transitionTimeRatio = 0f;

			player.currentWorld = world;
			player.transform.position = player.currentWorld.transform.position;

			isInTransition = false;

		} else {
			yield return 0;
		}
	}

	public IEnumerator Goto (Gate gate) {

		World world = null;
		
		if (gate.anotherGate) {
			world = gate.anotherGate.GetComponentInParent<World>();
		}

		if (world != null) {

			// Start

			isInTransition = true;
			player.currentWorld.SetNextWorld(world);
			Shader.SetGlobalFloat("_InterpolationRatio", 0f);
			Shader.SetGlobalFloat("_IsUniverseTransition", gate.isAnotherUniverse ? 1f : 0f);

			// Update

			float t = 0;
			float delay = gate.isAnotherUniverse ? 14f : 1f;

			while (t < delay) {

				transitionRatio = t / delay;
				transitionTimeRatio = Mathf.Clamp(t, 0f, 1f);
				Shader.SetGlobalFloat("_InterpolationRatio", transitionRatio);

				t += Time.deltaTime;
				yield return 0;
			}

			// OnComplete

			transitionRatio = 0f;
			transitionTimeRatio = 0f;
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
}