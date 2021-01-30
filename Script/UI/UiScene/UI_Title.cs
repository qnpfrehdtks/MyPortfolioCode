using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Title : Ui_Scene
{

    enum Buttons
    {
        PLAY,
    }

    public override void Initialize(GameObject factory)
    {
        base.Initialize(factory);
        BindUI<Button>(typeof(Buttons));

        AddUIEvent(GetButton((int)Buttons.PLAY).gameObject, DownPlayBtn, E_UIEVENT.DOWN);
    }

    /// <summary>
    /// UI를 켤때 실행되는 함수.
    /// </summary>
    public override void OnShowUp()
    {
        base.OnShowUp();

    }
    void DownPlayBtn(PointerEventData data)
    {
        UIManager.Instance.CloseSceneUI();
        SceneChanger.Instance.LoadScene(E_SCENE_TYPE.MAIN);

        Debug.Log("PLAY BUTTON");

    }
}
