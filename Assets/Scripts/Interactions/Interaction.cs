using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interaction : MonoBehaviour {

	public bool isEnabled = true;
	Gate[] gateArray;
	[HideInInspector] public Interaction child = null;
	[HideInInspector] public Renderer render;
	[HideInInspector] public Collider collision;

	void Awake () 
	{
		gateArray = GetComponentsInChildren<Gate>();

		render = GetComponent<Renderer>();
		if (render != null) {
			render.enabled = this.isEnabled;
		}

		collision = GetComponentInChildren<Collider>();
		if (collision != null) {
			collision.enabled = this.isEnabled;
		}

		if (this.transform.childCount > 0) {
			child = this.transform.GetChild(0).GetComponent<Interaction>();
			if (child) {
				child.Hide();
				child.Disable();
			}
		}
	}

	public virtual void Interact () {
		Disable();
		
		if (child) {
			child.Show();
			child.Enable();
		}
	}

	public void Enable () {
		this.isEnabled = true;
		if (collision != null) {
			collision.enabled = true;
		}
		for (int i = 0; i < gateArray.Length; ++i) {
			gateArray[i].isEnabled = true;
		}
	}

	public void Disable () {
		this.isEnabled = false;
		if (collision != null) {
			collision.enabled = false;
		}
		for (int i = 0; i < gateArray.Length; ++i) {
			gateArray[i].isEnabled = false;
		}
	}

	public void Show () {
		if (render != null) {
			render.enabled = true;
		}
	}

	public void Hide () {
		if (render != null) {
			render.enabled = false;
		}
	}

	List<Interaction> GetChildren () {
		List<Interaction> list = new List<Interaction>();
		Interaction[] childs = GetComponentsInChildren<Interaction>();	
		for (int i = 0; i < childs.Length; ++i) {
			Interaction child = childs[i];
			if (child.gameObject != this.gameObject) {
				list.Add(child);
			}
		}
		return list;
	}
}
