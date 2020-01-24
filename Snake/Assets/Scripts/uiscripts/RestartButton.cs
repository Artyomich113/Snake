using UnityEngine.UI;


class RestartButton : ButtonEvents
{
	public Image image = null;
	//

	private void Start()
	{
		Click += TileManager.instance.CleadAllLevels;
		Click += TileManager.instance.CleanUpPlanes;
		Click += GameManager.instanse.cameraRef.RetirnToRootPosition;
		Click += TileManager.instance.StartLevel1;
		Click += OnClick;
		GameManager.instanse.GameOver += OnGameOver;

	}

	void OnClick()
	{
		//UnityEngine.Debug.Log("RestartButton Off");
		GameManager.instanse.gameState = GameManager.GameState.PreGame;
		image.enabled = false;
		new ScoreHandler(false).FinishUpScore();
		ScoreManager.instance.ScoreTransform.gameObject.SetActive(false);
	}

	void OnGameOver(SnakeHead snakeHead)
	{
		//UnityEngine.Debug.Log("RestartButton On");
		image.enabled = true;
	}
}