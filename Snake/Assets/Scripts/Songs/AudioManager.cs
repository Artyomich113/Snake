using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioClip GachiBass;
	public AudioClip Dimon;

	public AudioSource audioSource;

	private void Awake()
	{
		audioSource.playOnAwake = false;
	}

	void Start()
    {
		GameManager.instanse.GameOver += OnGameOver;
		GameManager.instanse.StartGame += OnGameStart;
    }

	public void OnGameOver(SnakeHead snakeHead)
	{
		audioSource.Stop();
		audioSource.clip = Dimon;
		audioSource.loop = false;
		audioSource.PlayOneShot(audioSource.clip);
	}

	private void OnEnable()
	{
		if(audioSource.clip)
			audioSource.Play();
	}

	private void OnDisable()
	{
		audioSource.Stop();
	}

	public void OnGameStart()
	{
		audioSource.Stop();
		audioSource.clip = GachiBass;
		audioSource.loop = true;
		audioSource.Play();
	}
}
