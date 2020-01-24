using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlignment : MonoBehaviour
{

	public RectTransform TargetRectTransform;

	public RectTransform SelfRectTransform;

	[SerializeField]
	TextAlignment verticalAlighment = TextAlignment.Center;

	[SerializeField]
	TextAlignment horizontalAlighment = TextAlignment.Center;

	[SerializeField]
	float verticalShift = 0;


	[SerializeField]
	float horizontalShift = 0;
	

	private void Update()
	{
		SelfRectTransform.position.Set(horizontalShift + TargetRectTransform.position.x, verticalShift + TargetRectTransform.position.y,0);
	}
}
