using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Shop : Ui_Scene
{
    enum Buttons
    {
        NORMAL,
        UNIQUE,
        LEGEND,
        OK,
        BUY,
        END
    }


    enum ShopButtons
    {
        COSTUME1,
        COSTUME2,
        COSTUME3,
        COSTUME4,
        COSTUME5,
        COSTUME6,
        COSTUME7,
        COSTUME8,
        COSTUME9,
        END
    }

    E_ITEMRANK m_CurrentPressedItemRank = E_ITEMRANK.END;
    Button m_BuyButton;
    TMPro.TextMeshProUGUI m_BuyBtnText;

    [SerializeField]
    List<int> m_ListCostume = new List<int>();

    [SerializeField]
    List<int> m_ListUniqueCostume = new List<int>();

    [SerializeField]
    List<int> m_ListLegendCostume = new List<int>();

    enum InputFields
    {
        PLAYER_INPUT
    }

    public override void OnShowUp()
    {
        base.OnShowUp();
        CameraManager.Instance.SetCameraMode(E_CAMERA_TYPE.NONE);
        CameraManager.Instance.SetDistance(-3.52f, 0.82f);
    }


    void OffAllSelectButton()
    {
        for (ShopButtons shopButton = ShopButtons.COSTUME1; shopButton < ShopButtons.END; shopButton++)
        {
            ShopButton otherBtn = GetUI<ShopButton>((int)shopButton);
            otherBtn.IsSelected = false;
        }
    }

    protected void Start()
    {

        BindUI<Button>(typeof(Buttons));
        BindUI<ShopButton>(typeof(ShopButtons));

        for (ShopButtons t = ShopButtons.COSTUME1; t < ShopButtons.END; t++)
        {
            GameObject btn = GetUI<ShopButton>((int)t).gameObject;
            AddUIEvent(
            btn, 
            (PointerEventData data) =>
            {
                ShopButton shopbtn = btn.GetComponentInChildren<ShopButton>();
                SelectCostume(shopbtn);
            }, 
            E_UIEVENT.DOWN);
        }

        AddUIEvent(GetButton((int)Buttons.OK).gameObject, PressOKButton, E_UIEVENT.CLICK);

        AddUIEvent(GetButton((int)Buttons.NORMAL).gameObject, PressNormalTabButton, E_UIEVENT.DOWN);
        AddUIEvent(GetButton((int)Buttons.UNIQUE).gameObject, PressUniqueTabButton, E_UIEVENT.DOWN);
        AddUIEvent(GetButton((int)Buttons.LEGEND).gameObject, PressLegendTabButton, E_UIEVENT.DOWN);

        m_BuyButton = GetButton((int)Buttons.BUY);
        AddUIEvent(m_BuyButton.gameObject, PressBuyBtn, E_UIEVENT.DOWN);

        m_BuyBtnText = Common.FindChild<TMPro.TextMeshProUGUI>(m_BuyButton.gameObject);

        PressNormalTabButton(null);
    }

    #region TapButtonAction
    void PressNormalTabButton(PointerEventData data)
    {
       
    }
    void PressUniqueTabButton(PointerEventData data)
    {
        
    }
    void PressLegendTabButton(PointerEventData data)
    {
        
    }
    #endregion

    void PressBuyBtn(PointerEventData data)
    {
        
    }


    void SelectCostume(ShopButton shopBtn)
    {
       
    }


    void PressOKButton(PointerEventData data)
    {
        UIManager.Instance.ShowUIScene(E_SCENE_UI_TYPE.TITLE);
    }

}
