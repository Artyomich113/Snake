using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
	[HideInInspector]
    public Objectpool objectpool;

    public void returnToPool ()
    {
		objectpool.returnToPool (this);
    }
}