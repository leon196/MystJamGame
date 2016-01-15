using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Page : Interaction {

	bool taken = false;

	public IEnumerator Attach (Transform parent) {
		
		if (taken == false) {
			taken = true;

			Disable();

			transform.parent = parent;
			Vector3 posFrom = transform.position;
			Vector3 posTo = Camera.main.transform.position;
			Quaternion rotFrom = transform.localRotation;
			Quaternion rotTo = Quaternion.Euler(Vector3.up * 180f);
			Material material = GetComponent<Renderer>().material;
			float hideFrom = material.GetFloat("_HideRatio");

			float timeElapsed = 0f;
			float duration = 1.5f;
			float ratio = 0f;
			while (timeElapsed < duration) {
				ratio = Mathf.Clamp(timeElapsed / duration, 0f, 1f);
				transform.position = Vector3.Slerp(posFrom, posTo, ratio);
				transform.localRotation = Quaternion.Slerp(rotFrom, rotTo, ratio);
				material.SetFloat("_Alpha", 1f - Mathf.Max(0f, ratio - 0.8f) * 5f);
				material.SetFloat("_HideRatio", Mathf.Lerp(hideFrom, 1f, ratio));
				timeElapsed += Time.deltaTime;
				yield return 0;
			}

			Home.AddToCollection(this.transform);
			material.SetFloat("_Alpha", 1f);

		} else {
			yield return 0;
		}
	}
}
