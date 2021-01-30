using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Stat Data", menuName = "Scriptable Object/ItemData/Item Data", order = int.MaxValue)]
public class ItemStat : Stat
{
    [SerializeField]
    Sprite m_ItemImage;
    [SerializeField]
    E_ITEMCATEGORY m_ItemCategory;
    [SerializeField]
    E_ITEMGRADE m_ItemGrade;

    string m_UID;

    public Sprite ItemImage { get { return m_ItemImage; }}
    public string UID { get { return m_UID; } set { m_UID = value; } }
    public E_ITEMCATEGORY ItemCategory { get { return m_ItemCategory; }  }
    public E_ITEMGRADE ItemGrade { get { return m_ItemGrade; } }


    public override Stat Clone()
    {
        return new ItemStat(this);
    }

    public ItemStat(ItemStat stat) : base(stat)
    {
        m_ItemCategory = stat.m_ItemCategory;
        m_ItemImage = stat.m_ItemImage;
        m_UID = stat.m_UID;
    }
}
