using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGlowScript : MonoBehaviour
{


	public Camera cam;

	

	private void Update()
	{
		Plane p = new Plane(Vector3.up, Vector3.zero);
		Vector3 mousePos = Input.mousePosition;
		Ray ray = cam.ScreenPointToRay(mousePos);

		if(p.Raycast(ray, out float enterDist))
		{
			Vector3 WorldMouse = ray.GetPoint(enterDist);
			//Debug.Log(mousePos + " "+ WorldMouse + " " + enterDist);
			Shader.SetGlobalVector("_MousePos", WorldMouse);
		}
	}

}
