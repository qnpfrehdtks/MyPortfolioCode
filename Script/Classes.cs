using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CostumeSetItemtable
{
    public List<CostumeSetInfo> arrItem = new List<CostumeSetInfo>();
}


[System.Serializable]
public class CostumeSetInfo : ISerializationCallbackReceiver
{
    public int CostumeSetID;
    public Dictionary<E_ITEMCATEGORY, int> dic = new Dictionary<E_ITEMCATEGORY, int>();

    [SerializeField]
    List<E_ITEMCATEGORY> keys;
    [SerializeField]
    List<int> values;


    public CostumeSetInfo(int CostumeSetID, int Chest, int Bottom, int Costume, int hair, int foot, int hand = 2001)
    {
        this.CostumeSetID = CostumeSetID;
    }

    public Dictionary<E_ITEMCATEGORY, int> ToDictionary() { return dic; }

    public void OnBeforeSerialize()
    {
        keys = new List<E_ITEMCATEGORY>(dic.Keys);
        values = new List<int>(dic.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Mathf.Min(keys.Count, values.Count);
        dic = new Dictionary<E_ITEMCATEGORY, int>(count);
        for (var i = 0; i < count; ++i)
        {
            dic.Add(keys[i], values[i]);
        }
    }


    public void insertItem(E_ITEMCATEGORY type, int info)
    {
        dic.Add(type, info);
    }

}


[System.Serializable]
public class ItemInfoTable
{
    public int ID = 2002;
    public List<ItemInfo> arrItem;


    public ItemInfoTable()
    {
        arrItem = new List<ItemInfo>();
    }

    public void insertItem(ItemInfo info)
    {
        arrItem.Add(info);
    }

    public void clearItemInfo()
    {
        arrItem.Clear();
    }

    public void Print()
    {

        for (int idx = 0; idx < arrItem.Count; idx++)
        {
            Debug.Log(string.Format("arrItem[{0}] = {1}", idx, arrItem[idx]));
        }
    }
}


[System.Serializable]
public class DamageInfo
{
    public E_DAMAGE_TYPE attackType;
    public E_ELEMENTAL_TYPE elementalType;
    public int DMG;
    public bool isCriAttack;

    public DamageInfo(E_DAMAGE_TYPE type, E_ELEMENTAL_TYPE elemental, int dmg, bool isCri)
    {
        attackType = type;
        elementalType = elemental;
        DMG = dmg;
        isCriAttack = isCri;
    }
}

[System.Serializable]
public class ItemStatTable
{
    public List<ItemStat> stat = new List<ItemStat>();
}

[System.Serializable]
public class PlayerBuyItemTable : ISerializationCallbackReceiver
{
    public List<int> keys;
    public List<bool> values;
    public Dictionary<int, bool> dic = new Dictionary<int, bool>();

    public void OnBeforeSerialize()
    {
        keys = new List<int>(dic.Keys);
        values = new List<bool>(dic.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Mathf.Min(keys.Count, values.Count);
        dic = new Dictionary<int, bool>(count);
        for (var i = 0; i < count; ++i)
        {
            dic.Add(keys[i], values[i]);
        }
    }

    public void insertItem(int type, bool info)
    {
        dic.Add(type, info);
    }

    public bool getBuyItem(int type)
    {
        bool value = false;
        dic.TryGetValue(type, out value);
        return value;
    }
}

[System.Serializable]
public class ItemInfo
{
    public int ItemID;
    public string ItemName;
    public E_ITEMCATEGORY Type;
    public string Info;
    public string itemResourcePath;
    public int materialID;
    public E_ITEMRANK itemRank;
    public string itemIconResourcePath;
    public int Price;

    public ItemInfo(int itemID, string itemName, E_ITEMCATEGORY type, string info, string resourcePath,  int materialID, E_ITEMRANK itemRank = E_ITEMRANK.NORMAL, int price = 1500)
    {
        ItemID = itemID;
        Type = type;
        Info = info;
        ItemName = itemName;
        itemResourcePath = resourcePath;
        this.materialID = materialID;
        this.itemRank = itemRank;
        Price = price;
    }
}
