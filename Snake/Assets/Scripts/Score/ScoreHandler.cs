using System;
using Artyomich;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler
{
	readonly string format = "{0:0.#}";
	public int Lenth;
	public int KillCount;
	public int EnergyErned;
	public int CoinsEarned;
	public float score;
	public int Expiriance;

	public ScoreHandler (bool interracteble)
	{
		Lenth = 1;
		KillCount = 0;
		EnergyErned = 0;
		CoinsEarned = 0;
		score = 0;
		Expiriance = 0;
		if (interracteble)
		{
			//Debug.Log("Activating scoreHandler");
			GameManager.instanse.GameOver += ShowScore;
		}
	}

	public void OnKilledEnemy (int count)
	{
		KillCount += count;
	}

	public int OnChangedLenth (int count)
	{
		//UnityEngine.Debug.Log($"onchangelenth {count}");
		return Lenth += count;
	}
	public void OnCoinsEarned (int value)
	{
		CoinsEarned += value;
	}

	public void OnEnergyEarned (int value)
	{
		EnergyErned += value;
	}

	public void CleanUp ()
	{
		score = 0;
		Lenth = 0;
		KillCount = 0;
		EnergyErned = 0;
		CoinsEarned = 0;
	}

	public void ShowScore (SnakeHead snakeHead)
	{

		//Debug.Log($"killcount {KillCount} lenth {Lenth} coins {CoinsEarned} energy {EnergyErned}");
		//Debug.Log($"killcountscore {KillCountScore()} lenthscore {LenthScore()}");
		RectTransform scoretransform = ScoreManager.instance.ScoreTransform;
		scoretransform.gameObject.SetActive (true);

		HorizontalLayoutGroup RowTransform;

		RowTransform = Extentions.CreateHorizontalLayoutElement ("description", scoretransform, true, true, ScoreManager.instance.UILayer, new Rect ());
		PrintScore ((RectTransform) RowTransform.transform, new string[3] { "Values", "Count", "Score" });

		if (Lenth > 0)
		{
			RowTransform = Extentions.CreateHorizontalLayoutElement ("Lenth", scoretransform, true, true, ScoreManager.instance.UILayer, new Rect ());
			PrintScore ((RectTransform) RowTransform.transform, new string[3] { "lenth", Lenth.ToString (), string.Format (format, LenthScore ()) });
		}

		if (KillCount > 0)
		{
			RowTransform = Extentions.CreateHorizontalLayoutElement ("killCount", scoretransform, true, true, ScoreManager.instance.UILayer, new Rect ());
			PrintScore ((RectTransform) RowTransform.transform, new string[3] { "KillCount", KillCount.ToString (), string.Format (format, KillCountScore ()) });
		}

		CalculateScore ();
		if (score > 0)
		{
			RowTransform = Extentions.CreateHorizontalLayoutElement ("Score", scoretransform, true, true, ScoreManager.instance.UILayer, new Rect ());
			PrintScore ((RectTransform) RowTransform.transform, new string[2] { "Score", string.Format (format, score) });
		}

	}

	public float CalculateScore ()
	{
		score = LenthScore () + KillCountScore ();
		return score;
	}

	public float LenthScore ()
	{
		return (float) Math.Pow (Lenth, 1.5f);
	}

	public float KillCountScore ()
	{
		return KillCount * 5;
	}

	public void FinishUpScore ()
	{
		RectTransform scoretransform = ScoreManager.instance.ScoreTransform;

		HorizontalLayoutGroup[] horizontalLayoutGroup = scoretransform.GetComponentsInChildren<HorizontalLayoutGroup> ();

		foreach (var ob in horizontalLayoutGroup)
		{
			UnityEngine.Object.Destroy (ob.gameObject);
		}
		// for (int i = 0; i < scoretransform.childCount; i++)
		// {
		// 	UnityEngine.Object.Destroy (scoretransform.GetChild (i).gameObject);
		// }
	}

	void PrintScore (RectTransform parent, string[] str)
	{
		for (int i = 0; i < str.Length; i++)
		{
			Extentions.CreateText (parent, str[i], 50, TextAnchor.MiddleCenter, ScoreManager.instance.UIFont, Color.black);
		}
	}
}