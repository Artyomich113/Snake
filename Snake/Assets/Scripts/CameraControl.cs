using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

	void Start ()
	{
		TileManager.instance.onStepedLevel += MoveCameraToLevelInDirection;
	}

	void MoveCameraToLevelInDirection (Level level, Direction direction)
	{
		StartCoroutine (ShiftRigToLevel (level, direction, 0.2f));
	}

	public void RetirnToRootPosition ()
	{
		//Debug.Log("returting camera to root position");
		StartCoroutine (MoveToBy (new Vector3 (5f, 0.0f, 5f), 1f));
	}

	IEnumerator MoveToBy (Vector3 pos, float time)
	{
		float localtime = 0;
		while (localtime < time)
		{
			transform.position = Vector3.Lerp (transform.position, pos, localtime / time);
			localtime += Time.deltaTime;
			yield return null;
		}
		transform.position = pos;
		yield return null;
	}

	IEnumerator ShiftRigToLevel (Level level, Direction direction, float time)
	{
		Vector3 startpos1 = transform.position;
		Vector3 DesiredFinalPosition1 = transform.position + SnakeHead.vector3ByDirection[direction] * 10.0f;
		Vector3 startpos2 = level.root + new Vector3 (5.0f, 0, 5.0f) - SnakeHead.vector3ByDirection[direction] * 10.0f;
		Vector3 DesiredFinalPosition2 = level.root + new Vector3 (5.0f, 0, 5.0f);

		float localtime = 0;
		while (localtime < time)
		{
			transform.position = Vector3.Lerp (startpos1, DesiredFinalPosition1, localtime / time);
			localtime += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate ();
		}
		transform.position = startpos2;
		localtime = 0;
		while (localtime < time)
		{
			transform.position = Vector3.Lerp (startpos2, DesiredFinalPosition2, localtime / time);
			localtime += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate ();
		}
		transform.position = DesiredFinalPosition2;
	}
}