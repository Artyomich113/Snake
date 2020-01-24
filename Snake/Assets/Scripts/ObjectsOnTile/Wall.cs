using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : ObjectOnTile
{
	public override void CleanUp()
	{

	}

	public override void Interract (ObjectOnTile obj)
    {

    }

	public override void OnDie()
	{
		tile.Remove(this);
		returnToPool();
	}
}