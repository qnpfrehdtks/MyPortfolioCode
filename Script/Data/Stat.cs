using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Stat Data", menuName = "Scriptable Object/CharacterData/Character Data", order = int.MaxValue)]
public class Stat : ScriptableObject, IMyData
{
    [SerializeField]
    public string m_StatID;
    [SerializeField]
    public string m_Name;

    [SerializeField]
    public float m_LightArmor;
    [SerializeField]
    public float m_DarkArmor;
    [SerializeField]
    public float m_FireArmor;
    [SerializeField]
    public float m_WaterArmor;
    [SerializeField]
    public float m_NatureArmor;
    [SerializeField]
    public float m_LightingArmor;
    [SerializeField]
    public float m_GoldBonus;
    [SerializeField]
    public float m_Vampire;

    [SerializeField]
    public float m_PlayerDetectRange;
    [SerializeField]
    public float m_AttackSpeed;
    [SerializeField]
    public float m_MoveSpeed;
    [SerializeField]
    public float m_CriRate;
    [SerializeField]
    public float m_CriChance;

    [SerializeField]
    public int m_HPRegen;
    [SerializeField]
    public int m_MaxHP;
    [SerializeField]
    public int m_MPRegen;
    [SerializeField]
    public int m_MaxMP;
    [SerializeField]
    public int m_Barrier;
    [SerializeField]
    public int m_Attack;
    [SerializeField]
    public int m_MaxEXP;
    
    [SerializeField]
    public bool m_Invin;

    [SerializeField]
    public GameObject m_GameObject;

    public string dataID { get { return m_StatID; } set { m_StatID = value; } }
    public string myName { get { return m_Name; } set { m_Name = value; } }

    public float PlayerDetectRange { get { return m_PlayerDetectRange; } set { m_PlayerDetectRange = value; } }
    public float AttackSpeed { get { return m_AttackSpeed; } set { m_AttackSpeed = value; } }
    public float MoveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }
    public float CriRate { get { return m_CriRate; } set { m_CriRate = value; } }
    public float CriChance { get { return m_CriChance; } set { m_CriChance = value; } }
    public float GoldBonus { get { return m_GoldBonus; } set { m_GoldBonus = value; } }
    public float Vampire { get { return m_Vampire; } set { m_Vampire = value; } }

    public int HPRegen { get { return m_HPRegen; } set { m_HPRegen = value; } }
    public int MaxHP { get { return m_MaxHP; } set { m_MaxHP = value; } }
    public int MPRegen { get { return m_MPRegen; } set { m_MPRegen = value; } }
    public int MaxMP { get { return m_MaxMP; } set { m_MaxMP = value; } }
    public int Barrier { get { return m_Barrier; } set { m_Barrier = value; } }
    public int Attack { get { return m_Attack; } set { m_Attack = value; } }
    public int MaxEXP { get { return m_MaxEXP; } set { m_MaxEXP = value; } }
    public bool Invin { get { return m_Invin; } set { m_Invin = value; } }
    public GameObject GameObjectPrefab { get { return m_GameObject; } }

    public virtual Stat Clone()
    {
        return new Stat(this);
    }

    public Stat(Stat stat)
    {
        m_StatID = stat.m_StatID;
        m_HPRegen = stat.m_HPRegen;
        m_MaxHP = stat.m_MaxHP;
        m_MPRegen = stat.m_MPRegen;
        m_MaxMP = stat.m_MaxMP;
        m_Barrier = stat.m_Barrier;
        m_Attack = stat.m_Attack;
        m_AttackSpeed = stat.m_AttackSpeed;
        m_MoveSpeed = stat.m_MoveSpeed;
        m_CriRate = stat.m_CriRate;
        m_CriChance = stat.m_CriChance;
        m_Invin = stat.m_Invin;
        m_MaxEXP = stat.m_MaxEXP;
        m_GameObject = stat.m_GameObject;
        m_Name = stat.m_Name;
        m_PlayerDetectRange = stat.m_PlayerDetectRange;
    }

    public static Stat operator +(Stat a, Stat b)
    {
        Stat newStat = a.Clone();
        newStat.m_HPRegen += b.m_HPRegen;
        newStat.m_MaxHP += b.m_MaxHP;
        newStat.m_MPRegen += b.m_MPRegen;
        newStat.m_MaxMP += b.m_MaxMP;
        newStat.m_Barrier += b.m_Barrier;
        newStat.m_Attack += b.m_Attack;
        newStat.m_AttackSpeed += b.m_AttackSpeed;
        newStat.m_MoveSpeed += b.m_MoveSpeed;
        newStat.m_CriRate += b.m_CriRate;
        newStat.m_CriChance += b.m_CriChance;
        newStat.m_PlayerDetectRange += b.m_PlayerDetectRange;
        return newStat;
    }

    public static Stat operator -(Stat a, Stat b)
    {
        Stat newStat = a.Clone();
        newStat.m_HPRegen -= b.m_HPRegen;
        newStat.m_MaxHP -= b.m_MaxHP;
        newStat.m_MPRegen -= b.m_MPRegen;
        newStat.m_MaxMP -= b.m_MaxMP;
        newStat.m_Barrier -= b.m_Barrier;
        newStat.m_Attack -= b.m_Attack;
        newStat.m_AttackSpeed -= b.m_AttackSpeed;
        newStat.m_MoveSpeed -= b.m_MoveSpeed;
        newStat.m_CriRate -= b.m_CriRate;
        newStat.m_CriChance -= b.m_CriChance;
        newStat.m_PlayerDetectRange -= b.m_PlayerDetectRange;
        return newStat;
    }
}
