
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackLogicInfo
{
    public float ratio;

    public List<string> m_listBuffID;
    public E_DAMAGE_TYPE m_AttackType;
    public E_ELEMENTAL_TYPE m_ElementalType;
}

    public abstract class Skill
    {
        public ICombatEntity m_Owner { get; set; }
        public SkillData m_skillInfo { get; set; }

        public float currentTime { get; set; }

        public string ID
        {
            get { return m_skillInfo.dataID; }
        }

        public bool CheckCanSkillUse()
        {
            CharacterBase cha = (m_Owner as MonoBehaviour).gameObject.GetComponent<CharacterBase>();
            if (!cha.CheckCanControll()) return false;
            if (currentTime >= 0.01f) return false;
            if (cha.Mana < m_skillInfo.Cost) return false;
            return true;
        }

        public virtual bool UseSkill()
        {
            if (!CheckCanSkillUse()) return false;

            currentTime = m_skillInfo.CoolTime;
            CharacterBase cha = (m_Owner as MonoBehaviour).gameObject.GetComponent<CharacterBase>();
            cha.m_StatController.DoDamageMP(m_skillInfo.Cost);
        
            return true;
        }

        public void UpdateSkill()
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0.01f)
                currentTime = 0.0f;
        }
    }


public class ProjectileSkill : Skill
{
    GameObject BulletPrefab;
    SkillBullet Bullet;

    public void Throw( Vector3 pos, Vector3 forward)
    {
        if(BulletPrefab == null)
        {
            BulletPrefab = (m_skillInfo as ProjectileSkillData).ProjectilePrefab;
            PoolingManager.Instance.CreatePool(BulletPrefab);
        }

        GameObject newBullet = PoolingManager.Instance.PopFromPool(BulletPrefab, pos, Quaternion.identity);
        newBullet.transform.forward = forward;

        Bullet = Common.GetOrAddComponent<SkillBullet>(newBullet);
        Bullet.Init(m_Owner, m_skillInfo);
    }

    public override bool UseSkill()
    {
        bool isSucc = base.UseSkill();

        if (isSucc)
        {
            GameObject obj = (m_Owner as MonoBehaviour).gameObject;
            CharacterBase cha = obj.GetComponent<CharacterBase>();

            if (cha == null) return false;

            if (cha.m_CharacterType == E_CHARACTER_TYPE.PLAYER)
            {
                CharacterBase enemy = CharacterManager.Instance.GetFindClosestCharacter(obj.GetComponent<CharacterBase>(), 5.0f, E_CHARACTER_TYPE.MONSTER);

                if (enemy != null)
                {
                    Vector3 dir = (enemy.transform.position - obj.transform.position).normalized;
                    if (Vector3.Dot(dir, obj.transform.forward) > 0.0f)
                    {
                        obj.transform.forward = (enemy.transform.position - obj.transform.position).normalized;
                    }
                }
            }

            cha.ChangeState(E_CHARACTER_STATE.SKILL);
            cha.SetAniamtionEvent(CastSpell);

            


            return true;
        }

        return false;
    }

    void CastSpell()
    {
        GameObject obj = (m_Owner as MonoBehaviour).gameObject;
        Throw(obj.transform.position + obj.transform.forward * 0.1f + obj.transform.up * 0.15f, obj.transform.forward);
    }
}

