using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTail : SnakePart
{
	//[HideInInspector]
	public int PartNumber;

	//[HideInInspector]
	public SnakePart AttachedSnakePart;

	public delegate void OnBite (int number);

	public delegate void OnLastSnakePart (SnakePart snakePart);

	public OnBite onBite = delegate { };

	public OnLastSnakePart onLastSnakePart = delegate { };

	public override char Type => 't';

	public override void Interract (ObjectOnTile obj)
	{
		if (obj is AEnemyBody)
		{
			//Debug.Log("tail eaten" + PartNumber);
			onBite (PartNumber);
		}
	}

	public override void MoveToNext (float timetomove)
	{
		//Debug.Log(GetHashCode() + " is MoveToNext " + AttachedSnakePart.GetHashCode());
		tile.Remove (this);
		Tile newtile = AttachedSnakePart.tile;
		RotateObject (Quaternion.LookRotation (newtile.position - transform.position), 90.0f);
		//MoveObject(transform.position, newtile.position,timetomove);
		MoveObject (tile.position, newtile.position, timetomove);

		//MoveObject(transform.position, AttachedSnakePart.transform.position,timetomove);
		//newtile.interract(this);

		newtile.AddObject (this, false);
		newtile.interractOneToMany (this);
		//MoveObject(tile.position, AttachedSnakePart.transform.position,timetomove);

		//transform.rotation = Quaternion.LookRotation(newtile.position - transform.position);
		AttachedSnakePart.MoveToNext (timetomove);
	}

	public override void OnDie ()
	{
		RemoveSelfFromTile ();
		returnToPool ();
	}

	private void OnDisable ()
	{
		AttachedSnakePart = null;
	}

	public override void CleanUp ()
	{

	}

	public override void DieUntil (int PartNumber)
	{

		if (this.PartNumber >= PartNumber)
		{
			//Debug.Log("chain death " + this.PartNumber);
			AttachedSnakePart.DieUntil (PartNumber);
			OnDie ();
		}
		else
		{
			onLastSnakePart (this);
		}
	}
}