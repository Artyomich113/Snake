using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Score : ScriptableObject
{
    public float score;

	public int lenth;

	public int EnemyKilled;

	public int EnergyErned;

	public int CoinsEarned;

	

	public void CleanUp()
	{
		score = 0;
		lenth = 0;
		EnemyKilled = 0;
		EnergyErned = 0;
		CoinsEarned = 0;
	}
}
