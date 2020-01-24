using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UiToggleOnClickEvent : MonoBehaviour, IPointerClickHandler
{
	[System.Serializable]
	public class MyBoolEvent : UnityEvent<bool>
	{ }

	public bool StartValue;

	public MyBoolEvent unityEvent;

	public void OnPointerClick(PointerEventData eventData)
	{
		StartValue = !StartValue;
		unityEvent.Invoke(StartValue);
	}
}
