using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CharacterBase : MonoBehaviour, IPoolingObject
{
    public bool IsInPool { get; set; } = false;
    public int StartCount { get; set; } = 8;

    CapsuleCollider m_CapsuleCollider;
    public GameObject m_Model { get; private set; }

    private FSMComponent m_FSMCompnent;

    private CollisionComponent m_CollisionComponent;

    private SkillComponent m_skillComponent;

    private ItemComponent m_itemComponent;

    private BattleComponent m_battleCompnent;

    private AnimationComponent m_AnimComponent;

    // 플레이어의 캐릭터인가? 아니면 AI 캐릭터인가?
    public bool m_isMyCharacter { get; private set; }

    Rigidbody m_RigidBody;

    public StatController m_StatController;
    public UI_HealthBar m_HealthBar;
    
    public E_CHARACTER_TYPE m_CharacterType;

    public E_CHARACTER_STATE State
    {
        get
        {
            if (m_FSMCompnent == null)
                return E_CHARACTER_STATE.IDLE;

            if (m_FSMCompnent.State == E_CHARACTER_STATE.NONE)
            {
                return E_CHARACTER_STATE.IDLE;
            }

            return m_FSMCompnent.State;
        }
    }

    public bool Invin
    {
        get
        {
            return m_StatController.ModifiedStats.m_Invin;
        }
    }

    public bool IsDead
    {
        get
        {
            return m_FSMCompnent.State == E_CHARACTER_STATE.DEAD;
        }
    }

    public float RunSpeed
    {
        get
        {
            return m_StatController.ModifiedStats.MoveSpeed;
        }
    }

    public int Health
    {
        get
        {
            return m_StatController.HP;
        }
    }

    public int Mana
    {
        get
        {
            return m_StatController.MP;
        }
    }

    public Stat Stat
    {
        get
        {
            return m_StatController.ModifiedStats;
        }
    }

    Shader m_TrShader;
    Shader m_OriginShader;

    public bool CheckCanControll()
    {
        return (State != E_CHARACTER_STATE.SKILL && State != E_CHARACTER_STATE.DEAD);
    }


    void Awake()
    {
        m_TrShader = Shader.Find("Custom/ToonShader_Tr");
        m_OriginShader = Shader.Find("Custom/ToonShader");
        m_RigidBody = GetComponent<Rigidbody>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void Init(bool isMyCharacter, E_CHARACTER_TYPE type)
    {
        m_CharacterType = type;
        m_isMyCharacter = isMyCharacter;

        m_Model = transform.Find("Character").gameObject;
        m_Model.transform.localPosition = Vector3.zero;
        m_Model.transform.localRotation = Quaternion.identity;
         
        m_AnimComponent = Common.GetOrAddComponent<AnimationComponent>(gameObject);
        m_AnimComponent.Init(this);

        m_FSMCompnent = Common.GetOrAddComponent<FSMComponent>(gameObject);
        m_CollisionComponent = Common.GetOrAddComponent<CollisionComponent>(gameObject);
        m_StatController = Common.GetOrAddComponent<StatController>(gameObject);

        m_StatController.OnDead = () => {
            m_FSMCompnent.ChangeState(E_CHARACTER_STATE.DEAD);
            gameObject.layer = LayerMask.NameToLayer("Default");
            m_CapsuleCollider.enabled = false;
        };

        m_skillComponent = Common.GetOrAddComponent<SkillComponent>(gameObject);
        m_skillComponent.Init(m_StatController, isMyCharacter);

        m_CapsuleCollider.enabled = true;
        SetPhysicalBody(false);
        InitHealthBar();
    }

    
    public void InitHealthBar()
    {
        if (m_isMyCharacter || m_HealthBar != null) return;
        m_HealthBar = UIManager.Instance.AttachHealthBarToObject(gameObject, Vector3.up * 1.4f , TextBarStyle.NONE);
        if (m_HealthBar == null) return;
        m_StatController.ConnectToStatHealthBar(m_HealthBar.m_UIProgressBar);
    }


    public void SetPhysicalBody(bool active)
    {
        if(active)
        {
            m_RigidBody.isKinematic = false;
            m_RigidBody.useGravity = true;
            m_CapsuleCollider.isTrigger = false;
        }
        else
        {
            m_RigidBody.isKinematic = true;
            m_RigidBody.useGravity = false;
            m_CapsuleCollider.isTrigger = true;
            m_RigidBody.velocity = Vector3.zero;
        }
    }

    public void ForceToChracter(Vector3 dir)
    {
        SetPhysicalBody(true);
        m_RigidBody.velocity = dir;
    }

    public void Initialize(GameObject factory)
    {
        transform.SetParent(factory.transform);
    }

    public void SetAniamtionEvent(System.Action a)
    {
        if (m_AnimComponent != null)
        {
            m_AnimComponent.SetAnimationEvent(a);
        }
    }

    public void OnPushToQueue()
    {
        ChangeState(E_CHARACTER_STATE.IDLE);

        if (m_HealthBar)
        {
            PoolingManager.Instance.PushToPool(m_HealthBar.gameObject);
            m_HealthBar = null;
        }
    }

    public void OnPopFromQueue()
    {
        ChangeState(E_CHARACTER_STATE.IDLE);
    }

    public void ChangeState(E_CHARACTER_STATE state)
    {
        if(m_FSMCompnent)
            m_FSMCompnent.ChangeState(state);
    }

    public float GetAnimationPlayRatio()
    {
        if (m_AnimComponent == null)
            return 0.0f;

        return m_AnimComponent.GetAnimationPlayRatio();
    }

    public void PlayAniamtion(E_CHARACTER_STATE state, int idx = 0)
    {
        if (m_AnimComponent == null)
            return;

        m_AnimComponent.PlayAnimation(state, idx);
    }

    public void ConsumeMP(int MP)
    {
        int dmg = 0;

        if (m_skillComponent){
            dmg = m_skillComponent.CaculateManaResult(MP);
        }

        if (m_itemComponent){
          //  dmg = m_itemComponent.CaculateManaResult(MP);
        }

        m_StatController.DoDamageMP(dmg);
    }

    public int CaculateDamageResult(DamageInfo dmg)
    {
        int result = 0;
        if(m_itemComponent)
        {
          //  result = m_itemComponent.CaculateDamageResult(dmg);
        }
        if (m_skillComponent)
        {
            result = m_skillComponent.CaculateDamageResult(dmg);
        }

        return result;
    }

    public Skill GetCanUseRanodmSkill()
    {
        if(m_skillComponent)
        {
            return m_skillComponent.GetCanUseRandomSkill();
        }

        return null;
    }



#if UNITY_EDITOR
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace) && m_isMyCharacter)
        {
        //    SkillDamageLogic skill = SkillManager.Instance.CreateSkillDamageLogic(1001);
        //    m_StatController.ApplyDamage(skill, m_StatController);
        }

    }
#endif

    public void PushToPool()
    {
        PoolingManager.Instance.PushToPool(gameObject);
    }
}
