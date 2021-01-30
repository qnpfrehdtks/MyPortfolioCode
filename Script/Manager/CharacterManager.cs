using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager : Singleton<CharacterManager>
{
    GameObject m_characterBase;

    public CharacterBase m_CurrentMyCharacter { get; set; }
    public Dictionary<E_CHARACTER_TYPE, List<CharacterBase>> m_dicCurrentCharacter = new Dictionary<E_CHARACTER_TYPE, List<CharacterBase>>();

    DataBase<Stat> m_ChracterDB;

    public override void InitializeManager()
    {
        m_ChracterDB = new DataBase<Stat>("Data/Stat");
        m_ChracterDB.LoadData();
    }

    public void AllPushCharacter()
    {
        foreach (var characterList in m_dicCurrentCharacter)
        {
            foreach (var character in characterList.Value)
            {
                character.PushToPool();
            }

            characterList.Value.Clear();
        }

        m_dicCurrentCharacter.Clear();
    }


    public Stat GetCharacterData(string EnemyID)
    {
        return m_ChracterDB.GetData(EnemyID) as Stat;
    }

    public CharacterBase GetFindClosestCharacter( CharacterBase myChracter, float radius, E_CHARACTER_TYPE CharacterType)
    {
        Vector3 pos = myChracter.transform.position;
        CharacterBase character = null;
        float max = 10000;

        List<CharacterBase> list;

        if (m_dicCurrentCharacter.TryGetValue(CharacterType, out list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (myChracter == list[i])
                    continue;

                pos.y = list[i].transform.position.y;
                float len = (list[i].transform.position - pos).magnitude;
                if (len < max && len < radius)
                {
                    max = len;
                    character = list[i];
                }
            }
        }

        return character;
    }

    public void InsertCharacter(CharacterBase character, E_CHARACTER_TYPE characterType)
    {
        List<CharacterBase> list;
        if (m_dicCurrentCharacter.TryGetValue(characterType, out list))
        {
            list.Add(character);
        }
        else
        {
            list = new List<CharacterBase>();
            list.Add(character);
            m_dicCurrentCharacter.Add(characterType ,list);
        }
    }

    public void RespawnCharacter(string CharacterID, Vector3 _pos, Quaternion _rot , E_CHARACTER_TYPE type)
    {
        IFactory<CharacterBase> factory;
        CharacterFactory.CreateFactory(out factory, type);
        (factory as CharacterFactory).RespawnCharacter(CharacterID, _pos, _rot);
    }
}
