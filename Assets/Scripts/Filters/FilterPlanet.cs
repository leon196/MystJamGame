using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FilterPlanet : MonoBehaviour 
{
	Material material;
	public float interpolationRatio = 0f;

	void Awake () {
		material = new Material( Shader.Find("Hidden/Stereographic") );
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetFloat("_InterpolationRatio", interpolationRatio);
		material.SetFloat("_CameraAngleY", Camera.main.transform.rotation.eulerAngles.y * Mathf.PI / 180f);
		Graphics.Blit (source, destination, material);
	}
}