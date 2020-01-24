using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : ObjectOnTile
{


	public override void CleanUp()
	{

	}

	public override void Interract (ObjectOnTile snakeHead)
    {
		OnDie();
	}

	public override void OnDie()
	{
        tile.Remove(this);
        returnToPool ();
	}
}