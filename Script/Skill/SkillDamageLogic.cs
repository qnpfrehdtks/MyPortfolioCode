using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamageLogic
{
    public SkillData m_skillInfo;
    List<IAttackLogic> m_LogicList = new List<IAttackLogic>();

    void CreateAttackLogic()
    {
        int idx = 0;
        foreach (var l in m_skillInfo.AttackLogic)
        {
            IAttackLogic newLogic = new ConcreteAttackLogic();
            newLogic.Init(l, idx);
            m_LogicList.Add(newLogic);
            idx++;
        }
    }

    IEnumerator C_UpdateCharacterSkill()
    {
        if (m_skillInfo.DurationTime <= 0) yield break;

        float Time = 0.0f;
        while (Time <= m_skillInfo.DurationTime - 0.01f)
        {
            for (int i = 0; i < m_LogicList.Count; i++)
            {
                m_LogicList[i].OnUpdateAttack();
            }

            Time += m_skillInfo.PeriodTime;
            yield return new WaitForSeconds(m_skillInfo.PeriodTime);
        }

       // EndSkill();
    }

    public void StopSkillLogicCoroutine()
    {
        if (SkillCoroutine != null)
        {
            (m_TargetEntity as MonoBehaviour).StopCoroutine(SkillCoroutine);
            SkillCoroutine = null;
        }
    }

    public void ResetAttackLogic()
    {
        StopSkillLogicCoroutine();
        SkillCoroutine = (m_TargetEntity as MonoBehaviour).StartCoroutine(C_UpdateCharacterSkill());
    }

    ICombatEntity m_TargetEntity;
    Coroutine SkillCoroutine;

    int combo = 0;
    public void ApplySkillToTarget(ICombatEntity subject, ICombatEntity target)
    {
        if (subject == null || target == null) return;

        m_LogicList.Clear();
        m_TargetEntity = target;
        CreateAttackLogic();

        foreach (var l in m_LogicList)
        {
            l.OnAttack(subject, target);
        }

        SkillCoroutine = (m_TargetEntity as MonoBehaviour).StartCoroutine(C_UpdateCharacterSkill());
    }

    public void EndSkill()
    {
        for (int i = 0; i < m_LogicList.Count; i++)
        {
            m_LogicList[i].OnEndAttack();
        }
    }
}
