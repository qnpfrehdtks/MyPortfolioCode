using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : MonoBehaviour
{
    CharacterBase m_Character;
    Animator m_Animator;
    RuntimeAnimatorController m_animatorController;
    Dictionary<E_CHARACTER_STATE, List<string>> m_AniTable = new Dictionary<E_CHARACTER_STATE, List<string>>();

    AnimationEventHandler m_AnimEventHandler;

    private void Awake()
    {
        m_AniTable.Add(E_CHARACTER_STATE.IDLE, new List<string> { "Idle" });
        m_AniTable.Add(E_CHARACTER_STATE.DEAD, new List<string> { "Death"});
        m_AniTable.Add(E_CHARACTER_STATE.RUN_DEST, new List<string> { "Run" });
        m_AniTable.Add(E_CHARACTER_STATE.RUN, new List<string> { "Run"});
        m_AniTable.Add(E_CHARACTER_STATE.SKILL, new List<string> { "Skill" });
        m_AniTable.Add(E_CHARACTER_STATE.REVIVE, new List<string> { "Defeat" });
        m_AniTable.Add(E_CHARACTER_STATE.VICTORY, new List<string> { "Dancing", "Defeat" });
        m_AniTable.Add(E_CHARACTER_STATE.JUMP, new List<string> { "Jump"});
    }

    public void Init(CharacterBase character)
    {
        m_Character = character;
        m_Animator = GetComponentInChildren<Animator>();
        m_Animator.enabled = false;
        m_animatorController = m_Animator.runtimeAnimatorController;

        m_AnimEventHandler = Common.GetOrAddComponent<AnimationEventHandler>(character.m_Model);
        m_AnimEventHandler.Init();
    }

    public void SetAnimationEvent(System.Action action)
    {
        m_AnimEventHandler.SetAnimationEvent(action);
    }

    /// <summary>
    ///  코스튬 갈아입을때 애니메이션이 먹통되면 이걸 실행해주세요.
    /// </summary>
    public void ResetAniamtion()
    {
        m_Animator.runtimeAnimatorController = null;
        m_Animator.enabled = false;
        m_Animator.enabled = true;
        m_Animator.runtimeAnimatorController = m_animatorController;
    }

    public float GetAnimationPlayRatio()
    {
        if (m_Animator == null)
        {
            return 0.0f;
        }

        if (m_Animator.enabled)
        {
            return m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        else
            return 0.0f;
    }

    public void PlayAnimation(E_CHARACTER_STATE state, int idx)
    {
        List<string> str;
        if(m_AniTable.TryGetValue(state, out str))
        {
            if (str.Count <= idx)
            {
                idx = str.Count - 1;
            }

            PlayAnimation(str[idx]);
        }
    }

    void PlayAnimation(string aniName)
    {
        if (m_Character == null) return;
        if (m_Animator.enabled)
        {
            m_Animator.SetTrigger(aniName);
        }
        else
        {
            m_Animator.enabled = true;
            m_Animator.SetTrigger(aniName);
        }

    }
}
