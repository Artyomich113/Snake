using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectOnTile : PooledObject
{
	public Tile tile;

	IEnumerator movecoroutine;

	private void Start ()
	{
		movecoroutine = MoveToWithSpeed (Vector3.zero, Vector3.zero, 0.0f);
	}
	public abstract void Interract (ObjectOnTile ob);

	public void RemoveSelfFromTile ()
	{
		//if(this is EnemyBody)
		//Debug.Log(GetHashCode() + "remove from "+ tile);
		tile.Remove (this);

	}

	public abstract void CleanUp ();

	public void Add (Tile tile)
	{
		tile.AddObject (this);
	}

	public void RotateObject (Quaternion target, float RotationSpeed)
	{
		StartCoroutine (RotateToWithSpeed (target, RotationSpeed));
	}

	public void MoveObject (Vector3 StartPoint, Vector3 EndPoint, float time)
	{
		//Debug.Log("move " + StartPoint + " to " + EndPoint);

		movecoroutine = MoveToWithSpeed (StartPoint, EndPoint, time);
		//StopCoroutine(movecoroutine);
		StartCoroutine (movecoroutine);
	}

	IEnumerator RotateToWithSpeed (Quaternion targetRotation, float rotationSpeed)
	{
		while (transform.rotation != targetRotation)
		{
			transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate ();
		}
	}

	IEnumerator MoveToWithSpeed (Vector3 StartPoint, Vector3 EndPoint, float time)
	{
		transform.position = StartPoint;
		float localtime = 0;
		while (localtime < time)
		{
			transform.position = Vector3.Lerp (StartPoint, EndPoint, localtime / time);
			localtime += Time.fixedDeltaTime;
			//Debug.Log(transform.position);
			yield return new WaitForFixedUpdate ();
		}
	}

	public abstract void OnDie ();

}