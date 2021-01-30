using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버프의 데이터들을 설정하는 클래스입니다. 스크립터블 오브젝트로 생성해서 사용가능합니다.
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/BuffData/Buff Data", order = int.MaxValue)]
public class BuffData : ScriptableObject, IMyData
{
    public string m_BuffID;
    public string m_Name;

    public float m_DurationTime;
    public float m_UpdatePeriod;
    
    public float m_Ratio;

    public bool m_CanDuplicated;

    public E_EFFECT m_BuffEffect;
    public E_SKILL_TYPE m_SkillType;
    public E_ELEMENTAL_TYPE m_Elemental;
    public Sprite m_BuffIcon;
    public AttackLogicInfo m_AttackType;

    public string dataID { get { return m_BuffID; } set { m_BuffID = value; } }
    public string myName { get { return m_Name; } set { m_Name = value; } }
}
