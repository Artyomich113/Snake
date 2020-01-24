using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artyomich;

public class TileTransition : Tile
{
	public Dictionary<Direction, Tile> TileByDirection;

	public TileTransition(Vector3 root, int row1, int column1)
	{
		cord = new Cord(row1, column1);
		position = root + new Vector3(cord.column, 0, cord.row);
		TileByDirection = new Dictionary<Direction, Tile>();
	}

	public override ObjectOnTile AddObject(ObjectOnTile objectOnTile, bool SetPosition = true)
	{
		if (objectOnTile.GetType() == typeof(Wall))
		{
			//Debug.Log("trying build " + objectOnTile.gameObject.name + " on tiletransition");
			return objectOnTile;
		}
		objectOnTile.tile = this;
		if (!objectsOnTile.Contains(objectOnTile))
		{
			objectsOnTile.Add(objectOnTile);
			objectOnTile.tile = this;
			if (SetPosition)
				objectOnTile.transform.position = position.SetZX(position.z, position.x);
		}
		return objectOnTile;
	}


	public override Tile GetTile(Direction direction)
	{
		if (TileByDirection.ContainsKey(direction))
		{
			//Debug.Log("tran in direction " + direction);
			Tile tile = TileByDirection[direction];
			tile.level.StepedOnLevel();
			TileManager.instance.onStepedLevel?.Invoke(tile.level, direction);

			return tile;
		}
		else
		{
			//Debug.Log("tiletrans direction " + direction + " keycount " + TileByDirection.Keys.Count);
			foreach (var ob in TileByDirection.Keys)
			{
				//Debug.Log(ob);
			}

			//Debug.Log(TileByDirection.Keys);
			switch (direction)
			{
				case Direction.Up:
					return level.tiles[cord.row + 1, cord.column];
				case Direction.Down:
					return level.tiles[cord.row - 1, cord.column];
				case Direction.Left:
					return level.tiles[cord.row, cord.column - 1];
				case Direction.Right:
					return level.tiles[cord.row, cord.column + 1];
				default: return level.tiles[cord.row, cord.column];
			}
		}
	}

}
