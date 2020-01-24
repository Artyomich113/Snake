using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Artyomich;

public class ExpirienceProggressBar : MonoBehaviour
{
	public Image FillingImage;

	public Text ResultText;

	IEnumerator coroutine;
	readonly string format = "{0:0.#}";

	float PrevMaxScoreBar;

	public void OnGameStart()
	{
		FillingImage.color = Color.blue;
		FillingImage.fillAmount = 0.0f;
		ResultText.text = "Fruts are to feed army";
	}

	public void OnGameOver(SnakeHead snakeHead)
	{
		ResultText.text = "@_@";
		FillingImage.color = Color.red;
	}

	private void Start()
	{
		ScoreManager.instance.ExpirianceEarned += fill;
		GameManager.instanse.GameOver += OnGameOver;
		GameManager.instanse.StartGame += OnGameStart;
		FillingImage.fillAmount = 0;
		coroutine = Addscore(1, 1, 1,1);
	}

	public void fill(float startExp,float modyfiedExp, float finaleExp)
	{
		//Debug.Log("fill " + startExp + " " + modyfiedExp + " " + finaleExp);
		//FillingImage.fillAmount = startExp;
		ResultText.text = string.Format(format, modyfiedExp) + " / " + string.Format(format, finaleExp);

		StopCoroutine(coroutine);

		coroutine = Addscore(startExp, modyfiedExp, finaleExp, 0.5f);
		StartCoroutine(coroutine);
	}

	IEnumerator Addscore(float startcsore, float finalescore, float MaxScoreBar, float time)
	{
		/*if (string.Format(format, startcsore) != ResultText.text)
		{
			startcsore = float.Parse(ResultText.text.Substring(ResultText.text.IndexOfAny("0123456789".ToCharArray())));
			if (ResultText.text.Contains("neg"))
				startcsore = -startcsore;
		}*/
		
		if(startcsore > finalescore)
		{
			finalescore += PrevMaxScoreBar;
		}
		else
		{
			PrevMaxScoreBar = MaxScoreBar;
		}

		float localtime = 0;

		while (localtime < time)
		{
			float curExp = Mathf.Lerp(startcsore, finalescore, localtime / time);
			ResultText.text = string.Format(format, curExp.FractionalPart(PrevMaxScoreBar)) + "/" + string.Format(format,MaxScoreBar);

			//Debug.Log(curExp + " " + curExp.FractionalPart(MaxScoreBar));
			if(curExp < PrevMaxScoreBar)
			{
				FillingImage.fillAmount = curExp / PrevMaxScoreBar;
			}
			else
			{
				FillingImage.fillAmount = curExp.FractionalPart(PrevMaxScoreBar) / MaxScoreBar;
			}

			localtime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		//Debug.Log($"finale score {score}");
		ResultText.text = string.Format(format, finalescore.FractionalPart(PrevMaxScoreBar)) + "/" + string.Format(format,MaxScoreBar);
		FillingImage.fillAmount = finalescore.FractionalPart(PrevMaxScoreBar) / MaxScoreBar;
		PrevMaxScoreBar = MaxScoreBar;

	}
}
