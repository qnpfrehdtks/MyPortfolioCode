using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Popup : UI_Base
{
    enum TEXTS
    {
        NOTICE,
        STRING
    }

    enum Buttons
    {
        OK,
        CANCLE,
        EXIT
    }

    private void Awake()
    {
        BindUI<TMPro.TextMeshProUGUI>(typeof(TEXTS));
        BindUI<Button>(typeof(Buttons));
    }

    public virtual void InitPopUp(string noticeName  ,string str, System.Action<PointerEventData> okAction, System.Action<PointerEventData> cancleAction)
    {
        GetText((int)TEXTS.STRING).text = str;
        GetText((int)TEXTS.NOTICE).text = noticeName;

        AddUIEvent(GetButton((int)Buttons.OK).gameObject, okAction, E_UIEVENT.DOWN);
        AddUIEvent(GetButton((int)Buttons.CANCLE).gameObject, cancleAction, E_UIEVENT.DOWN);
       // AddUIEvent(GetButton((int)Buttons.EXIT).gameObject, cancleAction, E_UIEVENT.DOWN);
    }

}
