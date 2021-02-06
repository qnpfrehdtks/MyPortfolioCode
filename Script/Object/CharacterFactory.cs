using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterFactory : IFactory<CharacterBase>
{
    protected E_CHARACTER_TYPE type;
    protected CharacterBase m_newCharacter;
    public abstract CharacterBase CreateObject();

    public static void CreateFactory(out IFactory<CharacterBase> factory, E_CHARACTER_TYPE type)
    {
        switch (type)
        {
            case E_CHARACTER_TYPE.MONSTER:
                factory = new MonsterFactory();
                break;
            case E_CHARACTER_TYPE.NPC:
                factory = new NPCFactory();
                break;
            case E_CHARACTER_TYPE.PLAYER:
                factory = new PlayerFactory();
                break;
            case E_CHARACTER_TYPE.DUMMY_PLAYER:
                factory = new DummyPlayerFactory();
                break;
            default:
                factory = new MonsterFactory();
                break;
        }
    }

    protected Stat newCharacterStat;

    public void RespawnCharacter(string CharacterID, Vector3 _pos, Quaternion _rot)
    {
        newCharacterStat = CharacterManager.Instance.GetCharacterData(CharacterID);

        if (newCharacterStat == null) return;

        GameObject newCharacter = PoolingManager.Instance.PopFromPool(newCharacterStat.GameObjectPrefab, _pos, _rot);
        m_newCharacter = Common.GetOrAddComponent<CharacterBase>(newCharacter);

        CreateObject();

        m_newCharacter.transform.position = _pos;
        m_newCharacter.transform.rotation = _rot;

        CharacterManager.Instance.InsertCharacter(m_newCharacter, type);
    }
}

public class MonsterFactory : CharacterFactory
{
    public MonsterFactory()
    {
        type = E_CHARACTER_TYPE.MONSTER;
    }

    public override CharacterBase CreateObject()
    {
        if (m_newCharacter != null)
        {
            StatController statContoller = null;
            statContoller = Common.GetOrAddComponent<StatController>(m_newCharacter.gameObject);
            statContoller.Init(newCharacterStat.Clone(), 2);

            m_newCharacter.Init(false, type);
            FSMComponent fsm = Common.GetOrAddComponent<FSMComponent>(m_newCharacter.gameObject);
            fsm.ResetAllState();
            fsm.InsertState(new AIRunState());
            fsm.InsertState(new AIIdleState());
            fsm.InsertState(new SkillState());
            fsm.InsertState(new DeadState());
            fsm.Init(m_newCharacter);

            m_newCharacter.gameObject.layer = (int)E_LAYER.Monster;

            return m_newCharacter;
        }

        return null;
    }

}

public class NPCFactory : CharacterFactory
{
    public override CharacterBase CreateObject()
    {
        CharacterBase ThowSkillFactory = new CharacterBase();
        return ThowSkillFactory;
    }

}

public class DummyPlayerFactory : CharacterFactory
{
    public DummyPlayerFactory()
    {
        type = E_CHARACTER_TYPE.PLAYER;
    }

    public override CharacterBase CreateObject()
    {
        if (m_newCharacter != null)
        {
            StatController statContoller = null;
            statContoller = Common.GetOrAddComponent<StatController>(m_newCharacter.gameObject);
            statContoller.Init(newCharacterStat.Clone(), 1);

            m_newCharacter.Init(true, type);

            FSMComponent fsm = Common.GetOrAddComponent<FSMComponent>(m_newCharacter.gameObject);
            fsm.ResetAllState();
            fsm.InsertState(new RunState());
            fsm.InsertState(new IdleState());
            fsm.InsertState(new SkillState());
            fsm.InsertState(new DeadState());
            fsm.Init(m_newCharacter);

            m_newCharacter.gameObject.layer = (int)E_LAYER.Character;

            ItemComponent itemComponent = Common.GetOrAddComponent<ItemComponent>(m_newCharacter.gameObject);
            itemComponent.Initialize(statContoller);

            CharacterManager.Instance.m_CurrentMyCharacter = m_newCharacter;

            return m_newCharacter;
        }

        return null;
    }

}

public class PlayerFactory : CharacterFactory
{
    public PlayerFactory()
    {
        type = E_CHARACTER_TYPE.PLAYER;
    }

    public override CharacterBase CreateObject()
    {
        if (m_newCharacter != null)
        {
            StatController statContoller = null;
            statContoller = Common.GetOrAddComponent<MyStatController>(m_newCharacter.gameObject);
            statContoller.Init(newCharacterStat.Clone(), 1);

            CameraManager.Instance.SetTarget(m_newCharacter.gameObject);
            CameraManager.Instance.SetDistance(4f, 4f);

            m_newCharacter.Init(true, type);

            FSMComponent fsm = Common.GetOrAddComponent<FSMComponent>(m_newCharacter.gameObject);
            fsm.ResetAllState();
            fsm.InsertState(new RunState());
            fsm.InsertState(new IdleState());
            fsm.InsertState(new SkillState());
            fsm.InsertState(new DeadState());
            fsm.Init(m_newCharacter);

            m_newCharacter.gameObject.layer = (int)E_LAYER.Character;

            ItemComponent itemComponent = Common.GetOrAddComponent<ItemComponent>(m_newCharacter.gameObject);
            itemComponent.Initialize(statContoller);

            CharacterManager.Instance.m_CurrentMyCharacter = m_newCharacter;

            return m_newCharacter;
        }

        return null;
    }

}
