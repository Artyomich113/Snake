using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public enum Direction
{
	Right,
	Up,
	Left,
	Down,
	idle,
}

public class SnakeHead : SnakePart, IMovable
{
	[SerializeField]
	float NormalTimeToMove = 1f;
	[SerializeField]
	float FasterTimeToMove = 0.5f;

	float TimeToMove;

	float MoveTimer;
	float CurrentExpireance = 0;
	float ExpireanceToLevelUp = 1;

	ScoreHandler scoreHandler;

	public Action<float> OnMoved;

	SnakePart LastSnakePart;
	//public delegate Event OnSnakeDie(SnakeHead snakeHead);

	bool IsDead = false;

	public Direction direction = Direction.idle;

	public static Dictionary<Direction, Vector3> vector3ByDirection = new Dictionary<Direction, Vector3>
	{ { Direction.Right, new Vector3 (1, 0, 0) },
		{ Direction.Left, new Vector3 (-1, 0, 0) },
		{ Direction.Up, new Vector3 (0, 0, 1) },
		{ Direction.Down, new Vector3 (0, 0, -1) }
	};

	public override char Type => 'h';

	private void Awake ()
	{
		/*
		vector3ByDirection = new Dictionary<Direction, Vector3>
		{
			{ Direction.Right, new Vector3(1, 0, 0) },
			{ Direction.Left, new Vector3(-1, 0, 0) },
			{ Direction.Up, new Vector3(0, 0, 1) },
			{ Direction.Down, new Vector3(0, 0, -1) }
		};
	*/
	}

	private void OnEnable ()
	{

		IsDead = false;
		LastSnakePart = this;
		direction = Direction.idle;
		if (GameManager.instanse)
		{
			//Debug.Log(GetHashCode() + "sub");
			GameManager.instanse.EnemyKilled += OnEnemyKilled;
		}
	}

	private void OnDisable ()
	{
		//Debug.Log(GetHashCode() + "unsub");
		GameManager.instanse.EnemyKilled -= OnEnemyKilled;
	}

	private void Start ()
	{

		scoreHandler = new ScoreHandler (true);
		LastSnakePart = this;
		//Debug.Log(GetHashCode() + "sub");
		GameManager.instanse.EnemyKilled += OnEnemyKilled;
	}

	public void ChangeDirection (Direction direction)
	{

		if ((int) direction < 4 && (this.direction.Equals (direction) || Mathf.Abs ((int) this.direction - (int) direction) == 2))
		{
			if (this.direction == direction)
			{
				TimeToMove = FasterTimeToMove;
				ResetAnimation (SoldierAnims.reset);
				PlayAnimation (SoldierAnims.run);
			}
		}
		else
		{
			this.direction = direction;

			transform.rotation = Quaternion.LookRotation (vector3ByDirection[direction], Vector3.up);
			TimeToMove = NormalTimeToMove;
			PlayAnimation (SoldierAnims.reset);
			PlayAnimation (SoldierAnims.walk);
			Move (); // needs deep animation understanding, SpeedUp your animations
		}
	}

	private void FixedUpdate ()
	{
		if (MoveTimer >= TimeToMove)
		{
			Move ();
		}
		else
		{
			MoveTimer += Time.fixedDeltaTime;
		}
	}

	public void Move ()
	{
		if (!direction.Equals (Direction.idle) && !IsDead)
		{
			//Debug.Log("move");
			MoveTimer = 0;
			//LastSnakePart.MoveToNext();
			Tile newtile = tile.GetTile (direction);
			//RotateObject(Quaternion.LookRotation(newtile.position - tile.position),150.0f);
			if (newtile != null)
			{

				tile.Remove (this);
				newtile.interractOneToMany (this);
				newtile.InterractManyToOne (this);
				/*
				foreach (var ob in newtile.objectsOnTile)
				{
					Interract(ob);
				}
				*/

				MoveObject (tile.position, newtile.position, TimeToMove);

				//OnMoved?.Invoke(TimeToMove);
				LastSnakePart.MoveToNext (TimeToMove);
				newtile.AddObject (this, false);

				newtile.level.SetColoredTile (newtile.cord.column, newtile.cord.row);

				GameManager.instanse.Iteration?.Invoke ();
			}
			else
			{
				Debug.Log ("level does not contain tile " + tile.level.GetHashCode ());
			}
		}
	}

	public override void MoveToNext (float time) { }

	public override void Interract (ObjectOnTile InteractionalObject)
	{
		if (InteractionalObject is Wall)
		{
			OnDie ();
			Debug.Log ("Death from Wall");
		}

		if (InteractionalObject is EnemyBody)
		{
			OnDie ();
			Debug.Log ("Death from Enemy");
		}
		if (InteractionalObject is SnakeTail)
		{
			OnDie ();
			Debug.Log ("Death from SnakeTail");
		}
		if (InteractionalObject is Fruit)
			Eat (2);
	}

	public void grow (int count)
	{
		SnakeTail snakeTail;
		for (int i = 0; i < count; i++)
		{
			snakeTail = TileManager.instance.BuildSnakeTail ();
			snakeTail.AttachedSnakePart = LastSnakePart;
			snakeTail.PartNumber = scoreHandler.Lenth;

			snakeTail.onBite += OnTailEaten;
			snakeTail.onLastSnakePart += OnNewLastSnakePart;

			//Debug.Log("new soldier " + snakeTail.PartNumber);
			snakeTail.transform.position = LastSnakePart.transform.position;
			//
			LastSnakePart.tile.AddObject (snakeTail);

			//OnMoved += snakeTail.MoveToNext;

			LastSnakePart = snakeTail;
		}
	}

	public void Eat (float val)
	{
		Taptic.Light ();
		float firstExpirience = CurrentExpireance;
		int StartLenth = scoreHandler.Lenth;
		CurrentExpireance += val;

		while (CurrentExpireance > ExpireanceToLevelUp)
			LeveLUp ();

		//float LastPart = CurrentExpireance / ExpireanceToLevelUp;
		//Debug.Log("LastP" + LastPart + " cur exp" + CurrentExpireance +" exp to lvl up" + ExpireanceToLevelUp);

		ScoreManager.instance.ExpirianceEarned?.Invoke (firstExpirience, CurrentExpireance, ExpireanceToLevelUp);
	}

	void OnEnemyKilled (AEnemyBody aEnemyBody)
	{
		transform.rotation = Quaternion.LookRotation ((aEnemyBody.transform.position - transform.position).normalized);
		PlayAnimation (SoldierAnims.attack01);
		PlayAnimation (SoldierAnims.reset);
		scoreHandler.OnKilledEnemy (1);
		//Debug.Log(GetHashCode() + " Eat enemy");
		Eat (5);
	}

	void LeveLUp ()
	{
		CurrentExpireance -= ExpireanceToLevelUp;
		ExpireanceToLevelUp = (Mathf.Pow (scoreHandler.OnChangedLenth (1), 1.3f));
		//Debug.Log($"new exp to lvl up {ExpireanceToLevelUp}");
		grow (1);
	}

	public override void OnDie ()
	{
		//foreach (var d in OnMoved?.GetInvocationList())
		//{
		//	OnMoved -= (Action<float>)d;
		//}

		if (Advertisement.IsReady ())
		{
			//Debug.Log ("ADS");
			//Advertisement.Show ();
		}

		Taptic.Heavy ();
		//Debug.Log("OnDie");
		ResetAnimation (SoldierAnims.reset);
		PlayAnimation (SoldierAnims.death);
		IsDead = true;
		GameManager.instanse.GameOver?.Invoke (this);
		scoreHandler.CleanUp ();
	}

	public Direction MoveInDirection (Direction direction)
	{
		ChangeDirection (direction);
		return direction;
	}

	public void OnTailEaten (int partnumber)
	{
		scoreHandler.OnChangedLenth (-(scoreHandler.Lenth - partnumber));
		//Debug.Log("tail eaten at " + partnumber);
		//Debug.Log("new lenth " + scoreHandler.Lenth);
		ExpireanceToLevelUp = Mathf.Pow (scoreHandler.Lenth, 1.3f);
		LastSnakePart.DieUntil (partnumber);
	}

	public void OnNewLastSnakePart (SnakePart snakePart)
	{
		LastSnakePart = snakePart;
		//Debug.Log("new last snake part " + snakePart);
	}

	public override void CleanUp ()
	{

	}

	public override void DieUntil (int PartNumber)
	{
		LastSnakePart = this;
	}
}