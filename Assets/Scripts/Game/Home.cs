using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Home : World {

	static Home instance;
	static List<Transform> tresorList;
	static int tresorCount = 0;

	void Start ()
	{
		instance = this;
		tresorList = new List<Transform>();
		tresorCount = GameObject.FindObjectsOfType<Page>().Length;
	}

	static public void AddToCollection (Transform tresor)
	{
		float tresorRatio = (float)tresorList.Count / (float)tresorCount;
		float angle = 4f * Mathf.PI * tresorRatio;
		float radius = 0.02f + 0.2f * Mathf.Sin(tresorRatio * Mathf.PI);
		float height = -0.2f  + tresorRatio * 0.4f;
		tresor.parent = Home.instance.transform;
		tresor.localPosition = new Vector3(Mathf.Cos(angle) * radius, height, Mathf.Sin(angle) * radius);
		tresor.LookAt(tresor.parent);
		tresorList.Add(tresor);
	}
}