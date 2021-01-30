using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class SerializeItemTable : ISerializationCallbackReceiver
{
    public Dictionary<string, string> m_Dic = new Dictionary<string, string>();

    [SerializeField]
    List<string> keys;
    [SerializeField]
    List<string> values;

    public void OnBeforeSerialize()
    {
        keys = new List<string>(m_Dic.Keys);
        values = new List<string>(m_Dic.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Mathf.Min(keys.Count, values.Count);
        m_Dic = new Dictionary<string, string>(count);
        for (var i = 0; i < count; ++i)
        {
            m_Dic.Add(keys[i], values[i]);
        }
    }

    public void insertItem(string type, string info)
    {
        m_Dic.Add(type, info);
    }
}

public class ItemManager : Singleton<ItemManager>
{
    DataBase<ItemStat> m_ItemDB;

    public override void InitializeManager()
    {
        m_ItemDB = new DataBase<ItemStat>("Data/Items");
    }

    public string CreateItemUID(ItemStat stat)
    {
         string str1 = System.DateTime.Today.ToString("yyyyMMdd");
         string str2 = System.DateTime.Now.ToString("HHmmss");

        string uid = str1 + str2 + stat.dataID;
        return uid;
    }

    public Item CreateItem(string itemID, string UID = null)
    {
        ItemStat stat = GetItemStat(itemID);

        if (stat == null) return null;

        Item item = new Item();

        if (string.IsNullOrEmpty(UID))
        {
           UID = CreateItemUID(stat);
        }

        item.Init(stat, UID);
        return item;
    }

    public ItemStat GetItemStat(string itemID)
    {
        if (m_ItemDB != null)
        {
            return m_ItemDB.GetData(itemID) as ItemStat;
        }

        return null;
    }
}
