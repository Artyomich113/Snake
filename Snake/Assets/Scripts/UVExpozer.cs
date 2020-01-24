using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVExpozer : MonoBehaviour
{
	public MeshFilter meshFilter;

	public bool CustomUVEnabled = false;
	public bool ExposeEnabled = false;
	public Vector2[] customUVs;
	public Vector3[] customVertices;
	public int[] custorTriangles;


	public Vector2[] UV;
	//public Vector2[] UV2;
	//public Vector2[] UV3;
	public Vector3[] Vertices;
	public int[] Triangles;
	private void Start()
	{
		//meshFilter.mesh.set
		
		if (CustomUVEnabled)
		{
			meshFilter.mesh.triangles = custorTriangles;
			meshFilter.mesh.vertices = customVertices;
			meshFilter.mesh.uv = customUVs;
		}
		if(ExposeEnabled)
		{
			UV = meshFilter.mesh.uv;
			//UV2 = meshFilter.mesh.uv2;
			//UV3 = meshFilter.mesh.uv3;
			Vertices = meshFilter.mesh.vertices;
			Triangles = meshFilter.mesh.triangles;
		}
	}
}
