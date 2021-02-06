using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class SerializeItemTable : ISerializationCallbackReceiver
{
    public Dictionary<string, string> m_Dic = new Dictionary<string, string>();

    public List<string> keys;
    public List<string> values;

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
        if(m_Dic.ContainsKey(type))
        {
            Debug.LogError($"Duplicated Item UID : {type}, ItemID : {info}");
            return;
        }
        else
        {
            Debug.Log($"Success Insert Equip ItemData!! Item UID : {type}, ItemID : {info}");
        }

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
        string str1 = System.DateTime.Today.ToString("yyMMdd");
        string str2 = System.DateTime.Now.ToString("HHmmss");
        int str3 = Random.Range(0, 99999);
        
        string uid = str1 + str2 + str3 + "_" + stat.dataID;
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

        Debug.Log($"Create Item!! : UID : {UID}, ID : {itemID}");
        item.Init(stat.Clone() as ItemStat, UID);
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
