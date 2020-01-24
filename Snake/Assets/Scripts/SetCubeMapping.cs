using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCubeMapping : MonoBehaviour
{
	public MeshFilter MeshFilter;

	public UVs uVs;

	private void Start()
	{
		SetUVs(uVs);
	}

	public void SetUVs(UVs uVs)
	{
		MeshFilter.mesh.uv = uVs.UV;
	}
}
