using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBullet : MonoBehaviour, IInteractableObject, IPoolingObject
{
    ICombatEntity m_Owner;
    ProjectileSkillData m_Info;
    float m_MoveRange = 0.0f;

    int m_TeamID;

    public void Init(ICombatEntity owner, SkillData info)
    {
        m_Owner = owner;
        m_TeamID = owner.PlayerTeamID;
        this.m_Info = info as ProjectileSkillData;
        m_MoveRange = 0.0f;
    }

    public void OnEnterCollision(ICombatEntity collisionEntity, Vector3 hitPos)
    {
        if (collisionEntity == m_Owner) return;
        if (m_TeamID == collisionEntity.PlayerTeamID) return;

        EffectManager.Instance.PlayEffect(E_EFFECT.EarthExplosionTiny, hitPos, Quaternion.identity, true, 2.0f);
        SkillDamageLogic logic = SkillFactory.CreateSkillDamageLogic(m_Info);
        logic.ApplySkillToTarget(m_Owner, collisionEntity);
        PoolingManager.Instance.PushToPool(gameObject);
    }
    public void OnExitCollision(ICombatEntity collisionEntity)
    {

    }

    public void Update()
    {
        if (m_Info == null) return;
        if (m_MoveRange <= m_Info.Range)
        {
            m_MoveRange += Time.deltaTime * m_Info.MoveSpeed;
            transform.position += m_Info.MoveSpeed * transform.forward * Time.deltaTime;
        }
        else
        {
            PoolingManager.Instance.PushToPool(gameObject);
        }
    }

    public bool IsInPool { get; set; } = false;
    public int StartCount { get; set; } = 10;
    public void Initialize(GameObject factory)
    {

    }
    public void OnPushToQueue()
    {

    }
    public void OnPopFromQueue()
    {

    }
}
