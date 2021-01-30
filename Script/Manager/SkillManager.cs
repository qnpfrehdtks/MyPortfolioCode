using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    DataBase<SkillData> m_SkillDB;
    DataBase<BuffData> m_BuffDB;

    public override void InitializeManager()
    {
        m_SkillDB = new DataBase<SkillData>("Data/Skill");
        m_BuffDB = new DataBase<BuffData>("Data/Buff");

        m_SkillDB.LoadData();
        m_BuffDB.LoadData();
    }

    public void UpdateSkillUI(Dictionary<int, Skill> skillTable)
    {
        if (UIManager.Instance.m_CurrentSceneUI == null) return;
        UI_InGame inGameUI = (UIManager.Instance.m_CurrentSceneUI as UI_InGame);

        if (inGameUI == null) return;

        inGameUI.AllInitSkillData(skillTable);
    }

    public Buff CreateBuff(string ID)
    {
        BuffData data = GetBuffData(ID);
        if (data == null) return null;
        Buff newBuff = null;
        switch (data.m_SkillType)
        {
            case E_SKILL_TYPE.MOVE_SLOW:
                newBuff = new MoveSlow_Buff(data);
                break;
            case E_SKILL_TYPE.DOT_DMG:
                newBuff = new DOTDamage_Buff(data);
                break;
        }

        return newBuff;
    }

    public Skill CreateSkill(string ID)
    {
        SkillData info = GetSkillInfo(ID);
        if (info == null) return null;

        SkillFactory factory;
        SkillFactory.CreateFactory(out factory, info.SkillBehavior);

        if (factory == null)
        {
            Debug.Log("Factory Null");
            return null;
        }

        Skill newSkill = factory.CreateSkill(info);
        return newSkill;
    }

    BuffData GetBuffData(string ID)
    {
        return m_BuffDB.GetData(ID) as BuffData;
    }

    SkillData GetSkillInfo(string ID)
    {
        return m_SkillDB.GetData(ID) as SkillData;
    }


}
