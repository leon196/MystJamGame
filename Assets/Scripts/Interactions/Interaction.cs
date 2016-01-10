using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interaction : MonoBehaviour {

	public bool isEnabled = true;
	[HideInInspector] public Interaction child = null;
	[HideInInspector] public Renderer render;
	[HideInInspector] public Collider collision;

	void Awake () 
	{
		render = GetComponent<Renderer>();
		if (render != null) {
			render.enabled = this.isEnabled;
		}

		collision = GetComponent<Collider>();
		if (collision != null) {
			collision.enabled = this.isEnabled;
		}

		if (this.transform.childCount > 0) {
			child = this.transform.GetChild(0).GetComponent<Interaction>();
		}
	}

	public virtual void Interact () {
		if (render != null) {
			render.enabled = true;
		}
		if (child) {
			child.isEnabled = true;
			child.render.enabled = true;
			child.collision.enabled = true;
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
