using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_CHARACTER_STATE
{
    NONE,
    IDLE,
    RUN,
    RUN_DEST,
    DEAD,
    VICTORY,
    JUMP,
    SKILL,
    CASTING,
    REVIVE,
    END
}


public class FSMComponent : MonoBehaviour
{
    protected CharacterBase m_myCharacter;
    protected ICharacterState m_DefaultState;
    protected ICharacterState m_CurrentState;

    public E_CHARACTER_STATE State 
    {
        get
        {
            if(m_CurrentState == null)
            {
                return E_CHARACTER_STATE.NONE;
            }

            return m_CurrentState.state;
        } 
    }

    protected Dictionary<E_CHARACTER_STATE, ICharacterState> m_dicState = new Dictionary<E_CHARACTER_STATE, ICharacterState>();

    
    public void DestoryComponent()
    {
        ChangeState(m_DefaultState);
        m_dicState.Clear();
        Destroy(this);
    }

    public virtual void Init(CharacterBase myCharacter)
    {
        m_myCharacter = myCharacter;
        m_DefaultState = GetState(E_CHARACTER_STATE.IDLE);
        ChangeState(m_DefaultState);
    }

    private void Update()
    {
        if(m_CurrentState != null)
            m_CurrentState.UpdateState(m_myCharacter);
    }

    public void ResetAllState()
    {
        m_dicState.Clear();
    }

    public void InsertState(ICharacterState state)
    {
        if(!m_dicState.ContainsKey(state.state))
        {
            m_dicState.Add(state.state, state);
        }
    }

    public ICharacterState GetState(E_CHARACTER_STATE state)
    {
        ICharacterState st;
        if (m_dicState.TryGetValue(state, out st))
        {
            return st;
        }

        return null;
    }
    void ChangeState(ICharacterState newState)
    {
        if (newState == null)
            return;

        ChangeState(newState.state);
    }

    public void ChangeState(E_CHARACTER_STATE state)
    {
        if(m_CurrentState != null && m_CurrentState.state == state)
        {
            return;
        }

        if(m_CurrentState != null)
        {
            m_CurrentState.EndState(m_myCharacter);
        }

        ICharacterState newState = GetState(state);

        if (newState != null)
        {
            newState.StartState(m_myCharacter);
            m_myCharacter.PlayAniamtion(newState.state);
            m_CurrentState = newState;
        }
        else
        {
            m_DefaultState.StartState(m_myCharacter);
            m_CurrentState = m_DefaultState;
        }
    }

}
