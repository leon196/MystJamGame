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
	public enum CursorType { None, Look, Use, Open, Step, Plus, Minus, Cancel, Load };
	CursorType cursorType;
	float cursorScale;
	MouseLook mouseLook;

	void Start () {
		textureNone = new Texture2D(1, 1);
		textureNone.SetPixel(0, 0, new Color(1,0,0,0));
    textureNone.Apply();
		cursorRender = GetComponentInChildren<Renderer>();
		cursorRender.material.mainTexture = textureNone;
		cursorScale = cursorRender.transform.localScale.x;
		mouseLook = GetComponent<MouseLook>();
	}

	void Update () {
		float fovRatio = (Camera.main.fieldOfView - mouseLook.minFOV) / mouseLook.maxFOV;
		fovRatio = 1.5f * Mathf.Clamp(fovRatio, 0.05f, 1f);
		cursorRender.transform.localScale = cursorScale * fovRatio * Vector3.one;
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
			}
		}
	}
}
