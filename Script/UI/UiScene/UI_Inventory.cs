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
    ScrollRect m_ItemSlotRect;

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
        Close,
        Equip,
        UnEquip
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
        AddUIEvent(GetButton((int)InventoryButton.Equip).gameObject, EquipItem, E_UIEVENT.CLICK);
        AddUIEvent(GetButton((int)InventoryButton.UnEquip).gameObject, UnEquipItem, E_UIEVENT.CLICK);

        InitItemSlot();
    }

    void InitItemSlot()
    {
        m_ItemSlotRect = GetUI<ScrollRect>((int)ScrollRectEnum.InvenScroll);
        Transform itemGrid = m_ItemSlotRect.transform.GetComponentInChildren<GridLayoutGroup>().transform;
        for (int i = 0; i < InventoryManager.MaxSlotCnt; i++)
        {
            UI_ItemSlot itemSlot = UIManager.Instance.CreateSlot<UI_ItemSlot>();
            itemSlot.transform.SetParent(itemGrid);
            itemSlot.MainScroll = m_ItemSlotRect;
            itemSlot.OnClickSlot =
            (slot) =>
            {
                if (slot.innerObject == null) return;
                SelectItemSlot(slot as UI_ItemSlot);
            };
            itemSlot.InitSlot(null);
            m_ItemSlot.Add(itemSlot);
        }
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

        InventoryManager.Instance.LoadPlayerInvenData();

        SelectItemSlot(null);
        InitItemSlot(m_currentItemCategory);
        UI_ScrollRectEx.ScrollToTop(m_ItemSlotRect);
    }

    public void SelectItemSlot(UI_ItemSlot slot)
    {
        if(m_SelecteditemSlot)
        {
            m_SelecteditemSlot.OffHighLightSlot();
        }

        m_SelecteditemSlot = slot;

        if (m_SelecteditemSlot)
        {
            m_SelecteditemSlot.OnHighLightSlot();
        }

        UI_ItemInfo info = GetUI<UI_ItemInfo>((int)(ItemInfo.ItemInfoWin));
        if (info != null)
        {
            info.InitIteminfo(m_SelecteditemSlot != null ? m_SelecteditemSlot.innerObject : null);
        }
    }

    void CloseInven(PointerEventData data)
    {
        UIManager.Instance.CloseSceneUI(true);
    }

    void EquipItem(PointerEventData data)
    {
        if (m_SelecteditemSlot == null) return;

        ItemComponent itemComponent = CharacterManager.Instance.m_CurrentMyCharacter.GetComponent<ItemComponent>();
        if(itemComponent.EquipItem(m_SelecteditemSlot.innerObject))
        {
            m_SelecteditemSlot.InitSlot(m_SelecteditemSlot.innerObject);
        }
    }

    void UnEquipItem(PointerEventData data)
    {
        if (m_SelecteditemSlot == null) return;

        ItemComponent itemComponent = CharacterManager.Instance.m_CurrentMyCharacter.GetComponent<ItemComponent>();
        if (itemComponent.UnEquipItem(m_SelecteditemSlot.innerObject.ItemCategory, true) != null)
        {
            m_SelecteditemSlot.InitSlot(null);
        }
    }

    public void InitItemSlot(E_INVEN_CATEGORY type)
    {
        SelectItemSlot(null);

        m_currentItemCategory = type;
        List<Item> items = InventoryManager.Instance.GetItemList(type);

        for(int i = 0; i < m_ItemSlot.Count; i++)
        {
            m_ItemSlot[i].MainScroll = m_ItemSlotRect;

            if (i < items.Count)
                m_ItemSlot[i].InitSlot(items[i]);
            else
                m_ItemSlot[i].InitSlot(null);
        }
    }

}
