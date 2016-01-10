using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class World : MonoBehaviour {

	protected Material material;
	protected Cubemap cubemap;
	Renderer[] rendererArray;

	void Awake () 
	{
		// Set perfect unwrapped sphere for 360 panorama (algo by Olivier Nemoz)
		GetComponent<MeshFilter>().mesh = MeshTool.CreateSphere(1f);

		material = GetComponent<Renderer>().sharedMaterial;
		cubemap = (Cubemap)material.GetTexture("_Cube");
		rendererArray = GetComponentsInChildren<Renderer>();
	}
	
	void Update () {
	
	}

	public void SetNextWorld (World world) {
		if (world != null) {
			material.SetTexture("_NextCube", world.cubemap);
		}
	}

	public void SetTransition (float ratio) {
		for (int i = 0; i < rendererArray.Length; ++i) {
			Material mat = rendererArray[i].material;
			mat.SetFloat("_InterpolationRatio", ratio);
		}
	}

	public void SetHoleDirection (Vector3 direction) {
		material.SetVector("_HoleDirection", direction);
	}
}
