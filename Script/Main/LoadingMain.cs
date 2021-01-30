using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingMain : SceneMain
{

    protected override void Awake()
    {
        base.Awake();
    }
    public override void OnInitializeScene()
    {
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.LOADING);
        SceneChanger.Instance.LoadReservedScene();
    }

}
