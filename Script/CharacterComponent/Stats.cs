using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatController : MonoBehaviour, ICombatEntity
{
    public int PlayerTeamID { get { return m_playerID; } }
    public int Lv { get { return m_Lv; }  }
    public int HP { get { return m_currentHP; }  }
    public int MP { get { return m_currentMP; } }
    public int EXP { get { return m_Exp; } }
    public Stat BaseStats { get { return m_baseStats; } }

    public Stat ModifiedStats { get { return m_modifiedStats; } }

    int m_playerID;
    int m_Lv;
    int m_currentHP;
    int m_currentMP;
    int m_Exp;

    private Stat m_baseStats;
    private Stat m_modifiedStats;

    public System.Action OnDead;
    protected System.Action<int, int> OnDamageHP;
    protected System.Action<int, int> OnHealHP;
    protected System.Action<int, int> OnDamageMP;
    protected System.Action<int, int> OnHealMP;
    protected System.Action<int, int> OnUpdateEXP;
    protected System.Action<int> OnUpdateLV;

    private List<IModifier> m_ListModifiers = new List<IModifier>();
    private List<Buff> m_ListBuff = new List<Buff>();

    public virtual void Init(Stat stat, int teamID)
    {
        m_playerID = teamID;
        m_baseStats = stat;
        m_modifiedStats = stat.Clone();

        m_currentHP = stat.MaxHP;
        m_currentMP = stat.MaxMP;
        m_Exp = 40;
        m_Lv = 3;
    }

    public int CaculateDamage(float ratio, ref bool isCri)
    {
        float random = Random.Range(0.0f, 100.1f);
        int dmg = 0;

        if (ModifiedStats.CriChance >= random)
        {
            dmg = (int)(ModifiedStats.Attack * ModifiedStats.CriRate * ratio);
            isCri = true;
        }
        else
        {
            dmg = (int)(ModifiedStats.Attack * ratio);
            isCri = false;
        }

        return dmg;
    }
    public float GetEXPRatio()
    {
        return EXP / m_modifiedStats.MaxEXP;
    }
    public float GetHPRatio()
    {
        return HP / m_modifiedStats.MaxHP;
    }
    public float GetMPRatio()
    {
        return MP / m_modifiedStats.MaxMP;
    }
    public void DoDamageMP(int dmg)
    {
        m_currentMP -= dmg;
        m_currentMP = Mathf.Clamp(HP, 0, m_modifiedStats.MaxMP);

        if (dmg > 0)
            OnDamageMP?.Invoke(MP, m_modifiedStats.MaxMP);
        else if (dmg < 0)
            OnHealMP?.Invoke(MP, m_modifiedStats.MaxMP);
    }
    int CaculateDamageResult(DamageInfo info)
    {
        return info.DMG;
    }
    public int TakeDamage(DamageInfo info)
    {
        if (m_modifiedStats.m_Invin)
            return 0;

        int dmg = CaculateDamageResult(info);

        m_currentHP -= dmg;
        m_currentHP = Mathf.Clamp(HP, 0, m_modifiedStats.MaxHP);

        if (HP <= 0)
        {
            Dead();
        }

        if(info.attackType == E_DAMAGE_TYPE.HEAL)
            OnHealHP?.Invoke(HP, m_modifiedStats.MaxHP);
        else
            OnDamageHP?.Invoke(HP, m_modifiedStats.MaxHP);

        return dmg;
   
    }
    public void Dead()
    {
        AllRemoveModifier();
        AllRemoveBuff();

        OnDead?.Invoke();
    }
    public void EarnEXP(int exp)
    {
        if (exp <= 0)
            return;

        int otherEXP = exp - (m_modifiedStats.MaxEXP - EXP);
        m_Exp += exp;

        if (EXP >= m_modifiedStats.MaxEXP)
        {
            LvUp();
            EarnEXP(otherEXP);
        }

        OnUpdateEXP?.Invoke(EXP, m_modifiedStats.MaxEXP);
    }
    void LvUp()
    {
        m_Lv += 1;
        m_Exp = 0;
        OnUpdateLV?.Invoke(Lv);
    }
    public void ConnectToStatHealthBar(UI_ProgressBar healthBar)
    {
        OnDamageHP = (int current, int max) => {
            healthBar.SetProgressBar(current, max);
        };

        OnHealHP = (int current, int max) => {
            healthBar.SetProgressBar(current, max);
        };
    }

    #region Modifier Stat을 수정하는 부분.
    private void ApplyModifiers()
    {
        var stats = m_baseStats.Clone();
        foreach (var mod in m_ListModifiers)
        {
            mod.AdjustStats(this ,ref stats);
        }

        m_modifiedStats = stats;
        m_currentHP = Mathf.Clamp(HP, 0, m_modifiedStats.MaxHP);
        m_currentMP = Mathf.Clamp(MP, 0, m_modifiedStats.MaxMP);
    }
    public void AddModifer(IModifier modifier)
    {
        modifier.OnStartModify(this);
        m_ListModifiers.Add(modifier);
        ApplyModifiers();
    }
    public bool RemoveModifier(IModifier modifier)
    {
        if(m_ListModifiers.Remove(modifier))
        {
            ApplyModifiers();
            return true;
        }

        return false;
    }
    public bool AllRemoveModifier()
    {
        m_ListModifiers.Clear();

        return false;
    }
    #endregion 

    #region Buff 담당
    bool CanInsertBuff(Buff buff)
    {
        foreach(var b in m_ListBuff)
        {
            if (b.BuffID == buff.BuffID)
            {
                if (b.CanDuplicated)
                {
                    return true;
                }
                else
                {
                    b.StartBuff();
                    return false;
                }
            }
        }

        return true;
    }
    public bool AddBuff(Buff buff)
    {
        if (CanInsertBuff(buff))
        {
            m_ListBuff.Add(buff);
            return true;
        }

        return false;
    }
    public void AllRemoveBuff()
    {
        foreach(var b in  m_ListBuff)
        {
            b.EndUpdateAttack();
        }

        m_ListBuff.Clear();
    }
    public bool RemoveBuff(Buff buff)
    {
        if (m_ListBuff.Remove(buff))
        {
            return false;
        }

        return true;
    }
    #endregion

    public void AddStat(Stat stat)
    {
        m_baseStats = m_baseStats + stat;
        ApplyModifiers();
    }

    public void MinusStat(Stat stat)
    {
        m_baseStats = m_baseStats - stat;
        ApplyModifiers();
    }
}

public class MyStatController : StatController
{
    public override void Init(Stat stat, int teamID)
    {
        base.Init(stat, teamID);

        UI_InGame inGameUI = (UIManager.Instance.m_CurrentSceneUI as UI_InGame);

        if (inGameUI == null) return;

        OnDamageHP = (int current, int max) => {
            (inGameUI).OnUpdateHPBar(current, max);
        };

        OnDamageMP = (int current, int max) => {
            (inGameUI).OnUpdateMPBar(current, max);
        };

        OnUpdateEXP = (int current, int max) => {
            (inGameUI).OnUpdateEXPBar(current, max);
        };

        OnUpdateLV = (int current) => {
            (inGameUI).OnUpdateLv(current);
        };

        inGameUI.InitCharacterHUD(Lv,HP, MP,EXP, ModifiedStats, transform);
    }
}


