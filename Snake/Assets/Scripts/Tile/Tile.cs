using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile
{
	public List<ObjectOnTile> objectsOnTile;

	public Level level;
	public Vector3 position;

	public Cord cord;

	public Tile (Vector3 vector3)
	{
		position = vector3;
	}

	public Tile ()
	{
		// Debug.Log("initializing tile");
		objectsOnTile = new List<ObjectOnTile> ();
	}

	public void Remove (ObjectOnTile objectOnTile)
	{
		if (objectsOnTile.Contains (objectOnTile))
		{
			objectsOnTile.Remove (objectOnTile);
		}
	}

	public void CleanUpSelf ()
	{
		foreach (var ob in objectsOnTile.ToArray ())
		{
			ob.RemoveSelfFromTile ();
			ob.returnToPool ();
			ob.CleanUp ();
		}
		objectsOnTile.RemoveRange (0, objectsOnTile.Count);
	}

	public void InterractManyToOne (ObjectOnTile objectOnTile) // entire tile interract with one object
	{
		if (objectsOnTile.Count > 0)
		{
			foreach (var ob in objectsOnTile.ToArray ())
			{
				ob.Interract (objectOnTile);
			}
		}
	}

	public void interractOneToMany (ObjectOnTile objectOnTile) // some object interracts with other objects
	{
		if (objectsOnTile.Count > 0)
		{
			foreach (var ob in objectsOnTile.ToArray ())
			{
				objectOnTile.Interract (ob);
			}
		}
	}

	public abstract Tile GetTile (Direction direction);

	public Tile GetTileOnLevel (Direction direction)
	{
		Cord newcord = cord;

		switch (direction)
		{
			case Direction.Up:
				newcord.row++;
				break;
				// return level.tiles[cord.row + 1, cord.column];
			case Direction.Down:
				newcord.row--;
				break;
				// return level.tiles[cord.row - 1, cord.column];
			case Direction.Left:
				newcord.column--;
				break;
				// return level.tiles[cord.row, cord.column - 1];
			case Direction.Right:
				newcord.column++;
				break;
				// return level.tiles[cord.row, cord.column + 1];
			default:
				break;
				//return level.tiles[cord.row, cord.column];
		}
		//Debug.Log("cord row:" + newcord.row + " column:" + newcord.column);
		if (newcord.row >= 0 && newcord.column >= 0 && newcord.row < 11 && newcord.column < 11)
			return level.tiles[newcord.row, newcord.column];
		else
			return null;
	}

	public abstract ObjectOnTile AddObject (ObjectOnTile objectOnTile, bool SetPosition = true);
}