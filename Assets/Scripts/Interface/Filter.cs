using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Filter : MonoBehaviour 
{
	Material materialUI;
	Material materialEquirectangle;
	public bool isEquirectangle = false;
	public Texture equirectangle;

	void Awake () {
		materialUI = new Material( Shader.Find("Hidden/UI") );
		// materialEquirectangle = new Material( Shader.Find("Hidden/Equirectangular") );
		materialEquirectangle = new Material( Shader.Find("Hidden/Stereographic") );
		materialEquirectangle.SetTexture("_Equirectangle", equirectangle);
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		
		if (isEquirectangle == false) {
			Graphics.Blit (source, destination, materialUI);

		} else {
			Graphics.Blit (source, destination, materialEquirectangle);
			materialEquirectangle.SetTexture("_Equirectangle", equirectangle);
		}
	}
}