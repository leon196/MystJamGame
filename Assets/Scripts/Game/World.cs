using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

	protected Material material;
	[HideInInspector]public Cubemap cubemap;
	[HideInInspector] public float rotation = 0f;
	Renderer[] rendererArray;
	Page[] pageArray;

	void Awake () 
	{
		material = GetComponent<Renderer>().material;
		cubemap = (Cubemap)material.GetTexture("_Cube");
		rendererArray = GetComponentsInChildren<Renderer>();
		rotation = material.GetFloat("_Rotation");
		pageArray = GetComponentsInChildren<Page>();
	}
	
	void Update () {
	
	}

	public void Show () {
		for (int i = 0; i < rendererArray.Length; ++i) {
			rendererArray[i].enabled = true;
		}
	}

	public void Hide () {
		for (int i = 0; i < rendererArray.Length; ++i) {
			rendererArray[i].enabled = false;
		}
	}

	public void SetNextWorld (World world) {
		material.SetTexture("_NextCube", world.cubemap);
		material.SetFloat("_RotationNext", world.rotation);
		Shader.SetGlobalVector("_HoleDirection", -Camera.main.transform.forward);
		Shader.SetGlobalTexture("_Cubemap", world.cubemap);
	}

	public void SetTransition (float ratio) {
		for (int i = 0; i < rendererArray.Length; ++i) {
			Material mat = rendererArray[i].material;
			mat.SetFloat("_InterpolationRatio", ratio);
		}
	}

	public void CloseAll () {
		for (int i = 0; i < pageArray.Length; ++i) {
			Page page = pageArray[i];
			page.Close();
		}
	}
}
