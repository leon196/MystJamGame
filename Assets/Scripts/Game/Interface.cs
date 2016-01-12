using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interface : MonoBehaviour {

	Renderer cursorRender;

	public enum CursorType { None, Look, Use, Open, Step, Plus, Minus, Cancel, Load, Lock };
	CursorType cursorType;

	public Texture textureLook;
	public Texture textureUse;
	public Texture texturePlus;
	public Texture textureMinus;
	public Texture textureCancel;
	public Texture textureStep;
	public Texture textureLoad;
	public Texture textureLock;
	private Texture2D textureNone;

	MouseLook mouseLook;
	public Camera cameraUI;
	public Camera cameraItems;
	public Camera cameraItemsTransition;
	RenderTexture textureUI;
	RenderTexture textureItems;
	RenderTexture textureItemsTransition;

	void Start () 
	{
		mouseLook = GetComponent<MouseLook>();
		
		textureUI = new RenderTexture((int)Screen.width, (int)Screen.height, 24, RenderTextureFormat.ARGB32);
		textureUI.Create();
		cameraUI.targetTexture = textureUI;
		Shader.SetGlobalTexture("_UITexture", textureUI);

		textureItems = new RenderTexture((int)Screen.width, (int)Screen.height, 24, RenderTextureFormat.ARGB32);
		textureItems.Create();
		cameraItems.targetTexture = textureItems;
		Shader.SetGlobalTexture("_ItemsTexture", textureItems);

		textureItemsTransition = new RenderTexture((int)Screen.width, (int)Screen.height, 24, RenderTextureFormat.ARGB32);
		textureItemsTransition.Create();
		cameraItemsTransition.targetTexture = textureItemsTransition;
		Shader.SetGlobalTexture("_ItemsNextTexture", textureItemsTransition);
		
		textureNone = new Texture2D(1, 1);
		textureNone.SetPixel(0, 0, new Color(1,0,0,0));
    textureNone.Apply();

		cursorRender = GetComponentInChildren<Renderer>();
		cursorRender.material.mainTexture = textureNone;

		Gate[] gateArray = GameObject.FindObjectsOfType<Gate>();
		List<Vector3> positionList = new List<Vector3>();
		for (int i = 0; i < gateArray.Length; ++i) {
			Gate gate = gateArray[i];
			if (gate.anotherGate != null && gate.GetComponent<Page>() == null) {
				positionList.Add(gate.transform.position);
			}
		}
		Vector3[] positionArray = positionList.ToArray();
		GameObject.FindObjectOfType<ParticleCloud>().SetPositionArray(positionArray);
	}

	void Update () 
	{
		float fovRatio = (Camera.main.fieldOfView - mouseLook.minFOV) / mouseLook.maxFOV;
		fovRatio = 1.5f * Mathf.Clamp(fovRatio, 0.05f, 1f);
		if (cameraItems) {
			cameraItems.fieldOfView = Camera.main.fieldOfView;
		}
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
