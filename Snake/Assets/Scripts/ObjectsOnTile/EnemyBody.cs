using System;
using UnityEngine;

public class EnemyBody : AEnemyBody, IMovable
{
	float Health;

	//public int code;
	bool Alive = true;

	public override void Interract(ObjectOnTile obj)
	{

	}

	//private void Start()
	//{
	//	code = GetHashCode();
	//}
	private void OnEnable()
	{
		Alive = true;
	}

	private void OnDisable()
	{
		Alive = false;
	}

	public override void OnDie()
	{
		//Debug.Log("enemy " + GetHashCode() + " destroyed");
		DestroySelf?.Invoke();
		GameManager.instanse.EnemyKilled?.Invoke(this);
		RemoveSelfFromTile();
		returnToPool();
	}

	public override Direction Move(Direction direction)
	{
		
			if (tile == null)
				Debug.LogWarning(GetHashCode() + " tile " + tile);
			Tile NextTile = tile.GetTile(direction);
			if (NextTile == null)
			{
				throw new Exception("nexttilenull");
			}
			bool IsWall = false;
			if (NextTile is TileTransition)
			{
				IsWall = true;
			}
			else
			{
				foreach (var ob in NextTile.objectsOnTile)
				{
					if (ob is Wall)
					{
						IsWall = true;
					}
				}
			}

			if (IsWall)
			{
				direction = (Direction)(((int)direction + 2) % 4);
				NextTile = tile.GetTile(direction);
			}

			RemoveSelfFromTile();

			NextTile.AddObject(this, false);
			NextTile.InterractManyToOne(this);
			MoveObject(transform.position, NextTile.position, 0.5f);
		return direction;
	}

	bool lookInDirection(Direction direction)
	{
		//Debug.Log("Looking in direction " + direction);
		Tile localtile = tile;
		int calls = 0;
		while ((localtile = localtile.GetTileOnLevel(direction)) != null)
		{
			//Debug.Log("Looking in " + localtile);
			foreach (var ob in localtile.objectsOnTile)
			{
				if (ob is SnakeHead || ob is SnakeTail)
				{
					//Debug.Log("found snakepart in " + direction);
					return true;
				}
			}
			if (calls++ > 12)
			{
				//Debug.Log("call overload " + calls);
				break;
			}
		}
		return false;
	}

	public override void CleanUp()
	{
		DestroySelf?.Invoke();
	}

	public override void LookForSnake()
	{
		int sides = 0;
		for (int i = 0; i < 4; i++)
		{
			sides += lookInDirection((Direction)i) == true ? 1 : 0;
		}

		//if (sides > 0)
			//Debug.Log("calculate search " + sides);
		if (sides > 1 && Alive)
		{
			//Debug.Log("sides " + sides);
			OnDie();
			Alive = false;
		}
	}
}
