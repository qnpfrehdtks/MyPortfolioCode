using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneMain : MonoBehaviour
{
    [SerializeField]
    protected E_SCENE_TYPE m_SceneType;
    protected bool m_isInit = false;

    virtual protected void Awake()
    {
        OnInitializeScene();
        SceneChanger.Instance.m_currentMain = this;
    }

    virtual public void ExitSceneInit()
    {
        UIManager.Instance.AllClosePopUI();
        UIManager.Instance.AllCloseSceneUI();
        CharacterManager.Instance.AllPushCharacter();
    }

    public abstract void OnInitializeScene();

}

