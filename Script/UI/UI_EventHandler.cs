﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public System.Action<PointerEventData> ClickAction;
    public System.Action<PointerEventData> DownAction;
    public System.Action<PointerEventData> UpAction;
    public System.Action<PointerEventData> OnDragAction;
    public System.Action<PointerEventData> BeginDragAction;
    public System.Action<PointerEventData> ExitDragAction;
      
    public void OnDrag(PointerEventData eventData)
    {
        OnDragAction?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragAction?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ExitDragAction?.Invoke(eventData);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(ClickAction != null)
            SoundManager.Instance.PlaySFX(E_SFX.CLICK);
        ClickAction?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (DownAction != null)
            SoundManager.Instance.PlaySFX(E_SFX.CLICK);
        DownAction?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpAction?.Invoke(eventData);
    }
}
