using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactory<T>
{
    T CreateObject();
}


public abstract class SkillFactory : IFactory<Skill>
{
    public static SkillDamageLogic CreateSkillDamageLogic(SkillData skillInfo)
    {
        SkillDamageLogic newSkill = new SkillDamageLogic();
        newSkill.m_skillInfo = skillInfo;
        return newSkill;
    }

    public abstract Skill CreateObject();

    public static void CreateFactory(out SkillFactory factory , E_SKILL_BEHAVIOR_TYPE type)
    {
        factory = new ProjectileSkillFactory();
        switch (type)
        {
            case E_SKILL_BEHAVIOR_TYPE.PROJECTILE:
                factory = new ProjectileSkillFactory();
                break;
            case E_SKILL_BEHAVIOR_TYPE.INSTANCE:
                factory = new InstanceSkillFactory();
                break;
            case E_SKILL_BEHAVIOR_TYPE.RAYCAST:
                factory = new RayCastSkillFactory();
                break;
            default:
                factory = new ProjectileSkillFactory();
                break;
        }
    }


    public Skill CreateSkill(SkillData info)
    {
        if (info == null)
            return null;

        Skill newSkill = CreateObject();
        newSkill.m_skillInfo = info;

        return newSkill;
    }

}

public class InstanceSkillFactory : SkillFactory
{

    public override Skill CreateObject()
    {
        Skill newSkill = new ProjectileSkill();
        return newSkill;
    }

}

public class RayCastSkillFactory : SkillFactory
{
    public override Skill CreateObject()
    {
        Skill ThowSkillFactory = new ProjectileSkill();
        return ThowSkillFactory;
    }

}

public class ProjectileSkillFactory : SkillFactory
{

    public override Skill CreateObject()
    {
        Skill ThowSkillFactory =  new ProjectileSkill();
        return ThowSkillFactory;
    }

}
