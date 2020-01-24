using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
	List<Level> levels;
	//public GameObject plane;
	public MeshRenderer plane;
	List<MeshRenderer> Planes;

	//public delegate void OnSnakeSpawn(SnakeHead snakeHead);
	public delegate void OnStepedLevel (Level level, Direction direction);

	public static TileManager instance;

	public System.Action<SnakeHead> onSnakeSpawn;
	public OnStepedLevel onStepedLevel;

	public Objectpool EnemyBodyPool;
	public Objectpool snakeHeadPool;
	public Objectpool Wallpool;
	public Objectpool Fruitpool;
	public Objectpool SnakeTailPool;
	/// <summary>

	public Texture2D StartLevel;

	public Texture2D[] OtherLevels;

	Dictionary<Color, Objectpool> objectPoolByColor;

	public ColorToObjectPool[] colorToObjectPools;
	//public ColorToObjectCollection ColorToObjectCollection;

	/// </summary>

	Director director;

	private void Awake ()
	{
		if (instance == null)
		{
			levels = new List<Level> ();
			Planes = new List<MeshRenderer> ();
			instance = this;

			objectPoolByColor = new Dictionary<Color, Objectpool> ();
			foreach (var ob in colorToObjectPools)
			{
				//Debug.Log("init dictionary " + ob.color + " " + ob.GetType());
				objectPoolByColor.Add (ob.color, ob.objectOnTile);
			}
		}
		else
		{
			Destroy (gameObject);
		}
		//DontDestroyOnLoad (gameObject);
	}

	public EnemyBody BuildEnemyBody ()
	{
		EnemyBody enemyBody = EnemyBodyPool.Get () as EnemyBody;
		enemyBody.gameObject.SetActive (true);
		return enemyBody;
	}

	public SnakeHead BuildSnakeHead ()
	{
		SnakeHead snakeHead = snakeHeadPool.Get () as SnakeHead;
		snakeHead.gameObject.SetActive (true);
		//Debug.Log("new snake" + snakeHead.name);
		onSnakeSpawn?.Invoke (snakeHead);
		return snakeHead;
	}

	public Fruit BuildFruit ()
	{
		Fruit fruit = Fruitpool.Get () as Fruit;
		fruit.gameObject.SetActive (true);
		return fruit;
	}
	public Wall BuildWall ()
	{
		Wall wall = Wallpool.Get () as Wall;
		wall.gameObject.SetActive (true);
		return wall;
	}

	public SnakeTail BuildSnakeTail ()
	{
		SnakeTail snakeTail = SnakeTailPool.Get () as SnakeTail;
		snakeTail.gameObject.SetActive (true);
		return snakeTail;
	}

	private void Start ()
	{
		director = new Director ();

		//GameManager.instanse.StartGame += StartLevel1;
		StartLevel1 ();
	}

	public void StartLevel1 ()
	{
		Level level = AddLevel ();
		//Debug.Log("Startlevel1");

		//ClosedLevelBuilder closedLevelBuilder = new ClosedLevelBuilder();

		//Level level = AddLevel();
		buildLevel (level, texture2D : StartLevel);
		MeshRenderer levelplane = Instantiate (plane, new Vector3 (20f * (levels.Count - 1.0f), 0.0f, 0.0f) + new Vector3 (5.0f, 0.0f, 5.0f), plane.transform.rotation);
		Planes.Add (levelplane);
		level.material = levelplane.material;
		level.StepedOnLevel ();
		//director.Construct(closedLevelBuilder, level, new Vector3(15f * (levels.Count - 1), 0f, 0f), null);

	}

	public void BuildLevel (Gate gate)
	{
		Level level = AddLevel ();
		//Debug.Log("build level by gate");
		//director.Construct(new CasualLevelBuilder(), level, new Vector3(20f * (levels.Count - 1), 0f, 0f), gate);
		buildLevel (level, gate : gate);
		gate.IsLevelBuilded = true;
		MeshRenderer levelplane = Instantiate (plane, new Vector3 (20f * (levels.Count - 1.0f), 0.0f, 0.0f) + new Vector3 (5.0f, 0.0f, 5.0f), plane.transform.rotation);
		Planes.Add (levelplane);
		level.material = levelplane.material;
	}

	public void buildLevel (Level level, Gate gate = null, Texture2D texture2D = null)
	{
		//Level level = AddLevel ();
		level.root = new Vector3 (20f * (levels.Count - 1.0f), 0.0f, 0.0f);
		Texture2D leveltext;
		if (texture2D)
		{
			leveltext = texture2D;
		}
		else
		{
			leveltext = OtherLevels[Random.Range (0, OtherLevels.Length)];
		}

		//	Debug.Log("texture name " + leveltext.name);
		//Debug.Log("building level " + level.GetHashCode());

		for (int x = 0; x < 11; x++)
		{
			for (int y = 0; y < 11; y++)
			{
				Color color = leveltext.GetPixel (x, y);

				Tile tile;

				if (color == Color.black)
				{
					tile = new TileTransition (level.root, x, y);
				}
				else
				{
					tile = new TileSimple (level.root, x, y);
				}

				tile.cord = new Cord (x, y);
				tile.level = level;
				//Debug.Log("tile " + x + " " + y + " assing");
				level.tiles[x, y] = tile;
				if (objectPoolByColor.ContainsKey (color))
				{
					ObjectOnTile objectOnTile = objectPoolByColor[color].Get () as ObjectOnTile;
					if (objectOnTile is SnakeHead)
					{
						onSnakeSpawn.Invoke (objectOnTile as SnakeHead);
					}
					if (objectOnTile is AEnemyBody)
					{
						EnemyAI enemyAI = GameManager.instanse.GetEnemyAI ();
						enemyAI.SetObject (objectOnTile as AEnemyBody);
					}
					objectOnTile.gameObject.SetActive (true);
					//Debug.Log("adding " + objectOnTile.GetType());
					tile.AddObject (objectOnTile);
				}
				else
				{
					//Debug.Log($"{x}:{y} missing Color " + color);
				}
			}
		}

		//Debug.Log("init gates in level " + level.GetHashCode());
		//Debug.Log("up");
		Gate levelgate = new Gate
		{
			direction = Direction.Up
		};
		//Debug.Log(levelgate.direction);
		level.GateByDirection.Add (levelgate.direction, levelgate);
		for (int i = 0; i < level.tiles.GetLength (1); i++)
		{
			if (level.tiles[level.tiles.GetLength (1) - 1, i] is TileTransition)
				levelgate.tileTransitions.Add (level.tiles[level.tiles.GetLength (1) - 1, i] as TileTransition);
		}
		//Debug.Log(levelgate.GetHashCode() + " Gate" + levelgate.direction + " " + levelgate.tileTransitions.Count);

		//Debug.Log("right");
		levelgate = new Gate
		{
			direction = Direction.Right
		};
		//Debug.Log(levelgate.direction);
		level.GateByDirection.Add (levelgate.direction, levelgate);
		for (int i = 0; i < level.tiles.GetLength (0); i++)
		{

			if (level.tiles[i, level.tiles.GetLength (0) - 1] is TileTransition)
				levelgate.tileTransitions.Add (level.tiles[i, level.tiles.GetLength (0) - 1] as TileTransition);
		}
		//Debug.Log(levelgate.GetHashCode() + " Gate" + levelgate.direction + " " + levelgate.tileTransitions.Count);

		//Debug.Log("down");
		levelgate = new Gate
		{
			direction = Direction.Down
		};
		//Debug.Log(levelgate.direction);
		level.GateByDirection.Add (levelgate.direction, levelgate);
		for (int i = 0; i < level.tiles.GetLength (0); i++)
		{

			if (level.tiles[0, i] is TileTransition)
				levelgate.tileTransitions.Add (level.tiles[0, i] as TileTransition);
		}
		//Debug.Log(levelgate.GetHashCode() + " Gate" + levelgate.direction + " " + levelgate.tileTransitions.Count);

		//Debug.Log("left");
		levelgate = new Gate
		{
			direction = Direction.Left
		};
		//Debug.Log(levelgate.direction);
		level.GateByDirection.Add (levelgate.direction, levelgate);
		for (int i = 0; i < level.tiles.GetLength (1); i++)
		{
			if (level.tiles[i, 0] is TileTransition)
				levelgate.tileTransitions.Add (level.tiles[i, 0] as TileTransition);
		}
		//Debug.Log(levelgate.GetHashCode() + " Gate" + levelgate.direction + " " + levelgate.tileTransitions.Count);

		if (gate != null)
		{
			//Debug.Log((Direction)(((int)gate.direction + 2) % 4));
			gate.LinkGate (level.GateByDirection[(Direction) (((int) gate.direction + 2) % 4)]);
		}
	}

	public void CleanUpPlanes ()
	{
		Debug.Log ("Cleaned up " + Planes.Count + " planes");
		for (int i = 0; i < Planes.Count; i++)
		{
			Destroy (Planes[i]);
		}
		Planes.RemoveRange (0, Planes.Count);
	}

	public void CleadAllLevels ()
	{
		//Debug.Log("clear all levels");
		foreach (var ob in levels)
		{
			ob.CleanLevel ();
		}
		levels.RemoveRange (0, levels.Count);
	}

	public Level AddLevel ()
	{
		Level level = new Level ();
		levels.Add (level);
		return level;
	}
}