using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artyomich;

public class TileSimple : Tile
{
	public TileSimple(Vector3 root, int row1, int column1)
	{
		cord = new Cord(row1, column1);
		position = root + new Vector3(cord.column, 0, cord.row);
	}

	public override ObjectOnTile AddObject(ObjectOnTile objectOnTile, bool SetPosition = true)
	{
		objectOnTile.tile = this;

		//if(objectOnTile is EnemyBody)
		//{
		//	Debug.Log(GetHashCode() + " add tile " + objectOnTile.tile);
		//}
		if (!objectsOnTile.Contains(objectOnTile))
		{
			//Debug.Log("Adding " + objectOnTile.name);
			objectsOnTile.Add(objectOnTile);
			if (SetPosition)
				objectOnTile.transform.position = position.SetZX(position.z, position.x);
		}

		return objectOnTile;
	}

	public override Tile GetTile(Direction direction)
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
			default: break;
				//return level.tiles[cord.row, cord.column];
		}

		//Debug.Log(level);
		//Debug.Log(level.tiles);

		if (newcord.row >= 0 && newcord.column >= 0 && newcord.row < level.tiles.GetLength(0) && newcord.row < level.tiles.GetLength(1))
		{
			//Debug.Log("tile " + newcord.row + " " + newcord.column);
			return level.tiles[newcord.row, newcord.column];
		}
		else
		{
			//Debug.Log("null tile");
			//Debug.LogError("tile out of bounds of array[" + level.tiles.GetLength(0) + "," + level.tiles.GetLength(1) + "] --" + newcord.row + "," + newcord.column);
			return null;
		}
	}
}
