using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadState : ICharacterState
{
    float time = 0.0f;
    public E_CHARACTER_STATE state { get; } = E_CHARACTER_STATE.DEAD;
    public E_CHARACTER_STATE animationState { get; } = E_CHARACTER_STATE.DEAD;
    public void StartState(CharacterBase character)
    {
        NavMeshAgent nav = Common.GetOrAddComponent<NavMeshAgent>(character.gameObject);
        if(nav != null)
        {
            nav.isStopped = true;
        }
    }
    public void UpdateState(CharacterBase character)
    {
        time += Time.deltaTime;
        if(time >= 2.0f && character.GetAnimationPlayRatio() >= 0.98f)
        {
            character.PushToPool();
        }
    }
    public void EndState(CharacterBase character)
    {

    }
}

public class SkillState : ICharacterState
{
    public E_CHARACTER_STATE state { get; } = E_CHARACTER_STATE.SKILL;
    public E_CHARACTER_STATE animationState { get; } = E_CHARACTER_STATE.SKILL;

    IEnumerator BackToIdle_C( float time ,CharacterBase character)
    {
        yield return new WaitForSeconds(time);

        character.ChangeState(E_CHARACTER_STATE.IDLE);
    }


    public void StartState(CharacterBase character)
    {
        character.StartCoroutine(BackToIdle_C(1.0f, character));
    }

    public void UpdateState(CharacterBase character){}
    public void EndState(CharacterBase character){}
}

public class IdleState : ICharacterState
{
    public E_CHARACTER_STATE state { get; } = E_CHARACTER_STATE.IDLE;
    public E_CHARACTER_STATE animationState { get; } = E_CHARACTER_STATE.IDLE;
    public virtual void StartState(CharacterBase character){}
    public virtual void UpdateState(CharacterBase character){}
    public virtual void EndState(CharacterBase character){}
}

public class RunState : ICharacterState
{
    public E_CHARACTER_STATE state { get; } = E_CHARACTER_STATE.RUN;
    public E_CHARACTER_STATE animationState { get; } = E_CHARACTER_STATE.RUN;
    public virtual void StartState(CharacterBase character){}
    public virtual void UpdateState(CharacterBase character)
    {
        character.transform.position += character.transform.forward * Time.deltaTime * character.RunSpeed;
    }
    public virtual void EndState(CharacterBase character){}
}

