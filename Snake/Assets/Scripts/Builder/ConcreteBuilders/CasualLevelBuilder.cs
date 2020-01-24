using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CasualLevelBuilder : Builder
{

	public override void BuildLevel(Level level, Vector3 root, Gate gate)
	{
		//Debug.Log("casual level builder");
		Direction TransitionGateDirection = (Direction)(((int)gate.direction + 2) % 4);

		level.root = root;

		//Debug.Log("building transition gate");
		//gate in current level
		Gate TransitionGate = level.buildGate(gate.tileTransitions[1].cord + new Tuple<Direction, int>(TransitionGateDirection, 10), TransitionGateDirection);


		for (int i = 0; i < 3; i++)
		{
			//Debug.Log(gate.tileTransitions[i] + " " + TransitionGate.tileTransitions[i]);
			TransitionGate.tileTransitions[i].TileByDirection.Add(TransitionGate.direction, gate.tileTransitions[i]);
			gate.tileTransitions[i].TileByDirection.Add(gate.direction, TransitionGate.tileTransitions[i]);
		}

		//Debug.Log("building transition gate");
		if (randomizer.Chance(50f))
			level.buildGate(new Cord(0, 5), Direction.Down);
		if (randomizer.Chance(50f))
			level.buildGate(new Cord(10, 5), Direction.Up);
		if (randomizer.Chance(50f))
			level.buildGate(new Cord(5, 0), Direction.Left);
		if (randomizer.Chance(50f))
			level.buildGate(new Cord(5, 10), Direction.Right);

		for (int i = 0; i < 11; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				if (level.tiles[j, i] == null)
				{
					level.tiles[j, i] = new TileSimple(root, j, i);
					level.tiles[j, i].cord.equals(new Tuple<int, int>(j, i));
					level.tiles[j, i].level = level;
				}
			}
		}

		for (int i = 0; i < 11; i++)
		{
			level.tiles[0, i].AddObject(TileManager.instance.BuildWall());
			level.tiles[i, 10].AddObject(TileManager.instance.BuildWall());
			level.tiles[10, 10 - i].AddObject(TileManager.instance.BuildWall());
			level.tiles[10 - i, 0].AddObject(TileManager.instance.BuildWall());
		}

		EnemyAI enemyAI = GameManager.instanse.GetEnemyAI();
		//Debug.Log("building enemybody on " + level.tiles[3, 3]);

		enemyAI.SetObject(level.tiles[3, 3].AddObject(TileManager.instance.BuildEnemyBody()) as AEnemyBody);


		level.tiles[1, 1].AddObject(TileManager.instance.BuildFruit());
		level.tiles[9, 1].AddObject(TileManager.instance.BuildFruit());
		level.tiles[1, 9].AddObject(TileManager.instance.BuildFruit());
		level.tiles[9, 9].AddObject(TileManager.instance.BuildFruit());
	}
}

