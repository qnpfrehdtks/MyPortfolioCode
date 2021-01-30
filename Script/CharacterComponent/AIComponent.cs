using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIRunState : RunState
{
    NavMeshAgent m_NavAgent;
    AIComponent m_CharacterAI;

    /// <summary>
    /// AI가 사용할 스킬입니다. 미리 할당받아 사용.
    /// </summary>
    Skill m_AIWillUseSkill;

    public override void StartState(CharacterBase character)
    {
        if (!m_NavAgent)
            m_NavAgent = Common.GetOrAddComponent<NavMeshAgent>(character.gameObject);

        if (!m_CharacterAI)
            m_CharacterAI = Common.GetOrAddComponent<AIComponent>(character.gameObject);
    }

    public override void UpdateState(CharacterBase character)
    {
        m_NavAgent.speed = character.Stat.MoveSpeed;
        CharacterBase target = null;

        if (m_CharacterAI.TargetCharacter == null)
        {
            target = CharacterManager.Instance.GetFindClosestCharacter(character, character.Stat.PlayerDetectRange, E_CHARACTER_TYPE.PLAYER);
            if (target != null)
            {
                m_CharacterAI.TargetCharacter = target;
            }
        }
        else
        {
            m_NavAgent.SetDestination(m_CharacterAI.TargetCharacter.transform.position);

            if (m_AIWillUseSkill == null)
                m_AIWillUseSkill = character.GetCanUseRanodmSkill();

            if (m_AIWillUseSkill == null)
                return;

            target = m_CharacterAI.TargetCharacter;
            if ((target.transform.position - character.transform.position).magnitude < m_AIWillUseSkill.m_skillInfo.Range && 
                (Vector3.Dot(target.transform.forward, (target.transform.position - character.transform.position) ) > 0.97f ))
            {
                m_NavAgent.SetDestination(character.transform.position);
                m_AIWillUseSkill.UseSkill();
            }
        }
    }

    public override void EndState(CharacterBase character)
    {
        m_AIWillUseSkill = null;
        m_CharacterAI.TargetCharacter = null;
    }
}

public class AIIdleState : IdleState
{
    NavMeshAgent m_NavAgent;

    AIComponent m_CharacterAI;

    public override void StartState(CharacterBase character)
    {
        if (!m_NavAgent)
            m_NavAgent = Common.GetOrAddComponent<NavMeshAgent>(character.gameObject);

        if (!m_CharacterAI)
            m_CharacterAI = Common.GetOrAddComponent<AIComponent>(character.gameObject);

      //  m_NavAgent.isStopped = false;
        m_CharacterAI.TargetCharacter = null;
    }

    public override void UpdateState(CharacterBase character)
    {
        CharacterBase target = CharacterManager.Instance.GetFindClosestCharacter(character, character.Stat.PlayerDetectRange, E_CHARACTER_TYPE.PLAYER);

        if (target == null) return;

        m_CharacterAI.TargetCharacter = target;
        m_NavAgent.speed = character.Stat.MoveSpeed;
        m_NavAgent.SetDestination(target.transform.position);
        character.ChangeState(E_CHARACTER_STATE.RUN);
    }


    public override void EndState(CharacterBase character)
    {

    }
}


public class AIComponent : MonoBehaviour
{
    CharacterBase m_Character;

    CharacterBase m_TargetCharacter;

    public CharacterBase TargetCharacter
    {
        get
        {
            return m_TargetCharacter;
        }
        set
        {
            SetTartget(value);
        }
    }

    public void SetTartget(CharacterBase entity)
    {
        m_TargetCharacter = entity;
    }

    private void Init()
    {
       
    }

 
}
