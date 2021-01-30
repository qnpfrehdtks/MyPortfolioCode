using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        if (m_Object == null) return;
        UIManager.Instance.OffToolTipBox();
    }
    protected override void DragBtn(PointerEventData data)
    {
        if (m_Object == null) return;

        string str = $"Elemental : ";
        str += $"\nDamage : Character Attack * ";

        UIManager.Instance.ShowToolTipBox(data.position, str, m_Object.Name);
    }
    protected override void DownBtn(PointerEventData data)
    {
    }

    protected override void BeginDragBtn(PointerEventData data)
    { 
    }
}
