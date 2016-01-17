using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Statue : Interaction 
{
	List<Material> materialList;
	Material materialShadow;

	void Start () 
	{
		materialList = new List<Material>();
		Renderer[] rendererArray = GetComponentsInChildren<Renderer>();
		for (int i = 0; i < rendererArray.Length; ++i)
		{
			Renderer renderer = rendererArray[i];
			for (int m = 0; m < renderer.materials.Length; ++m) {
				Material material = renderer.materials[m];
				// Statue
				if (material.shader.name == "Custom/Statue") {
					materialList.Add(material);
				// Shadow
				} else {
					materialShadow = material;
				}
			}
		}
	} 

	public override void Interact () 
	{
		StartCoroutine(Teleport());
	}
	
	IEnumerator Teleport ()
	{
		// Start

		Disable();

		// Update

		float timeElapsed = 0f;
		float timeRatio = 0f;
		float duration = 5f;

		while (timeElapsed < duration) 
		{
			timeRatio = timeElapsed / duration;

			foreach (Material material in materialList) {
				material.SetFloat("_TeleportationRatio", timeRatio);
			}
			materialShadow.SetFloat("_Alpha", 1f - timeRatio);

			timeElapsed += Time.deltaTime;
			yield return 0;
		}

		Home.AddStatueToCollection(this.transform);

		foreach (Material material in materialList) {
			material.SetFloat("_TeleportationRatio", 0f);
			materialShadow.SetFloat("_MetaRatio", 1f);
		}
		materialShadow.SetFloat("_Alpha", 0f);
	}
}
