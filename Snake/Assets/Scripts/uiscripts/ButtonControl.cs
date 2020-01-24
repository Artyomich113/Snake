using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonControl :InputHandler , IPointerDownHandler
{
    public TileManager tileManager;

    public IMovable snakeHead;

    public Direction direction;

	KeyCode keyCode;

	public string keyname;

	//public System.Action InputEvent;

    private void Awake()
    {
		keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyname); ;
        tileManager.onSnakeSpawn += SetMovable;
		//InputEvent += OnInputEvent;
	}

	private void Update()
	{
		if (Input.GetKeyDown(keyCode))
		{
			//InputEvent?.Invoke();
			OnInputEvent();
		}
	}

	public void OnPointerDown (PointerEventData eventData)
    {
		OnInputEvent();
	}

    public void SetMovable(SnakeHead snakeHead)
    {
		//Debug.Log("SetMovable " + snakeHead.name + " " + keyname);
		this.snakeHead = snakeHead;
    }

	public override void OnInputEvent()
	{
		snakeHead?.MoveInDirection(direction);

		InputHapaned?.Invoke();
	}
}