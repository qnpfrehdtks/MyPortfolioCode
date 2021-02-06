using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UI_BaseSlot<T> : UI_Base
{
    protected enum UIText
    {
        InfoText = 0,
        InfoText2,
    }

    protected enum UIImage
    {
        SlotImage = 0,
        HighLightImage,
    }

    protected TMPro.TextMeshProUGUI m_Info;
    protected TMPro.TextMeshProUGUI m_Info2;

    protected Image m_HighLightImage;
    protected Image m_IconImage;
    protected T m_Object;

    ScrollRect m_MainScroll;
    System.Action<UI_BaseSlot<T>> m_OnHighLightSlot;
    System.Action<UI_BaseSlot<T>> m_OffHighLightSlot;

    public System.Action<UI_BaseSlot<T>> OffClickSlot { get { return m_OffHighLightSlot; } set { m_OffHighLightSlot = value; } }
    public System.Action<UI_BaseSlot<T>> OnClickSlot { get { return m_OnHighLightSlot; } set { m_OnHighLightSlot = value; } }
    public ScrollRect MainScroll { get { return m_MainScroll; } set { m_MainScroll = value; } }
    public T innerObject { get { return m_Object; } }


    protected virtual void Awake()
    {
        BindUI<Image>(typeof(UIImage));
        BindUI<TMPro.TextMeshProUGUI>(typeof(UIText));

        m_IconImage = GetImage((int)(UIImage.SlotImage));
        m_HighLightImage = GetImage((int)(UIImage.HighLightImage));

        m_Info = GetText((int)(UIText.InfoText));
        m_Info2 = GetText((int)(UIText.InfoText2));

        if(m_Info)
        {
            m_Info.text = "";
        }
        if (m_Info2)
        {
            m_Info2.text = "";
        }

        AddUIEvent(gameObject, ExitDragBtn, E_UIEVENT.EXIT_DRAG);
        AddUIEvent(gameObject, BeginDragBtn, E_UIEVENT.BEGIN_DRAG);
        AddUIEvent(gameObject, DragBtn, E_UIEVENT.DRAG);
        AddUIEvent(gameObject, UpBtn, E_UIEVENT.UP);
        AddUIEvent(gameObject, DownBtn, E_UIEVENT.DOWN);

        m_HighLightImage.gameObject.SetActive(false);
    }

    public abstract void InitSlot(T obj);
    protected abstract void BeginDragBtn(PointerEventData data);
    protected abstract void DragBtn(PointerEventData data);
    protected abstract void ExitDragBtn(PointerEventData data);
    protected abstract void UpBtn(PointerEventData data);
    protected abstract void DownBtn(PointerEventData data);
    public abstract void OnHighLightSlot();
    public abstract void OffHighLightSlot();
}
