using UnityEngine;
using System;

public class Swipecontroleer : MonoBehaviour
{

	// public enum SwipeDirection { right, left, up, down, nulldir }

	Vector2 firstTouchPosition;
	Vector2 currentTouchPosition;

	public TileManager tileManager;

	public float holdScreenPart = 0.2f; // while moving touch

	public float endScreenPart = 0.1f; // when end touch phase

	float holdTriggerRange;
	float endTriggerRange;

	bool isDragged;

	//public Action<Direction> OnSwipe;

	SnakeHead snakeHead;

	public void SetMovable(SnakeHead snakeHead)
	{
		//Debug.Log("SetMovable " + snakeHead.name + " " + keyname);
		this.snakeHead = snakeHead;
		
	}

	private void Awake()
	{
		tileManager.onSnakeSpawn += SetMovable;
	}

	private void Start()
	{
		holdTriggerRange = holdScreenPart * Screen.width;
		//Debug.Log ($"HoldTriggerRange {HoldTriggerRange}");
		endTriggerRange = endScreenPart * Screen.width;
		//Debug.Log ($"EndedTriggerRange {EndedTriggerRange}");
		//EndedTriggerRange = endscreenpart * Screen.width;

		isDragged = false;
	}
	// Update is called once per frame
	void Update()
	{
#if UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			var touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
				firstTouchPosition = touch.position;
				isDragged = false;
			}
			if (touch.phase == TouchPhase.Moved && isDragged.Equals(false))
			{
				currentTouchPosition = touch.position;
				if ((currentTouchPosition - firstTouchPosition).magnitude > holdTriggerRange)
				{
					Debug.Log(touch.position);
					isDragged = true;
					triggerDrag(currentTouchPosition - firstTouchPosition);
				}
			}
			if (touch.phase == TouchPhase.Ended && isDragged.Equals(false))
			{
				if ((touch.position - firstTouchPosition).magnitude > endTriggerRange)
				{

					isDragged = true;
					currentTouchPosition = touch.position;

					triggerDrag(currentTouchPosition - firstTouchPosition);
				}
			}

		}
#endif
	}

	Direction definedirection(Vector2 dir)
	{
		//Debug.Log(dir);
		if (Mathf.Abs(dir.x) > Mathf.Sqrt(0.5f))
		{
			return (Mathf.Sign(dir.x) == 1) ? Direction.Right : Direction.Left;
		}
		else if (Mathf.Abs(dir.y) > Mathf.Sqrt(0.5f))
		{
			return (Mathf.Sign(dir.y) == 1) ? Direction.Up : Direction.Down;
		}
		else return Direction.Up;
	}

	void triggerDrag(Vector2 dir)
	{
		Debug.Log (dir + " " + dir.magnitude);
		Direction direction = definedirection(dir.normalized);
		snakeHead?.MoveInDirection(direction);
		//OnSwipe?.Invoke(direction);
	}

	public void SetEnable(bool val)
	{
		enabled = val;
	}

}