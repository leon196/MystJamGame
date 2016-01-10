using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class World : MonoBehaviour {

	protected Material material;
	[HideInInspector] public Cubemap cubemap;
	[HideInInspector] public float rotation = 0f;
	Renderer[] rendererArray;

	void Awake () 
	{
		material = GetComponent<Renderer>().sharedMaterial;
		cubemap = (Cubemap)material.GetTexture("_Cube");
		rendererArray = GetComponentsInChildren<Renderer>();
		rotation = material.GetFloat("_Rotation");
	}
	
	void Update () {
	
	}

	public void SetNextWorld (World world) {
		if (world != null) {
			material.SetTexture("_NextCube", world.cubemap);
			material.SetVector("_HoleDirection", -Camera.main.transform.forward);
			material.SetFloat("_RotationNext", world.rotation);
		}
	}

	public void SetTransition (float ratio) {
		for (int i = 0; i < rendererArray.Length; ++i) {
			Material mat = rendererArray[i].material;
			mat.SetFloat("_InterpolationRatio", ratio);
		}
	}
}
