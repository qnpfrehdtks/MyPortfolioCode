using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 버프의 로직을 맡은 클래스입니다.
/// BuffData 에는 버프에 대한 데이터값들이 들어가있습니다.
/// </summary>
public abstract class Buff 
{
    protected ICombatEntity m_Attacker; 
    protected ICombatEntity m_Defender;
    protected BuffData m_BuffData;

    Coroutine m_Coroutine;
    GameObject m_AttachedEffect;

    public Buff(BuffData data)
    {
        m_BuffData = data;
    }

    public string BuffID
    {
        get
        {
            if (m_BuffData == null)
                return string.Empty;

            return m_BuffData.m_BuffID;
        }

    }
    public bool CanDuplicated
    {
        get
        {
            if (m_BuffData == null)
                return false;

            return m_BuffData.m_CanDuplicated;
        }
    }

    public Sprite BuffSprite
    {
        get
        {
            if (m_BuffData == null)
                return null;
            return m_BuffData.m_BuffIcon;
        }
    }

    protected IEnumerator C_UpdateCharacterBuff()
    {
        if (m_BuffData.m_DurationTime <= 0) yield break;

        float time = 0.0f;
        while (time <= m_BuffData.m_DurationTime - 0.01f)
        {
            OnUpdateBuff();

            time += m_BuffData.m_UpdatePeriod;
            yield return new WaitForSeconds(m_BuffData.m_UpdatePeriod);
        }

        EndUpdateAttack();

        if (m_Defender != null)
            m_Defender.RemoveBuff(this);
    }

    public virtual bool OnApplyBuff(ICombatEntity attacker, ICombatEntity defender)
    {
        if (defender == null || attacker == null) return false;

        m_Defender = defender;
        m_Attacker = attacker;

        if (defender.AddBuff(this))
        {
            StartBuff();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StartBuff()
    {
        StopBuffCoroutine();

        m_Coroutine = (m_Defender as MonoBehaviour).StartCoroutine(C_UpdateCharacterBuff());
        Transform tr = (m_Defender as MonoBehaviour).transform;

        if (m_AttachedEffect == null && 
            m_BuffData.m_BuffEffect != E_EFFECT.END && 
            m_BuffData.m_BuffEffect != E_EFFECT.NONE)
        {
            m_AttachedEffect = EffectManager.Instance.PlayEffect_AttachToObject(m_BuffData.m_BuffEffect, Vector3.zero, Quaternion.identity, Vector3.zero, tr);
        }
    }

    protected abstract void OnUpdateBuff();
    public virtual void EndUpdateAttack()
    {
        StopBuffCoroutine();
        EffectManager.Instance.StopEffect(m_AttachedEffect);
        m_AttachedEffect = null;
    }

    public void StopBuffCoroutine()
    {
        if (m_Coroutine != null)
        {
            (m_Defender as MonoBehaviour).StopCoroutine(m_Coroutine);
            m_Coroutine = null;
        }
    }
}

public class DOTDamage_Buff : Buff
{
    public DOTDamage_Buff(BuffData data ) : base(data)
    {

    }
    protected override void OnUpdateBuff()
    {
        BattleManager.Instance.DamageToUnit(m_Defender, m_Attacker, m_BuffData.m_Ratio,
           m_BuffData.m_AttackType.m_AttackType, m_BuffData.m_AttackType.m_ElementalType, E_SKILL_TYPE.DOT_DMG, 0);
    }
}

public class MoveSlow_Buff : Buff
{
    IModifier slow;

    public MoveSlow_Buff(BuffData data) : base(data)
    {

    }

    public override bool OnApplyBuff(ICombatEntity attacker, ICombatEntity defender)
    {
        if (base.OnApplyBuff(attacker, defender))
        {
            slow = new MoveSlowModifier();
            slow.Ratio = m_BuffData.m_Ratio;
            m_Defender.AddModifer(slow);
            return true;
        }

        return false;
    }

    public override void EndUpdateAttack()
    {
        base.EndUpdateAttack();
        m_Defender.RemoveModifier(slow);
    }

    protected override void OnUpdateBuff()
    {
    }
}


