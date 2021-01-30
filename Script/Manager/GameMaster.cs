using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : Singleton<GameMaster>
{
    public const int ONE_GOLD_VALUE = 20; 

    int m_Gold;
    List<ObjectRespawner> m_ListWoodRespawner = new List<ObjectRespawner>();

    public GameObject m_WoodPrefab; 

    public int Gold
    {
        get
        {
            return m_Gold;
        }
        set
        {
            if(UIManager.Instance.m_CurrentSceneUI != null)
            {
                UIManager.Instance.OnChangeGoldUI(value - m_Gold);
            }
            
            if(value < 0)
            {
                value = 0;
            }

            m_Gold = value;

            PlayerPrefsManager.Instance.SaveKey_int("myGold", m_Gold);
        }
    }

    public System.Action<int> OnGoldChangeAction = null;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void InitializeManager()
    {
        InitGold();
    }

    public void InitGold()
    {
        m_Gold = PlayerPrefsManager.Instance.GetKeyInt("myGold");
    }

    /// <summary>
    /// 내 캐릭터가 골에 들어가면 실행되는 함수
    /// </summary>
    public void OnGameGoal()
    {
        CameraManager.Instance.SetTarget(null);
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.VICTORY);
        PlayerPrefsManager.Instance.SaveKey_int("myGold", Gold + 1000);
        m_Gold += 1000;
    }

}
