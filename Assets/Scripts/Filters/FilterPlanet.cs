using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FilterPlanet : MonoBehaviour 
{
	public Texture equirectangle;
	Material material;

	void Awake () {
		material = new Material( Shader.Find("Hidden/Stereographic") );
		material.SetTexture("_Equirectangle", equirectangle);
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		Graphics.Blit (source, destination, material);
	}
}