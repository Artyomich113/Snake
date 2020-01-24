using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class AEnemyBody: ObjectOnTile,IMovable
{
	public Action DestroySelf;

	public Direction MoveInDirection(Direction direction)
	{
		return Move(direction);
	}

	public abstract void LookForSnake();

	public abstract Direction Move(Direction direction);
}

