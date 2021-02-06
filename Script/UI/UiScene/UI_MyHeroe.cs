using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_MyHeroe : UI_Base
{
    enum ScrollRectEnum
    {
        StatScroll,
    }

    enum StatButton
    {
        Close
    }

    enum ItemSlot
    {
        Weapom,
        Armor,
        Acc1,
        Acc2
    }


    List<UI_ItemSlot> m_ItemSlot = new List<UI_ItemSlot>();

    private void Awake()
    { 
        BindUI<ScrollRect>(typeof(ScrollRectEnum));
        BindUI<Button>(typeof(StatButton));
        BindUI<UI_ItemSlot>(typeof(ItemSlot));

        AddUIEvent(GetButton((int)StatButton.Close).gameObject,CloseInven, E_UIEVENT.CLICK);
    }

    void CloseInven(PointerEventData data)
    {
        UIManager.Instance.CloseSceneUI(true);
    }

    public override void OnShowUp()
    {
        base.OnShowUp();
        UI_StatSlot[] statSlot = GetUI<ScrollRect>((int)ScrollRectEnum.StatScroll).gameObject.GetComponentsInChildren<UI_StatSlot>();

        if (statSlot == null ) return;
        if (CharacterManager.Instance.m_CurrentMyCharacter == null) return;

        Stat stat = CharacterManager.Instance.m_CurrentMyCharacter.Stat;
        foreach (var slot in statSlot)
        {
            slot.SetStatInfo(stat);
        }

        for (ItemSlot slot = ItemSlot.Weapom; slot <= ItemSlot.Acc2; slot++)
        {
            GetUI<UI_ItemSlot>((int)slot).InitSlot(null);
        }
    }
}
