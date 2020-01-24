using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public abstract class ButtonEvents : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
	
	public Action Click;
	public Action Up;
	public Action Down;
	public Action Enter;
	public Action Exit;

	public void OnPointerClick(PointerEventData eventData)
	{
		//Debug.Log("click ");
		Click?.Invoke();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Down?.Invoke();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Enter?.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Exit?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Up?.Invoke();
	}
}
