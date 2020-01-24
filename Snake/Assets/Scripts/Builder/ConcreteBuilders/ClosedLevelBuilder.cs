using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClosedLevelBuilder : Builder
{
	public ClosedLevelBuilder() { }
	public override void BuildLevel(Level level, Vector3 root, Gate gate)
	{
		level.buildGate(new Cord(0, 5), Direction.Down);
		level.buildGate(new Cord(10, 5), Direction.Up);
		level.buildGate(new Cord(5, 0), Direction.Left);
		level.buildGate(new Cord(5, 10), Direction.Right);

		//Debug.Log(level.tiles.Length);
		for (int i = 0; i < 11; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				if (level.tiles[j, i] == null)
				{
					level.tiles[j, i] = new TileSimple(root/* + new Vector3(i, 0, j)*/, j, i);
					//level.tiles[j, i].cord.equals(new Tuple<int, int>(j, i));
					level.tiles[j, i].level = level;
				}
			}
		}
		SnakeHead snakeHead = TileManager.instance.BuildSnakeHead();
		level.tiles[4, 4].AddObject(snakeHead);

		for (int i = 0; i < 10; i++)
		{
			level.tiles[0, i].AddObject(TileManager.instance.BuildWall());
			level.tiles[i, 10].AddObject(TileManager.instance.BuildWall());
			level.tiles[10, 10 - i].AddObject(TileManager.instance.BuildWall());
			level.tiles[10 - i, 0].AddObject(TileManager.instance.BuildWall());
		}

		level.tiles[2, 2].AddObject(TileManager.instance.BuildFruit());
		level.tiles[6, 6].AddObject(TileManager.instance.BuildFruit());
		level.tiles[2, 6].AddObject(TileManager.instance.BuildFruit());
		level.tiles[6, 2].AddObject(TileManager.instance.BuildFruit());

		level.StepedOnLevel();
	}
}




