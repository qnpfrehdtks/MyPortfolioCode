using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Inventory : Ui_Scene
{
    E_INVEN_CATEGORY m_currentItemCategory = E_INVEN_CATEGORY.EQUIPMENT;
    List<UI_ItemSlot> m_ItemSlot = new List<UI_ItemSlot>();
    UI_ItemInfo m_ItemInfo;
    UI_ItemSlot m_SelecteditemSlot;

    enum ScrollRectEnum
    {
       // DefendStatScroll,
        InvenScroll
    }

    enum InventoryButton
    {
        Equipment,
        Quickable,
        ETC,
        Close
    }

    enum ItemInfo
    {
        ItemInfoWin
    }

    enum InventoryGridPanel
    {
        InvenGrid
    }

    private void Awake()
    {
        BindUI<Button>(typeof(InventoryButton));
        BindUI<ScrollRect>(typeof(ScrollRectEnum));
        BindUI<UI_ItemInfo>(typeof(ItemInfo));

        ScrollRect sc = GetUI<ScrollRect>((int)ScrollRectEnum.InvenScroll);

        Transform itemGrid = sc.transform.GetComponentInChildren<GridLayoutGroup>().transform;

        for (int i = 0; i < InventoryManager.MaxSlotCnt; i++)
        {
            UI_ItemSlot itemSlot = UIManager.Instance.CreateSlot<UI_ItemSlot>();
            itemSlot.transform.SetParent(itemGrid);
            itemSlot.InitSlot(null);
            m_ItemSlot.Add(itemSlot);
        }

        AddUIEvent(GetButton((int)InventoryButton.Equipment).gameObject, 
            (PointerEventData data) =>
            {
                InitItemSlot(E_INVEN_CATEGORY.EQUIPMENT);
            }
            , E_UIEVENT.CLICK);
        AddUIEvent(GetButton((int)InventoryButton.ETC).gameObject,
            (PointerEventData data) =>
            {
                InitItemSlot(E_INVEN_CATEGORY.ETC);
            }
            , E_UIEVENT.CLICK);
        AddUIEvent(GetButton((int)InventoryButton.Quickable).gameObject,
            (PointerEventData data) =>
            {
                InitItemSlot(E_INVEN_CATEGORY.QUICKABLE);
            }
            , E_UIEVENT.CLICK);

        AddUIEvent(GetButton((int)InventoryButton.Close).gameObject, CloseInven, E_UIEVENT.CLICK);
    }

    public override void Initialize(GameObject factory)
    {
        base.Initialize(factory);
    }
    /// <summary>
    /// UI를 켤때 실행되는 함수.
    /// </summary>
    public override void OnShowUp()
    {
        base.OnShowUp();
        SelectItemSlot(null);
        InitItemSlot(m_currentItemCategory);
    }

    public void SelectItemSlot(UI_ItemSlot slot)
    {
        m_SelecteditemSlot = slot;
        UI_ItemInfo info = GetUI<UI_ItemInfo>((int)(ItemInfo.ItemInfoWin));
        if (info != null)
        {
            info.InitIteminfo(m_SelecteditemSlot != null ? m_SelecteditemSlot.innerObject : null);
        }
    }

    void CloseInven(PointerEventData data)
    {
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.MAIN);
    }

    public void InitItemSlot(E_INVEN_CATEGORY type)
    {
        m_currentItemCategory = type;
        List<Item> items = InventoryManager.Instance.GetItemList(type);

        for(int i = 0; i < m_ItemSlot.Count; i++)
        {
            if(i < items.Count)
                m_ItemSlot[i].InitSlot(items[i]);
            else
                m_ItemSlot[i].InitSlot(null);
        }
    }

}
