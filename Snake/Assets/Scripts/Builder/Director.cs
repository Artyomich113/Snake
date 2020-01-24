using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director
{
	public Director()
	{
		//Debug.Log("director creation");
	}

	public void Construct(Builder builder, Level level, Vector3 root, Gate gate)
	{
		builder.BuildLevel(level, root, gate);
	}
}