using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : Button
{
    Image m_SelectedIcon;
    ItemInfo m_itemInfo;
    bool m_isBuy;

    bool m_isSelected;

    Animator m_Anim;
    

    protected override void Awake()
    {
        base.Awake();

        m_Anim = GetComponent<Animator>();
        ItemInfo = null;
        IsBuy = false;
        m_isSelected = false;
        m_SelectedIcon = transform.Find("Selected").GetComponent<Image>();
    }

    Image SelectedIcon
    {
        get
        {
            if (m_SelectedIcon == null)
            {
                m_SelectedIcon = transform.Find("Selected").GetComponent<Image>();
            }

            return m_SelectedIcon;
        }
    }

    public ItemInfo ItemInfo
    {
        get
        {
            return m_itemInfo;
        }
        set
        {
            m_itemInfo = value;
            InitItemInfo();
        }
    }

    public bool IsSelected
    {
        get
        {
            return m_isSelected;
        }
        set
        {
            m_isSelected = value;
            if(m_isSelected)
            {
                OnSelectIcon();
            }
            else
            {
                OffSelectIcon();
            }
        }
    }

    public bool IsBuy
    {
        get
        {
            return m_isBuy;
        }
        set
        {
            m_isBuy = value;
            InitItemInfo();
        }
    }

    void OnSelectIcon()
    {
        image.color = Color.grey;
        SelectedIcon.gameObject.SetActive(true);
    }

    void OffSelectIcon()
    {

        image.color = Color.white;
        SelectedIcon.gameObject.SetActive(false);
    }

    public void PlayAnimation(string str)
    {
        if(m_Anim == null)
        {
            m_Anim = GetComponent<Animator>();
        }

        m_Anim.SetTrigger(str);
    }

    void InitItemInfo()
    {
        if (m_isBuy == false)
        {
            image.color = Color.white;
         //   image.sprite = ResourceManager.Instance.GetSprite(Common.RANDOM_AVATA_ICON);
            SelectedIcon.gameObject.SetActive(false);
            return;
        }

        image.color = Color.white;

        if (m_itemInfo == null)
        {
            SelectedIcon.gameObject.SetActive(false);
           // image.sprite = ResourceManager.Instance.GetSprite(Common.RANDOM_AVATA_ICON);
        }
        else
        {
            SelectedIcon.gameObject.SetActive(false);
            image.sprite = ResourceManager.Instance.GetSprite(m_itemInfo.itemIconResourcePath);
        }
    }

}
