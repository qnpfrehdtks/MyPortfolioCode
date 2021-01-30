using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UI_BaseSlot<T> : UI_Base
{
    protected enum UIImage
    {
        SlotImage = 0
    }

    protected Image m_IconImage;
    protected T m_Object;

    public T innerObject { get { return m_Object; } }

    protected virtual void Awake()
    {
        BindUI<Image>(typeof(UIImage));
        m_IconImage = GetImage((int)(UIImage.SlotImage));

        AddUIEvent(gameObject, ExitDragBtn, E_UIEVENT.EXIT_DRAG);
        AddUIEvent(gameObject, BeginDragBtn, E_UIEVENT.BEGIN_DRAG);
        AddUIEvent(gameObject, DragBtn, E_UIEVENT.DRAG);
        AddUIEvent(gameObject, UpBtn, E_UIEVENT.UP);
        AddUIEvent(gameObject, DownBtn, E_UIEVENT.DOWN);
    }

    public abstract void InitSlot(T obj);

    protected abstract void BeginDragBtn(PointerEventData data);
    protected abstract void DragBtn(PointerEventData data);
    protected abstract void ExitDragBtn(PointerEventData data);
    protected abstract void UpBtn(PointerEventData data);
    protected abstract void DownBtn(PointerEventData data);

}
