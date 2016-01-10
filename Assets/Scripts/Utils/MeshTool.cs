using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshTool : MonoBehaviour 
{
	// Code from Olivier Nemoz translated into C#
	static public Mesh CreateSphere (float radius = 10f, int latitudeSubdivs = 90, int longitudeSubdivs = 45, bool inverseV = true)
	{
		latitudeSubdivs = Mathf.Max(latitudeSubdivs, 2);
		longitudeSubdivs = Mathf.Max(longitudeSubdivs, 4);

		List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> triangles = new List<int>();

		for (var i = 0; i < longitudeSubdivs + 1; ++i) 
		{
			var lg0 = (2 * i * Mathf.PI) / (longitudeSubdivs) + Mathf.PI * 6 / 4;

			for (var j = 0; j < latitudeSubdivs + 1; ++j) 
			{
				var l0 = (j * Mathf.PI) / (latitudeSubdivs) - Mathf.PI / 2;

				var clat = Mathf.Cos(l0);
				var slat = Mathf.Sin(l0);
				var clong = Mathf.Cos(lg0);
				var slong = Mathf.Sin(lg0);

				float x = clong * clat * radius;
				float y = slat * radius;
				float z = slong * clat * radius;
				vertices.Add(new Vector3(x,y,z));

				float s = i / (float)longitudeSubdivs;
				float t;
				if (inverseV) 
				{
					t = j / (float)latitudeSubdivs;
				} 
				else 
				{
					t = 1 - j / (float)latitudeSubdivs;
				}
				uvs.Add(new Vector2(s, t));

				var idx = i * (latitudeSubdivs + 1) + j;
				if (i != 0 && j != 0) 
				{
					triangles.Add(idx);
					triangles.Add(idx - 1);
					triangles.Add(idx - 1 - (latitudeSubdivs + 1));
					triangles.Add(idx - 1 - (latitudeSubdivs + 1));
					triangles.Add(idx - (latitudeSubdivs + 1));
					triangles.Add(idx);
				}
			}
		}

		Mesh mesh = new Mesh();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = uvs.ToArray();

		return mesh;
	}
}
