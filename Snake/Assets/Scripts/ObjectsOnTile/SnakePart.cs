using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SnakePart : ObjectOnTile
{
	public enum SoldierAnims
	{
		run,
		attack01,
		attack02,
		death,
		reset,
		walk
	};

	public static Dictionary<SoldierAnims, string> AnimationByEnum = new Dictionary<SoldierAnims, string>
	{
		{SoldierAnims.run,"Run"},
		{SoldierAnims.attack01,"Attack01"},
		{SoldierAnims.attack02,"Attack02"},
		{SoldierAnims.death,"death"},
		{SoldierAnims.reset,"reset"},
		{SoldierAnims.walk,"walk"},
	};

	public Animator animator;

	public void ResetAnimation(SoldierAnims soldierAnims)
	{
		animator.ResetTrigger(AnimationByEnum[soldierAnims]);
	}

	public void PlayAnimation(SoldierAnims soldierAnims)
	{
		animator.SetTrigger(AnimationByEnum[soldierAnims]);
	}
	public abstract void DieUntil(int PartNumber);

    public abstract void MoveToNext (float timetomove);

    public abstract char Type { get; }

}