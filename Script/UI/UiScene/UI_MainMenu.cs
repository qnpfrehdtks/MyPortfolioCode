using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_MainMenu : Ui_Scene
{

    enum Buttons
    {
        PLAY,
        OPTION,
        SKILL,
        HERO,
        ITEM
    }

    public override void Initialize(GameObject factory)
    {
        base.Initialize(factory);

        BindUI<Button>(typeof(Buttons));

        AddUIEvent(GetButton((int)Buttons.HERO).gameObject, DownMyHeroBtn, E_UIEVENT.DOWN);
        AddUIEvent(GetButton((int)Buttons.ITEM).gameObject, DownInventoryBtn, E_UIEVENT.CLICK);
        AddUIEvent(GetButton((int)Buttons.PLAY).gameObject, DownPlayBtn, E_UIEVENT.DOWN);
        AddUIEvent(GetButton((int)Buttons.OPTION).gameObject, ClickOptionBtn, E_UIEVENT.CLICK);
        AddUIEvent(GetButton((int)Buttons.SKILL).gameObject, ClickShopBtn, E_UIEVENT.CLICK);

        Debug.Log("Title Init");
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
        SceneChanger.Instance.LoadScene(E_SCENE_TYPE.IN_GAME);
        Debug.Log("PLAY BUTTON");
    }

    void DownMyHeroBtn(PointerEventData data)
    {
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.MY_HEROES);
        Debug.Log("PLAY BUTTON");
    }

    void DownInventoryBtn(PointerEventData data)
    {
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.INVENTORY);
        Debug.Log("PLAY BUTTON");
    }

    void ClickOptionBtn(PointerEventData data)
    {
        UIManager.Instance.ClosePopUp();
        UIManager.Instance.ShowPopUp("Setting", "Setting", null,
            null,
            (PointerEventData d) =>
            {
                UIManager.Instance.ClosePopUp();
            });
    }

    void ClickShopBtn(PointerEventData data)
    {
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.SHOP);
    }
}
