using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour {

	[HideInInspector] public bool isInTransition = false;
	[HideInInspector] public float transitionRatio = 0f;
	[HideInInspector] public float transitionTimeRatio = 0f;
	Player player;

	void Start () {
		player = GetComponent<Player>();
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
			Shader.SetGlobalFloat("_IsUniverseTransition", gate.isAnotherUniverse ? 1f : 0f);

			// Update

			float t = 0;
			float delay = gate.isAnotherUniverse ? 5f : 1f;

			while (t < delay) {

				transitionRatio = t / delay;
				transitionTimeRatio = Mathf.Clamp(t, 0f, 1f);
				player.currentWorld.SetTransition(transitionRatio);

				t += Time.deltaTime;
				yield return 0;
			}

			// OnComplete

			World previousWorld = player.currentWorld;
			player.currentWorld = world;
			player.transform.position = player.currentWorld.transform.position;

			transitionRatio = 0f;
			transitionTimeRatio = 0f;
			previousWorld.CloseAll();
			previousWorld.SetTransition(transitionRatio);
			Shader.SetGlobalFloat("_IsUniverseTransition", 0f);

			isInTransition = false;

		} else {
			yield return 0;
		}
	}
}