using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/SkillData/Projectile Skill Data", order = int.MaxValue)]

[System.Serializable]
public class ProjectileSkillData : SkillData
{
    [SerializeField]
    float projectile_moveSpeed;
    public float MoveSpeed { get { return projectile_moveSpeed; } }

    [SerializeField]
    float projectile_range;
    public float Range { get { return projectile_range; } }

    [SerializeField]
    GameObject projectilePrefab;
    public GameObject ProjectilePrefab { get { return projectilePrefab; } }
}
