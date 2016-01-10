using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class World : MonoBehaviour {

	protected Material material;
	[HideInInspector] public Cubemap cubemap;
	[HideInInspector] public float rotation = 0f;
	Renderer[] rendererArray;
	Page[] pageArray;

	void Awake () 
	{
		material = GetComponent<Renderer>().sharedMaterial;
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
		Shader.SetGlobalVector("_HoleDirection", -Camera.main.transform.forward);
		if (world != null) {
			material.SetTexture("_NextCube", world.cubemap);
			material.SetFloat("_RotationNext", world.rotation);
		}
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
			bool isRootPage = page.GetComponentsInParent<Page>().Length > 1;
			if (isRootPage) {
				page.Close();
			} else {
				page.Reset();
			}
		}
	}
}
