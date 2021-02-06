using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    ItemStat m_Stat;
    ICombatEntity m_Owner;

    int m_ItemLv;

    public bool IsEuipped
    {
        get
        {
            return m_Owner == null ? false : true;
        }
    }

    public ItemStat Stat
    {
        get { return m_Stat; }
    }

    public string Name
    {
        get { return m_Stat.myName; }
    }

    public int ItemLevel
    {
        get { return m_ItemLv; }
    }

    public string ItemUID
    {
        get { return m_Stat.UID; }
    }

    public string ItemID
    {
        get { return m_Stat.dataID; }
    }

    public E_ITEMCATEGORY ItemCategory
    {
        get { return m_Stat.ItemCategory; }
    }

    public E_ITEMGRADE ItemGrade
    {
        get { return m_Stat.ItemGrade; }
    }

    public void Init(ItemStat stat, string UID)
    {
        m_Stat = stat;
        m_Stat.UID = UID;
    }

    public void Equip(ICombatEntity entity)
    {
        if (entity == null) return;

        m_Owner = entity;
        m_Owner.AddStat(m_Stat);
    }

    public void UnEquip()
    {
        if (m_Owner == null) return;

        m_Owner.MinusStat(m_Stat);
        m_Owner = null;
    }

}
