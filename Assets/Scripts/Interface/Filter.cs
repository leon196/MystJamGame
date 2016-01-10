using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Filter : MonoBehaviour 
{
	public Texture equirectangle;
	public Camera cameraCubemap;
	Material materialCubemap;
	RenderTexture cameraRender;

	void Awake () {

		Camera.main.depthTextureMode = DepthTextureMode.Depth;
		cameraRender = new RenderTexture(1024, 1024, 24); 
		cameraRender.isCubemap = true;
		cameraCubemap.targetTexture = cameraRender;

		materialCubemap = new Material( Shader.Find("Hidden/Equirectangular") );
		Shader.SetGlobalTexture("_EquirectangleTexture", equirectangle);
		Shader.SetGlobalTexture("_CubemapTexture", cameraRender);
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		Graphics.Blit (source, destination, materialCubemap);
	}
}