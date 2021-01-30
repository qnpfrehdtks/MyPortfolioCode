using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/SkillData/Skill Data", order = int.MaxValue)]
public class SkillData : ScriptableObject, IMyData
{
    [SerializeField]
    int lv;
    public int Lv {get {return lv; }}

    [SerializeField]
    string id;
    public string dataID { get { return id; } set { id = value; } }

    [SerializeField]
     string name;
    public string myName { get { return name; } set { name = value; } }

    [SerializeField]
     string def;
    public string definition { get { return def; } }

    [SerializeField]
     Sprite iconPath;
    public Sprite IconPath { get { return iconPath; } }

    [SerializeField]
     float coolTime;
    public float CoolTime { get { return coolTime; } }

    [SerializeField]
    int cost;
    public int Cost { get { return cost; } }

    [SerializeField]
    List<AttackLogicInfo> attackLogic;
    public List<AttackLogicInfo> AttackLogic { get { return attackLogic; } }

    [SerializeField]
    E_SKILL_BEHAVIOR_TYPE skillBehaviorType;
    public E_SKILL_BEHAVIOR_TYPE SkillBehavior { get { return skillBehaviorType; } }

    [SerializeField]
    string nextSkillID;
    public string NextSkillID { get { return nextSkillID; } }

    [SerializeField]
     float durationTime;
    public float DurationTime { get { return durationTime; } }

    [SerializeField]
     float periodTime;
    public float PeriodTime { get { return periodTime; } }

    [SerializeField]
    float range;
    public float Range { get { return range; } }
}


