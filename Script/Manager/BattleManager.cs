using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLogicDecorator : IAttackLogic
{
    public E_DAMAGE_TYPE attackType
    {
        get;
        set;
    }
    public E_ELEMENTAL_TYPE elementalType
    {
        get;
        set;
    }
    public SkillDamageLogic characterSkill { get; set; }
    IAttackLogic m_decoratedLogic;
    IModifier m_Modifier;

    public AttackLogicDecorator(SkillDamageLogic skill)
    {
        characterSkill = skill;
    }

    public AttackLogicDecorator(IAttackLogic decoratedLogic)
    {
        m_decoratedLogic = decoratedLogic;
    }

    public virtual void Init(AttackLogicInfo skillInfo,int combo)
    {
        m_decoratedLogic.Init(skillInfo, 0);
    }

    public virtual void ResetAttack()
    {

    }

    public virtual void OnAttack(ICombatEntity attacker, ICombatEntity defender)
    {
        m_decoratedLogic.OnAttack(attacker, defender);
    }

    public virtual void OnUpdateAttack()
    {
        m_decoratedLogic.OnUpdateAttack();
    }

    public virtual void OnEndAttack()
    {
        m_decoratedLogic.OnEndAttack();
    }
}
public class ConcreteAttackLogic : IAttackLogic
{
    public E_DAMAGE_TYPE attackType { 
        get;  
        set; }
    public E_ELEMENTAL_TYPE elementalType { 
        get;  
        set; }

    protected ICombatEntity m_Attacker;
    protected ICombatEntity m_Defender;

    IModifier m_Modifier;

    public IModifier modifier{
        get { return m_Modifier; }
        set { m_Modifier = value; }
    }

    public AttackLogicInfo baseInfo { 
        get { return m_AttackLogicInfo; } 
        set { m_AttackLogicInfo = value; } }

    public float ratio { 
        get { return m_ratio; } 
        set { m_ratio = value; } }

    public int attackLogicCnt { 
        get { return m_attackLogicCnt; } 
        set { m_attackLogicCnt = value; } }

    public int Combo
    {
        get { return m_combo; }
        set { m_combo = value; }
    }

    AttackLogicInfo m_AttackLogicInfo;

    float m_ratio;
    int m_combo;
    int m_attackLogicCnt;

    public void Init(AttackLogicInfo skillInfo, int combo)
    {
        m_AttackLogicInfo = skillInfo;
        attackType = m_AttackLogicInfo.m_AttackType;
        elementalType = m_AttackLogicInfo.m_ElementalType;
        m_combo = combo;
    }

    public virtual void OnAttack(ICombatEntity attacker, ICombatEntity defender)
    {
        m_Attacker = attacker;
        m_Defender = defender;
        m_ratio = m_AttackLogicInfo.ratio;

        BattleManager.Instance.DamageToUnit(m_Defender, m_Attacker, ratio, attackType, elementalType, E_SKILL_TYPE.DMG, m_combo);

       foreach(var buffId in m_AttackLogicInfo.m_listBuffID)
       {
           Buff buff = SkillManager.Instance.CreateBuff(buffId);

           if (buff == null) continue;

           buff.OnApplyBuff(attacker, defender);
       }
    }

    public virtual void OnUpdateAttack()
    {
    }

     public virtual void ResetAttack()
    {
        m_attackLogicCnt = 0;
    }

    public virtual void OnEndAttack()
    {
        if (modifier != null){
            m_Defender.RemoveModifier(modifier);
        }
    }
}

class MoveSlowModifier : IModifier
{
    public float Ratio { get; set; }

    public void OnStartModify(ICombatEntity entity)
    {
    }

    public void AdjustStats(ICombatEntity entity, ref Stat stat)
    {
        stat.MoveSpeed /= Ratio;
    }

    public void EndModify(ICombatEntity entity)
    {
    }
}

////class DOT_DamageLogic : ConcreteAttackLogic
////{
////    public DOT_DamageLogic(SkillDamageLogic skill) : base(skill)
////    {
////    }

////    public override void OnAttack(ICombatEntity attacker, ICombatEntity defender)
////    {
////        base.OnAttack(attacker, defender);
////    }

////    public override void OnUpdateAttack()
////    {
////        base.OnUpdateAttack();
////        /*BattleManager.Instance.DamageToUnit(m_Defender, m_Attacker, ratio, attackType, elementalType, baseInfo.skillType, 0);*/
////        attackLogicCnt++;
////    }
////}

//class MoveSlowLogic : ConcreteAttackLogic
//{
//    public MoveSlowLogic(SkillDamageLogic skill) : base(skill)
//    {
//    }

//    public override void OnAttack(ICombatEntity attacker, ICombatEntity defender)
//    {
//        base.OnAttack(attacker, defender);
//        modifier = new MoveSlowModifier();
//        modifier.Ratio = this.ratio;
//        defender.AddModifer(modifier);

//        BattleManager.Instance.ShowDamagePopup(defender, baseInfo.skillType, Combo);
//    }

//    public override void OnUpdateAttack()
//    {
//        base.OnUpdateAttack();
//    }
//}


public class BattleManager : Singleton<BattleManager>
{

    public override void InitializeManager()
    {
    }


    public int DamageToUnit(ICombatEntity target, ICombatEntity attacker, float ratio, E_DAMAGE_TYPE attackType, E_ELEMENTAL_TYPE elementalType, E_SKILL_TYPE skillType, int cnt)
    {
        bool isCri = false;
        float random = 0;

        random = Random.Range(0.0f, 100.0f);
        int dmg = (int)(attacker.ModifiedStats.Attack * ratio);

        if (attacker.ModifiedStats.CriChance >= random)
        {
            isCri = true;
            dmg = (int)(dmg * attacker.ModifiedStats.CriRate);
        }

        DamageInfo info = new DamageInfo(attackType, elementalType, dmg, isCri);
        if (info == null) return 0;

        int resultDmg = target.TakeDamage(info);

        ShowDamagePopup(target, skillType, cnt, resultDmg, info.isCriAttack);
        return resultDmg;
    }

    public void ShowDamagePopup(ICombatEntity target, E_SKILL_TYPE type, int DamageCnt, int dmg = 0, bool isCri = false)
    {
        if (target == null) return;

        Transform tr = (target as MonoBehaviour).transform;

        Vector3 offset = Vector3.up * 0.75f + (Vector3.up * 0.75f) * DamageCnt;

        switch (type)
        {
            case E_SKILL_TYPE.HEAL:
                UIManager.Instance.ShowDamagePopUp(tr, offset, $"+{dmg}", Color.green, 0.4f, 17);
                break;
            case E_SKILL_TYPE.STUN:
                UIManager.Instance.ShowDamagePopUp(tr, offset, "Stun", Color.white, 0.4f, 17);
                break;
            case E_SKILL_TYPE.MOVE_SLOW:
                UIManager.Instance.ShowDamagePopUp(tr, offset, "Move Slow", Color.white, 0.4f, 17);
                break;
            case E_SKILL_TYPE.MOVE_SPEEDUP:
                UIManager.Instance.ShowDamagePopUp(tr, offset, "Speed Up", Color.white, 0.4f, 17);
                break;
            case E_SKILL_TYPE.ATTACK_SLOW:
                UIManager.Instance.ShowDamagePopUp(tr, offset, "Attack Slow", Color.white, 0.4f, 17);
                break;
            case E_SKILL_TYPE.ATTACK_SPEEDUP:
                UIManager.Instance.ShowDamagePopUp(tr, offset, "Attack SpeedUp", Color.white, 0.4f, 17);
                break;
            default:
                if (dmg <= 0) return;
                if (isCri)
                    UIManager.Instance.ShowDamagePopUp(tr, offset, $"{dmg}", new Color(1, 0, 0), 0.4f, 21);
                else
                    UIManager.Instance.ShowDamagePopUp(tr, offset, $"{dmg}", new Color(1, 0.5f, 0), 0.4f, 17);
                break;
        }
    }

}
