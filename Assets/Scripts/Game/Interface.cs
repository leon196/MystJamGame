using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour {

	Renderer cursorRender;
	Texture2D textureNone;
	public Texture textureLook;
	public Texture textureUse;
	public Texture texturePlus;
	public Texture textureMinus;
	public Texture textureCancel;
	public Texture textureStep;
	public Texture textureLoad;
	public Texture textureLock;
	public enum CursorType { None, Look, Use, Open, Step, Plus, Minus, Cancel, Load, Lock };
	CursorType cursorType;
	// float cursorScale;
	MouseLook mouseLook;
	public Camera cameraUI;
	public Camera cameraItems;
	RenderTexture textureUI;
	RenderTexture textureItems;

	void Start () {
		textureNone = new Texture2D(1, 1);
		textureNone.SetPixel(0, 0, new Color(1,0,0,0));
    textureNone.Apply();
		cursorRender = GetComponentInChildren<Renderer>();
		cursorRender.material.mainTexture = textureNone;
		// cursorScale = cursorRender.transform.localScale.x;
		mouseLook = GetComponent<MouseLook>();
		
		textureUI = new RenderTexture((int)Screen.width, (int)Screen.height, 24, RenderTextureFormat.ARGB32);
		textureUI.Create();
		cameraUI.targetTexture = textureUI;
		Shader.SetGlobalTexture("_UITexture", textureUI);

		textureItems = new RenderTexture((int)Screen.width, (int)Screen.height, 24, RenderTextureFormat.ARGB32);
		textureItems.Create();
		cameraItems.targetTexture = textureItems;
		Shader.SetGlobalTexture("_ItemsTexture", textureItems);
	}

	void Update () {
		float fovRatio = (Camera.main.fieldOfView - mouseLook.minFOV) / mouseLook.maxFOV;
		fovRatio = 1.5f * Mathf.Clamp(fovRatio, 0.05f, 1f);
		if (cameraItems) {
			cameraItems.fieldOfView = Camera.main.fieldOfView;
		}
		// cursorRender.transform.localScale = cursorScale * fovRatio * Vector3.one;
	}
	
	public void SetCursorType (CursorType type) {
		if (type != cursorType) {
			cursorType = type;
			switch (cursorType) {
				case CursorType.None: cursorRender.material.mainTexture = textureNone; break;
				case CursorType.Look: cursorRender.material.mainTexture = textureLook; break;
				case CursorType.Use: cursorRender.material.mainTexture = textureUse; break;
				case CursorType.Plus: cursorRender.material.mainTexture = texturePlus; break;
				case CursorType.Minus: cursorRender.material.mainTexture = textureMinus; break;
				case CursorType.Cancel: cursorRender.material.mainTexture = textureCancel; break;
				case CursorType.Step: cursorRender.material.mainTexture = textureStep; break;
				case CursorType.Load: cursorRender.material.mainTexture = textureLoad; break;
				case CursorType.Lock: cursorRender.material.mainTexture = textureLock; break;
			}
		}
	}
}
