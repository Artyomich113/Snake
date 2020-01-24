using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectpool : MonoBehaviour
{

    [SerializeField]
    private PooledObject prefab = null;

    private Queue<PooledObject> objects;

	//public int count;

    private void Awake ()
    {
        objects = new Queue<PooledObject> ();
    }
    public int GetLenth ()
    {
        return objects.Count;
    }

    public PooledObject Get ()
    {
        //Debug.Log("genericobjectpool index " + index);
        if (objects.Count == 0)
            addobjects (1);
		PooledObject ob = objects.Dequeue();
		//count = objects.Count;
		return ob;
    }

    public void returnToPool (PooledObject objectToReturn, int index = 0)
    {
        objectToReturn.gameObject.SetActive (false);
        objects.Enqueue (objectToReturn);
    }

    public void addobjects (int count)
    {
        for (int i = 0; i < count; i++)
        {
            var newobject = Instantiate (prefab);
            newobject.gameObject.SetActive (false);
            newobject.objectpool = this;
            objects.Enqueue (newobject);
        }
    }
}