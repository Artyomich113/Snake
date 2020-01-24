using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artyomich;

public class alphatest : MonoBehaviour
{
	public MeshRenderer meshRenderer;

	public void MaterialAlpha(float val)
	{
		meshRenderer.material.color = meshRenderer.material.color.SetA(val);
		Debug.Log(meshRenderer.material.color);
	}
		
}
