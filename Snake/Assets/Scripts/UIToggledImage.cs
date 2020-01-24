using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggledImage : MonoBehaviour
{
	public Image image;

	public Sprite True;
	public Sprite False;

	public bool StartValue;

	private void Start()
	{
		ToggledImage(StartValue);
	}

	public void ToggledImage(bool val)
	{
		if (val)
		{
			image.sprite = True;
		}
		else
		{
			image.sprite = False;
		}
	}
}
