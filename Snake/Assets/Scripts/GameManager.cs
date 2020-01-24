
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		Menu,
		PreGame,
		Game,
	}

	public InputHandler[] StartInputElements;

	[HideInInspector]
	public GameState gameState;

	public static GameManager instanse;

	List<EnemyAI> enemyAIs = new List<EnemyAI>();

	public CameraControl cameraRef;

	public Action StartGame;

	public Action<SnakeHead> GameOver;

	public Action<AEnemyBody> EnemyKilled;

	public Action Iteration;

	private void Awake()
	{
		if (!instanse)
		{
			instanse = this;
		}
		else
		{
			Destroy(this);
		}
	}

	public EnemyAI GetEnemyAI()
	{
		foreach(var ob in enemyAIs)
		{
			if (ob.someobject.Equals(null))
			{
				return ob;
			}
		}
		EnemyAI enemyAI = new EnemyAI(UnityEngine.Random.Range(0,3),3);
		enemyAIs.Add(enemyAI);
		return enemyAI;
	}



	private void Start()
	{
		gameState = GameState.PreGame;
		foreach(var ob in StartInputElements)
		{
			ob.InputHapaned += StartInputElementsEvent;
		}
		GameOver += OnGameOver;
		//StartGame?.Invoke();
	}

	public void OnGameOver(SnakeHead snakeHead) // called by event
	{
		gameState = GameState.Menu;
		//Debug.Log("gameover");
	}

	public void StartInputElementsEvent()
	{
		//Debug.Log("StartInputElementsEvent");
		if(gameState == GameState.PreGame)
		{

			StartGame?.Invoke();
			gameState = GameState.Game;
		}
	}
}
