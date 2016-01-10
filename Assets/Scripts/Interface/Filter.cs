using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Filter : MonoBehaviour 
{
	Material materialUI;
	Material materialEquirectangle;
	public bool isEquirectangle = false;

	void Awake () {
		materialUI = new Material( Shader.Find("Hidden/UI") );
		materialEquirectangle = new Material( Shader.Find("Hidden/Equirectangular") );
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		
		if (isEquirectangle == false) {
			Graphics.Blit (source, destination, materialUI);

		} else {
			Graphics.Blit (source, destination, materialEquirectangle);
		}
	}
}