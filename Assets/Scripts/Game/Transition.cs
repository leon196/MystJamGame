using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour {

	[HideInInspector] public bool isInTransition = false;
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

				float ratio = t / delay;
				player.currentWorld.SetTransition(ratio);

				t += Time.deltaTime;
				yield return 0;
			}

			// OnComplete

			World previousWorld = player.currentWorld;
			player.currentWorld = world;
			player.transform.position = player.currentWorld.transform.position;

			previousWorld.SetTransition(0f);
			previousWorld.CloseAll();
			Shader.SetGlobalFloat("_IsUniverseTransition", 0f);

			isInTransition = false;

		} else {
			yield return 0;
		}
	}
}