using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMain : SceneMain
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnInitializeScene()
    {
        SoundManager.Instance.PlayBGM(E_BGM.TITLE);
        PoolingManager.Instance.PushAllObjectToPool();
        CharacterManager.Instance.RespawnCharacter(CharacterID.PLAYER_BASEMAGICIAN, new Vector3(3, 0.5f, 3), Quaternion.identity, E_CHARACTER_TYPE.DUMMY_PLAYER);
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.MAIN);
    }

}
