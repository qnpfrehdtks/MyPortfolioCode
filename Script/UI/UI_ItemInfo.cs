using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemInfo : UI_Base
{
    enum ItemInfo
    {
        ItemName,
        ItemInfo1,
        ItemInfo2
    }

    enum ItemSlot
    {
        ItemImage
    }

    private void Awake()
    {
        BindUI<TMPro.TextMeshProUGUI>(typeof(ItemInfo));
        BindUI<UI_ItemSlot>(typeof(ItemSlot));
    }

    public void InitIteminfo(Item item)
    {
        UI_ItemSlot slot = GetUI<UI_ItemSlot>((int)ItemSlot.ItemImage);

        if (slot != null)
        {
            slot.InitSlot(item);
        }

        if (item != null)
        {
            string str = $"Attack {item.Stat.Attack}";

            GetText((int)ItemInfo.ItemName).text = $"{item.Stat.myName}";
            GetText((int)ItemInfo.ItemInfo2).text = $"Part {item.Stat.ItemCategory.ToString()}\nGrade {item.Stat.ItemGrade}";
            GetText((int)ItemInfo.ItemInfo1).text = str;
        }
        else if (item == null)
        {
            GetText((int)ItemInfo.ItemName).text = "";
            GetText((int)ItemInfo.ItemInfo2).text = "";
            GetText((int)ItemInfo.ItemInfo1).text = "";
        }
    }
}
