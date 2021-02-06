using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : Singleton<InventoryManager>
{
    Dictionary<E_INVEN_CATEGORY, List<Item>> m_dicItemInventory = new Dictionary<E_INVEN_CATEGORY, List<Item>>();

    public const int MaxSlotCnt = 60;

    public override void InitializeManager()
    {
        //LoadPlayerInvenData();
        //m_dicItemInventory.Clear();
        //m_dicItemInventory.Add(E_INVEN_CATEGORY.EQUIPMENT, new List<Item>());
        //m_dicItemInventory.Add(E_INVEN_CATEGORY.ETC, new List<Item>());
        //m_dicItemInventory.Add(E_INVEN_CATEGORY.QUICKABLE, new List<Item>());

        //Item item = ItemManager.Instance.CreateItem("1001");
        //AddItem(item);

        //Item item2 = ItemManager.Instance.CreateItem("1001");
        //AddItem(item2);

        //Item item3 = ItemManager.Instance.CreateItem("1001");
        //AddItem(item3);

        //Item item4 = ItemManager.Instance.CreateItem("1001");
        //AddItem(item4);
    }

    E_INVEN_CATEGORY GetItemInvenCategory(E_ITEMCATEGORY itemType)
    {
        switch(itemType)
        {
            case E_ITEMCATEGORY.ACC1:
            case E_ITEMCATEGORY.ACC2:
            case E_ITEMCATEGORY.WEAPON:
            case E_ITEMCATEGORY.ARMOR:
                return E_INVEN_CATEGORY.EQUIPMENT;
            case E_ITEMCATEGORY.CONSUMPTION:
                return E_INVEN_CATEGORY.QUICKABLE;
            case E_ITEMCATEGORY.QUEST:
                return E_INVEN_CATEGORY.ETC;
        }

        return E_INVEN_CATEGORY.NONE;
    }


    int SortByName(Item a, Item b)
    {
        return a.Name.CompareTo(b.Name);
    }

    int SortByGrade(Item a, Item b)
    {
        if(a.ItemGrade == b.ItemGrade)
        {
            return a.Name.CompareTo(b.Name);
        }

        return a.ItemGrade.CompareTo(b.ItemGrade);
    }

    public void SortItemByGrade()
    {
        foreach (var list in m_dicItemInventory)
        {
            if (list.Value.Count > 0)
            {
                list.Value.Sort(SortByGrade);
            }
        }

        SavePlayerInvenData();
    }

    public Item GetItem(string ItemUID)
    {
        foreach (var list in m_dicItemInventory)
        {
            foreach (var item in list.Value)
            {
                if(item.ItemUID == ItemUID)
                {
                    return item;
                }
            }
        }

        return null;
    }

    public void SortItemByName()
    {
        foreach (var list in m_dicItemInventory)
        {
            if (list.Value.Count > 0)
            {
                list.Value.Sort(SortByName);
            }
        }

        SavePlayerInvenData();
    }

    public void RemoveItem(Item item)
    {
        var type =  GetItemInvenCategory(item.ItemCategory);
        List<Item> list = GetItemList(type);

        if (list != null)
        {
            list.Remove(item);
        }

        Debug.Log($"Item Remove Success Part : {item.ItemCategory}, itemUID : {item.ItemUID}, item : {item.ItemID}");

        SavePlayerInvenData();
    }

    public void AddItem(Item item)
    {
        var type = GetItemInvenCategory(item.ItemCategory);

        bool isFull = IsFullInventory(type);
        if (isFull) return;

        List<Item> list = GetItemList(type);

        if(list != null)
        {
            list.Add(item);
        }
        else
        {
            list = new List<Item>();
            list.Add(item);
            m_dicItemInventory.Add(type, list);
        }

        SavePlayerInvenData();
    }

    public List<Item> GetItemList(E_INVEN_CATEGORY type)
    {
        List<Item> listItem = null;
        if(m_dicItemInventory.TryGetValue(type, out listItem))
        {
            return listItem;
        }

        return null;
    }

    public bool IsFullInventory(E_INVEN_CATEGORY type)
    {
        List<Item> listItem = null;
        if (m_dicItemInventory.TryGetValue(type, out listItem))
        {
            return listItem.Count > MaxSlotCnt;
        }
        return true;
    }

    public void SavePlayerInvenData()
    {
        SerializeItemTable table = new SerializeItemTable();

        foreach (var itemList in m_dicItemInventory)
        {
            foreach (var item in itemList.Value)
            {
              //  var type = GetItemInvenCategory(item.ItemCategory);
                table.insertItem(item.ItemUID, item.ItemID);
            }
        }
        JsonManager.Instance.SaveJsonPlayerPrefs("myInvenData", table);
    }

    public void LoadPlayerInvenData()
    {
        m_dicItemInventory.Clear();
        m_dicItemInventory.Add(E_INVEN_CATEGORY.EQUIPMENT, new List<Item>());
        m_dicItemInventory.Add(E_INVEN_CATEGORY.ETC, new List<Item>());
        m_dicItemInventory.Add(E_INVEN_CATEGORY.QUICKABLE, new List<Item>());

        SerializeItemTable table = JsonManager.Instance.LoadJsonPlayerPrefs<SerializeItemTable>("myInvenData");
        if (table != null)
        {
            foreach (var i in table.m_Dic)
            {
                Item item = ItemManager.Instance.CreateItem(i.Value, i.Key);
                if (item == null) continue;
                AddItem(item);
            }
        }
    }
}
