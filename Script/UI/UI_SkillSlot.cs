using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_SkillSlot : UI_BaseSlot<Skill>
{
    enum Text
    {
        CoolTime
    }

    enum SkillImage
    {
        CoolTimeImage = UIImage.SlotImage + 1
    }

    TMPro.TextMeshProUGUI m_CoolTimeText;
    UI_ProgressBar m_CoolTimeProgressBar;
    SkillData m_SkillInfo;

    public int m_SkillIdxIndex = 0;
    protected override void Awake()
    {
        base.Awake();
        BindUI<Image>(typeof(SkillImage));
        BindUI<TMPro.TextMeshProUGUI>(typeof(Text));

        m_CoolTimeText = GetText((int)(Text.CoolTime));
        m_CoolTimeText.text = "";
    }
    private void Update()
    {
        if (m_CoolTimeProgressBar != null && m_Object != null)
        {
            if (m_Object.currentTime >= 0.01f && m_CoolTimeText.gameObject.activeSelf)
            {
                m_CoolTimeText.text = string.Format("{0:0.0} ", double.Parse(m_Object.currentTime.ToString()));
            }
            else if (m_Object.currentTime < 0.01f)
            {
                m_CoolTimeText.gameObject.SetActive(false);
            }

            m_CoolTimeProgressBar.SetProgressBar(m_Object.currentTime, m_Object.m_skillInfo.CoolTime);
        }
    }
    public override void InitSlot(Skill obj)
    {
        m_Object = obj;
        m_CoolTimeProgressBar = Common.GetOrAddComponent<UI_ProgressBar>(gameObject);

        if (m_Object != null)
        {
            m_SkillInfo = m_Object.m_skillInfo;
            m_IconImage.sprite = m_SkillInfo.IconPath;
            m_IconImage.gameObject.SetActive(true);

            if(m_Object.currentTime <= 0.001f)
            {
                m_CoolTimeText.gameObject.SetActive(false);
            }
            else
            {
                m_CoolTimeText.gameObject.SetActive(true);
            }

            m_CoolTimeText.text = m_Object.currentTime.ToString();
            m_CoolTimeProgressBar.Init(m_Object.currentTime, m_Object.m_skillInfo.CoolTime, TextBarStyle.NONE);
        }
        else
        {
            m_IconImage.sprite = null;
            m_IconImage.gameObject.SetActive(false);
            m_CoolTimeText.gameObject.SetActive(false);
            m_CoolTimeProgressBar.Init(0, 1, TextBarStyle.NONE);
        }

        m_CoolTimeProgressBar.SetSprite(m_IconImage.sprite);

    }
    public string GetElementalName(E_ELEMENTAL_TYPE type)
    {
        switch(type)
        {
            case E_ELEMENTAL_TYPE.DARK:
                return "<color=#7A33FF>Dark</color>";
            case E_ELEMENTAL_TYPE.LIGHT:
                return "<color=#FFEC33>Light</color>";
            case E_ELEMENTAL_TYPE.NATURE:
                return "<color=#64FF33>Nature</color>";
            case E_ELEMENTAL_TYPE.WATER:
                return "<color=#33ECFF>Water</color>";
            case E_ELEMENTAL_TYPE.FIRE:
                return "<color=#FFAF33>Fire</color>";
            case E_ELEMENTAL_TYPE.LIGHTING:
                return "<color=#C433FF>Lighting</color>";
        }
        return "";
    }
    protected override void UpBtn(PointerEventData data)
    {
        return;
    }
    protected override void ExitDragBtn(PointerEventData data)
    {
        if (m_Object == null) return;
        UIManager.Instance.OffToolTipBox();
    }
    protected override void DragBtn(PointerEventData data)
    {
        if (m_Object == null) return;

        string str = $"Elemental : {GetElementalName(m_SkillInfo.AttackLogic[0].m_ElementalType)}";
        str += $"\nDamage : Character Attack * {m_SkillInfo.AttackLogic[0].ratio}";
        
        UIManager.Instance.ShowToolTipBox(data.position, str, m_SkillInfo.myName);
    }
    protected override void DownBtn(PointerEventData data)
    {
        if (m_Object == null) return;

        if (m_Object.UseSkill())
        {
            m_CoolTimeText.gameObject.SetActive(true);
        }
    }

    protected override void BeginDragBtn(PointerEventData data)
    {
        
    }
}
