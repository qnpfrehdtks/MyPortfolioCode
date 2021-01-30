using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRespawnObject
{
    int RespawnNumber { get; set; }
    void InitializeRespawn(ObjectRespawner respawner);
    void OnRespawn(int idx);
}

/// <summary>
/// 카메라에 닿으면 투명해지는 오브젝트용
/// </summary>
public interface ISlotIcon
{
    // 투명 상태인가? 투명 상태인데 또 투명 만들 필요가 없어서 bool 변수 둠
    bool IsFade { get; set; }
    void EnterDrag();
    void ExitCollisionToCameraRay();
}

/// <summary>
/// 카메라에 닿으면 투명해지는 오브젝트용
/// </summary>
public interface ICameraObstacle
{
    // 투명 상태인가? 투명 상태인데 또 투명 만들 필요가 없어서 bool 변수 둠
    bool IsFade { get; set; }
    void EnterCollisionToCameraRay();
    void ExitCollisionToCameraRay();
}


public interface ICombatEntity
{
    int PlayerTeamID { get;  }
    int HP { get; }
    Stat BaseStats { get; }
    Stat ModifiedStats { get; }
    //  void ApplyDamage(SkillDamageLogic skill, ICombatEntity target);
    // void RemoveAppliedDamage(SkillDamageLogic skill);

    void AddStat(Stat stat);
    void MinusStat(Stat stat);
    bool AddBuff(Buff buff);
    bool RemoveBuff(Buff buff);
    void AddModifer(IModifier modifier);
    bool RemoveModifier(IModifier modifier);
    int TakeDamage(DamageInfo info);
  //  void AddAttackLogic(IAttackLogic attackLogic);
  //  void RemoveAttackLogic(IAttackLogic attackLogic);
    void Dead();
}

public interface IPoolingObject
{
    bool IsInPool { get; set; }
    int StartCount { get; set; }
     void Initialize(GameObject factory);
     void OnPushToQueue();
     void OnPopFromQueue();
}

public interface IInteractableObject
{
    void OnEnterCollision(ICombatEntity character, Vector3 hitPos);
    void OnExitCollision(ICombatEntity character);
}

public interface ICharacterState
{
    E_CHARACTER_STATE state { get;  }
    E_CHARACTER_STATE animationState { get;  }

    void StartState(CharacterBase character);
    void UpdateState(CharacterBase character);
    void EndState(CharacterBase character);
}

// 공격 관련 인터페이스
public interface IAttackLogic
{
    E_DAMAGE_TYPE attackType { get; set;  }
    E_ELEMENTAL_TYPE elementalType { get; set;  }
    void Init(AttackLogicInfo skillInfo, int combo);
    void OnAttack(ICombatEntity attacker, ICombatEntity defender);
    void ResetAttack();
    void OnUpdateAttack();
    void OnEndAttack();
}

public interface IModifier
{
    float Ratio { get; set; }

    void OnStartModify(ICombatEntity entity);
    void AdjustStats(ICombatEntity entity, ref Stat stat);
    void EndModify(ICombatEntity entity);
}


public interface IControllerObserver
{
    int fingerID { get; set; }
    TouchPhase keyCode { get; set; }
    void OnMoved(Touch touch);
    void OnTouchStationary(Touch touch);
    void OnTouchBegan(Touch touch);
    void OnTouchEnded(Touch touch);
}