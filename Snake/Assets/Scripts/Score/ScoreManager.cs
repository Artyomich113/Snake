using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreManager : MonoBehaviour
{
	public RectTransform ScoreTransform;

	public Font UIFont;

	public LayerMask UILayer;

	public PlayerData playerData;

	//public System.Action<int> OnLenthChanged;
	//public System.Action<float> OnExpirienceChanged;
	//public System.Action<float> OnEnergyChanged;

	public static ScoreManager instance;

	private void Awake()
	{
		if (!instance)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this);
		}
	}
	
	//start exp // modifyed exp // finale exp
	public Action<float,float,float> ExpirianceEarned;

	public Action<float,int,int> EnergyEarned;

	public Action<int> Coins;

	public Action<int> Killed;

	public Action<int> Lenth;
}
