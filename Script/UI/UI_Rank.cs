using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Rank : UI_Base
{
    enum Texts
    {
        PlayerName,
        Rank,
        Gold
    }

    enum Images
    {
        First,
        BG
    }


    protected void Awake()
    {
        BindUI<TMPro.TextMeshProUGUI>(typeof(Texts));
        BindUI<Image>(typeof(Images));
    }
}
