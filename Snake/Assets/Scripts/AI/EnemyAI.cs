using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI
{
	Direction direction = Direction.idle;

	public AEnemyBody someobject;
	
	float MoveRate;
	int CurrentItteration = 0;

	public EnemyAI(int direction, float moverate, EnemyBody someEnemy = null)
	{
		MoveRate = moverate;
		GameManager.instanse.Iteration += OnItteration;

		this.direction = (Direction)(direction % 4);
		someobject = someEnemy;
		if (someEnemy)
			someobject.DestroySelf += ObjectDestroyed;
		
	}

	public void SetObject(AEnemyBody aEnemyBody)
	{
		someobject = aEnemyBody;
		aEnemyBody.DestroySelf += ObjectDestroyed;
	}

	public void OnItteration()
	{

		if (++CurrentItteration >= MoveRate && someobject)
		{
			direction = someobject.Move(direction);
			CurrentItteration = 0;
		}
		someobject.LookForSnake();
	}

	void ObjectDestroyed()
	{
		GameManager.instanse.Iteration -= OnItteration;
		someobject.DestroySelf -= ObjectDestroyed;
	}
}
