using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillComponent : MonoBehaviour
{
    Dictionary<int, Skill> m_dicEquippedSkill = new Dictionary<int, Skill>();

    ICombatEntity m_Owner;

    public void Init(ICombatEntity entity, bool MyCharacter)
    {
        AllResetSkill();
        m_Owner = entity;

        Skill skill = SkillManager.Instance.CreateSkill("POISON_BALL");
        InsertSkill(0, skill);

        if(MyCharacter)
            SkillManager.Instance.UpdateSkillUI(m_dicEquippedSkill);
    }

    private void Update()
    {
        foreach(var s in m_dicEquippedSkill)
        {
            s.Value.UpdateSkill();
        }
    }

    public void InsertSkill(int idx, Skill skill)
    {
        if (skill == null) return;

        skill.m_Owner = m_Owner;
        if (m_dicEquippedSkill.ContainsKey(idx))
        {
            m_dicEquippedSkill[idx] = skill;
            return;
        }
        m_dicEquippedSkill.Add(idx, skill);
    }

    public void AllResetSkill()
    {
        m_dicEquippedSkill.Clear();
    }
    public void RemoveSkill(int idx)
    {
        if (m_dicEquippedSkill.Remove(idx))
        {
            return;
        }
    }

    public Skill GetCanUseRandomSkill()
    {
        if (m_dicEquippedSkill.Count == 0) return null;

        List<Skill> list = new List<Skill>(m_dicEquippedSkill.Values);

        List<Skill> result = new List<Skill>();
        foreach(var s in list)
        {
           if( s.CheckCanSkillUse())
            {
                result.Add(s);
            }
        }

        if (result.Count == 0)
            return null;

        return result[Random.Range(0, result.Count)];
    }


    public Skill GetRandomSkill()
    {
        if (m_dicEquippedSkill.Count == 0) return null;

        List<int> list = new List<int>(m_dicEquippedSkill.Keys);
        int ran = UnityEngine.Random.Range(0, list.Count);
        return m_dicEquippedSkill[ran];
    }

    public Skill GetSkillAtIdx(int idx)
    {
        Skill skill = null;
        if (m_dicEquippedSkill.TryGetValue(idx, out skill))
        {
            return skill;
        }

        return null;
    }

    public int CaculateDamageResult(DamageInfo info)
    {
        return info.DMG;
    }

    public int CaculateManaResult(int dmg)
    {
        return dmg;
    }
}
