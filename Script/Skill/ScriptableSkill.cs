using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/SkillData/Skill Data", order = int.MaxValue)]
public class ScriptableSkill : ScriptableObject
{
    public SkillData m_Skill;
}
