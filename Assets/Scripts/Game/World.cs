using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

	protected Material material;
	[HideInInspector]public Cubemap cubemap;
	[HideInInspector] public float rotation = 0f;
	Renderer[] rendererArray;
	Book[] bookArray;

	void Awake () 
	{
		material = GetComponent<Renderer>().material;
		cubemap = (Cubemap)material.GetTexture("_Cube");
		rendererArray = GetComponentsInChildren<Renderer>();
		rotation = material.GetFloat("_Rotation");
		bookArray = GetComponentsInChildren<Book>();
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

	public void SetupMaterial (World world) {
		material.SetTexture("_NextCube", world.cubemap);
		material.SetFloat("_RotationNext", world.rotation);
		world.material.SetTexture("_NextCube", world.cubemap);
		world.material.SetFloat("_RotationNext", world.rotation);
	}

	public void CloseAll () {
		for (int i = 0; i < bookArray.Length; ++i) {
			Book book = bookArray[i];
			book.Close();
		}
	}
}
