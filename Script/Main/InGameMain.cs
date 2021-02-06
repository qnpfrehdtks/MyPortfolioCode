using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMain : SceneMain
{



    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnInitializeScene()
    {
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.INGAME);
        TileGenerator.Instance.CreateMap();
        SoundManager.Instance.PlayBGM(E_BGM.IN_GAME);

        CharacterManager.Instance.RespawnCharacter(CharacterID.PLAYER_BASEMAGICIAN, new Vector3(3,0.5f,3), Quaternion.identity, E_CHARACTER_TYPE.PLAYER);
        CharacterManager.Instance.RespawnCharacter(CharacterID.ENEMY_GHOST, new Vector3(5, 0.5f, 5), Quaternion.identity,E_CHARACTER_TYPE.MONSTER);
        CharacterManager.Instance.RespawnCharacter(CharacterID.ENEMY_GHOST, new Vector3(8, 0.5f, 8), Quaternion.identity, E_CHARACTER_TYPE.MONSTER);
        CharacterManager.Instance.RespawnCharacter(CharacterID.ENEMY_GHOST, new Vector3(5, 0.5f, 8), Quaternion.identity, E_CHARACTER_TYPE.MONSTER);
        //  CharacterManager.Instance.CreateOtherCharacter(new Vector3(5, 0.5f, 5), Quaternion.identity);
    }

    public override void ExitSceneInit()
    {
        base.ExitSceneInit();
        
    }
}
