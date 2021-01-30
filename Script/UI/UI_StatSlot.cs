using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_StatSlot : UI_Base
{
    [SerializeField]
    E_STAT_TYPE m_StatType;

    enum StatText
    {
        Stat
    }

    public void Awake()
    {
        BindUI<TMPro.TextMeshProUGUI>(typeof(StatText));
    }

    public void SetStatInfo(Stat stat)
    {
        switch(m_StatType)
        {
            case E_STAT_TYPE.Attack:
                GetText(0).text = stat.Attack.ToString();
                break;
            case E_STAT_TYPE.AttackSpeed:
                GetText(0).text = stat.AttackSpeed.ToString();
                break;
            case E_STAT_TYPE.CriticalChance:
                GetText(0).text = stat.CriChance.ToString();
                break;
            case E_STAT_TYPE.CriticalDMG:
                GetText(0).text = stat.CriRate.ToString();
                break;
            case E_STAT_TYPE.Defence:
                GetText(0).text = stat.Barrier.ToString();
                break;
            case E_STAT_TYPE.Health:
                GetText(0).text = stat.MaxHP.ToString();
                break;
            case E_STAT_TYPE.Mana:
                GetText(0).text = stat.MaxMP.ToString();
                break;
            case E_STAT_TYPE.MoveSpeed:
                GetText(0).text = stat.MoveSpeed.ToString();
                break;
            case E_STAT_TYPE.Vampire:
                GetText(0).text = stat.Vampire.ToString();
                break;
            case E_STAT_TYPE.GoldBonus:
                GetText(0).text = stat.GoldBonus.ToString();
                break;
            case E_STAT_TYPE.HPRegen:
                GetText(0).text = stat.HPRegen.ToString();
                break;
            case E_STAT_TYPE.ManaRegen:
                GetText(0).text = stat.MPRegen.ToString();
                break;
        }
    }
}
