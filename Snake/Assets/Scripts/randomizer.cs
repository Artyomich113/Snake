using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomizer : MonoBehaviour
{
    public static bool Chance(float val)
	{
		if(Random.Range(0,100) < val)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
