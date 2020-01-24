using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level
{
	public int number;
	public Tile[, ] tiles;

	public bool IsBuilded;

	public Vector3 root;

	public Material material;
	public List<Gate> Gates;
	public Dictionary<Direction, Gate> GateByDirection;

	public void StepedOnLevel ()
	{
		//Debug.Log("stepped on level " + GetHashCode());
		//Debug.Log("gate count " + GateByDirection.Values.Count);
		foreach (var ob in GateByDirection.Values /*Gates*/ )
		{

			//Debug.Log(ob);
			//Debug.Log(ob.tileTransitions[0]);
			if (!ob.IsLevelBuilded)
			{
				TileManager.instance.BuildLevel (ob);
			}
		}
	}

	public void SetColoredTile (int x, int y)
	{
		material.SetInt ("_SteppedTileX", x);
		material.SetInt ("_SteppedTileY", y);
	}

	public void CleanLevel ()
	{
		//Debug.Log("clean level " + root);
		for (int i = 0; i < tiles.GetLength (0); i++)
		{
			for (int j = 0; j < tiles.GetLength (1); j++)
			{
				tiles[i, j].CleanUpSelf ();
				tiles[i, j] = null;
			}
		}
	}

	public Level ()
	{
		Gates = new List<Gate> ();
		GateByDirection = new Dictionary<Direction, Gate> ();
		IsBuilded = false;
		//Debug.Log("Level tiles allocation");
		tiles = new Tile[11, 11];
	}

	public Gate buildGate (Cord cord, Direction direction)
	{
		if (GateByDirection.ContainsKey (direction))
		{
			return null;
		}

		Gate localgate = new Gate ();
		localgate.direction = direction;

		Gates.Add (localgate);
		GateByDirection.Add (localgate.direction, localgate);

		localgate.tileTransitions[1] = new TileTransition (root, cord.row, cord.column);
		switch (direction)
		{
			case Direction.Up:
				localgate.tileTransitions[0] = new TileTransition (root, cord.row, cord.column - 1);
				localgate.tileTransitions[2] = new TileTransition (root, cord.row, cord.column + 1);
				break;
			case Direction.Down:
				localgate.tileTransitions[0] = new TileTransition (root, cord.row, cord.column - 1);
				localgate.tileTransitions[2] = new TileTransition (root, cord.row, cord.column + 1);
				break;
			case Direction.Left:
				localgate.tileTransitions[0] = new TileTransition (root, cord.row - 1, cord.column);
				localgate.tileTransitions[2] = new TileTransition (root, cord.row + 1, cord.column);
				break;
			case Direction.Right:
				localgate.tileTransitions[0] = new TileTransition (root, cord.row - 1, cord.column);
				localgate.tileTransitions[2] = new TileTransition (root, cord.row + 1, cord.column);
				break;
		}
		foreach (var ob in localgate.tileTransitions)
		{
			//Debug.Log(ob.cord.row + " " + ob.cord.column + " " + direction);
			ob.level = this;
			ob.position = root + new Vector3 (ob.cord.column, 0, ob.cord.row);
			tiles[ob.cord.row, ob.cord.column] = ob;
		}
		return localgate;
	}
}

public class Gate
{
	public Direction direction;

	//public TileTransition[] tileTransitions;
	//public List<TileTransition> tileTransitions;
	public List<TileTransition> tileTransitions;

	public bool IsLevelBuilded;

	public Gate ()
	{
		IsLevelBuilded = false;
		//tileTransitions = new TileTransition[3];
		tileTransitions = new List<TileTransition> ();
	}

	public void LinkGate (Gate gate)
	{
		//Debug.Log("linking gates tiletrans " + tileTransitions.Count + " " + gate.tileTransitions.Count);
		//Debug.Log("gates directions " + direction + " " + gate.direction);
		//Debug.Log("IslevelBuilded origin " + IsLevelBuilded + " gate " + gate.IsLevelBuilded);
		for (int i = 0; i < tileTransitions.Count; i++)
		{
			//int index = (i / tileTransitions.Count) * gate.tileTransitions.Count;
			//Debug.Log(i + " " + index);
			//Debug.Log(tileTransitions[i] + " " + gate.tileTransitions[i]);
			tileTransitions[i].TileByDirection.Add (direction, gate.tileTransitions[i]);

			//Debug.Log("link " + i + " " + index);
		}
		for (int i = 0; i < gate.tileTransitions.Count; i++)
		{
			//int index = (i / gate.tileTransitions.Count) * tileTransitions.Count;
			gate.tileTransitions[i].TileByDirection.Add (gate.direction, tileTransitions[i]);
			//Debug.Log("linkback " + i + " " + index);
		}

		IsLevelBuilded = gate.IsLevelBuilded = true;

	}
}

[System.Serializable]
public struct Cord
{
	public int row;
	public int column;

	public Cord (int row1, int column1)
	{
		row = row1;
		column = column1;
	}

	public static Cord operator + (Cord cd, Tuple<Direction, int> tuple)
	{
		Cord Localcord = new Cord (cd.row, cd.column);
		switch (tuple.Item1)
		{
			case Direction.Up:
				Localcord.row += tuple.Item2;
				break;
			case Direction.Left:
				Localcord.column -= tuple.Item2;
				break;
			case Direction.Down:
				Localcord.row -= tuple.Item2;
				break;
			case Direction.Right:
				Localcord.column += tuple.Item2;
				break;
		}
		return Localcord;
	}

	public void equals (Tuple<int, int> rowcolumn)
	{
		row = rowcolumn.Item1;
		column = rowcolumn.Item2;
	}

	public Vector3 ToVector3 ()
	{
		return new Vector3 (column, 0, row);
	}

	/*public static Cord operator = (Cord cd,Tuple<int,int> rowcolumn)
	{
		cd.row = rowcolumn.Item1;
		cd.column = rowcolumn.Item2;
		return cd;
	}*/
}