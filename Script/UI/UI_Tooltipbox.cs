using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UI_Tooltipbox : MonoBehaviour, IPoolingObject
{
     TMPro.TextMeshProUGUI TooltipTitle;
     TMPro.TextMeshProUGUI Tooltip;
     Image TooltipBox;
    public bool IsInPool { get; set; } = false;
    public int StartCount { get; set; } = 3;
    public void Initialize(GameObject factory)
    {
    }
    public void OnPushToQueue()
    {

    }

    public void OnPopFromQueue()
    {
    }

    private void Awake()
    {
        TooltipTitle = transform.Find("TitleText").GetComponent<TMPro.TextMeshProUGUI>();
        Tooltip = transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        TooltipBox = transform.Find("BG").GetComponent<Image>();
    }


     public void ShowTooptipBox(string title, string str)
    {
        TooltipTitle.text = title;
        Tooltip.text = str;
    }
}
