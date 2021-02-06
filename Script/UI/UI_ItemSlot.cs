using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemSlot : UI_BaseSlot<Item>
{
    public override void InitSlot(Item obj)
    {
        m_Object = obj;

        if (m_Object != null)
        {
            m_IconImage.sprite = m_Object.Stat.ItemImage;
            m_IconImage.gameObject.SetActive(true);

            if (m_Info)
            {
                m_Info.text = obj.IsEuipped == true ? "Equipped" : "";
            }
            if (m_Info2)
            {
                m_Info2.text = $"+{obj.ItemLevel.ToString()}";
            }

        }
        else
        {
            m_IconImage.sprite = null;
            m_IconImage.gameObject.SetActive(false);
        }

       

    }

    protected override void UpBtn(PointerEventData data)
    {
        return;
    }
    protected override void ExitDragBtn(PointerEventData data)
    {
        if (MainScroll)
            MainScroll.OnEndDrag(data);

        if (m_Object == null) return;
        UIManager.Instance.OffToolTipBox();
    }
    protected override void DragBtn(PointerEventData data)
    {
        if (MainScroll)
            MainScroll.OnDrag(data);

        if (m_Object == null) return;

        string str = $"Elemental : ";
        str += $"\nDamage : Character Attack * ";

        UIManager.Instance.ShowToolTipBox(data.position, str, m_Object.Name);
    }


    protected override void DownBtn(PointerEventData data)
    {
         OnClickSlot?.Invoke(this);
    }

    protected override void BeginDragBtn(PointerEventData data)
    {
        if (MainScroll)
            MainScroll.OnBeginDrag(data);
    }

    public override void OffHighLightSlot()
    {
        if (m_HighLightImage)
        {
            m_HighLightImage.gameObject.SetActive(false);
        }

    }
    public override void OnHighLightSlot()
    {
        if(m_HighLightImage)
        {
            m_HighLightImage.gameObject.SetActive(true);
        }
    }
}
