using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemComponent : MonoBehaviour
{
    ICombatEntity m_Character;
    Dictionary<E_ITEMCATEGORY, Item> m_myItemTable;
    public void Initialize(ICombatEntity characterBase)
    {
        m_Character = characterBase;
        LoadPlayerItemData();
    }

    public Item GetEpuippedItem(E_ITEMCATEGORY category)
    {
        Item item = null;
        if (m_myItemTable.TryGetValue(category, out item))
        {
            return item;
        }

        return null;
    }

    public void AllUnEquip()
    {
        foreach (var item in m_myItemTable)
        {
            UnEquipItem(item.Value.ItemCategory , false);
        }

        SavePlayerItemData();
    }

    public Item UnEquipItem(E_ITEMCATEGORY category, bool updateStat)
    {
        Item item = GetEpuippedItem(category);

        if (item != null)
        {
            item.UnEquip();
            m_myItemTable.Remove(category);

            if (updateStat)
                SavePlayerItemData();
        }

        return item;
    }

    public void EquipItem(E_ITEMCATEGORY category, Item item)
    {
        if (item == null) return;
        if (category != item.ItemCategory) return;

        UnEquipItem(category, false);

        item.Equip(m_Character);
        m_myItemTable.Add(category, item);

        SavePlayerItemData();
    }

    public void SavePlayerItemData()
    {
        SerializeItemTable table = new SerializeItemTable();

        foreach (var item in m_myItemTable)
        {
            table.insertItem(item.Value.ItemUID, item.Value.ItemID);
        }
        JsonManager.Instance.SaveJsonPlayerPrefs("myItemData", table);
    }

    public void LoadPlayerItemData()
    {
        m_myItemTable.Clear();

        SerializeItemTable table = JsonManager.Instance.LoadJsonPlayerPrefs<SerializeItemTable>("myItemData");
        if (table != null)
        {
            foreach (var i in table.m_Dic)
            {
                Item item = InventoryManager.Instance.GetItem(i.Value);
                if (item == null) continue;
                EquipItem(item.ItemCategory, item);
            }
        }
    }
}
