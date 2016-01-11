using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FilterEquirectangle : MonoBehaviour 
{
	Material material;
	public float interpolationRatio = 0f;

	void Awake () {
		material = new Material( Shader.Find("Hidden/Equirectangular") );
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		Graphics.Blit (source, destination, material);
	}
}