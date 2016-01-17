using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Home : World {

	static Home instance;
	static BoidManager boidManager;
	static List<Transform> pageList;
	static List<Transform> statueList;
	static int pageCount = 0;
	static int statueCount = 0;

	void Start ()
	{
		instance = this;
		pageList = new List<Transform>();
		statueList = new List<Transform>();
		pageCount = GameObject.FindObjectsOfType<Page>().Length;
		statueCount = GameObject.FindObjectsOfType<Statue>().Length;
		boidManager = GetComponent<BoidManager>();
	}

	static public void AddToCollection (Transform page)
	{
		float pageRatio = (float)pageList.Count / (float)pageCount;
		float angle = 4f * Mathf.PI * pageRatio;
		float radius = 0.02f + 0.2f * Mathf.Sin(pageRatio * Mathf.PI);
		float height = -0.2f  + pageRatio * 0.4f;
		page.parent = Home.instance.transform;
		page.localPosition = new Vector3(Mathf.Cos(angle) * radius, height, Mathf.Sin(angle) * radius);
		page.LookAt(page.parent);
		pageList.Add(page);
		// boidManager.AddBoid(page.gameObject, Home.instance.transform.position);
	}

	static public void AddStatueToCollection (Transform statue)
	{
		float statueRatio = (float)statueList.Count / (float)statueCount;
		float angle = 2f * Mathf.PI * statueRatio;
		float radius = 0.3f;
		statue.parent = Home.instance.transform;
		statue.localPosition = new Vector3(Mathf.Cos(angle) * radius, -0.03f, Mathf.Sin(angle) * radius);
		statue.LookAt(statue.parent);
		Vector3 rot = statue.rotation.eulerAngles;
		rot.x = 0f;
		rot.z = 0f;
		statue.rotation = Quaternion.Euler(rot);
		statueList.Add(statue);
	}
}