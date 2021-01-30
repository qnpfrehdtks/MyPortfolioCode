using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_InGame : Ui_Scene
{
    enum Controller
    {
        Move
        
    }

    enum Progress
    {
        UserHPBar,
        UserMPBar,
        UserEXPBar
    }

    enum Buttons
    {
        ITEM,
        GO_TO_HOME,
    }

    enum Text
    {
        LV_COUNT = 0,
    }

    enum Stage_Info
    {
        PlayStageTitle
    }

    enum JoyStick
    {
        JoyStick
    }

    enum SkillIcon
    {
        SkillButton1,
        SkillButton2,
        SkillButton3,
        SkillButton4,
        SkillButton5,
        END
    }

    UI_StageStartTitle m_Title;

    void Awake()
    {
        BindUI<UI_ProgressBar>(typeof(Progress));
        BindUI<TMPro.TextMeshProUGUI>(typeof(Text));

        BindUI<Button>(typeof(Buttons));
        BindUI<UI_StageStartTitle>(typeof(Stage_Info));
        BindUI<InputJoyStick>(typeof(JoyStick));

        BindUI<UI_SkillSlot>(typeof(SkillIcon));

        AddUIEvent(GetButton((int)Buttons.GO_TO_HOME).gameObject, ClickGotoHome, E_UIEVENT.CLICK);

        m_Title = GetUI<UI_StageStartTitle>((int)(Stage_Info.PlayStageTitle));
        m_Title.gameObject.SetActive(false);
    }

    void ClickGotoHome(PointerEventData data)
    {
        Time.timeScale = 0.0f;

        UIManager.Instance.ShowPopUp("Alert", "Shall we go back to the menu?", "You will lose progress at the current level.",
            (PointerEventData d) =>
            {
                Time.timeScale = 1.0f;
                UIManager.Instance.ClosePopUp();
              //  GameMaster.Instance.ChangeToTitle();
            },
            (PointerEventData d) =>
            {
                Time.timeScale = 1.0f;
                UIManager.Instance.ClosePopUp();
            });
    }

    public void OnUpdateHPBar(int current, int max)
    {

        GetUI<UI_ProgressBar>((int)(Progress.UserHPBar)).SetProgressBar(current, max);
    }

    public void OnUpdateLv(int current)
    {
        GetUI<TMPro.TextMeshProUGUI>((int)(Text.LV_COUNT)).text = current.ToString();
    }

    public void OnUpdateMPBar(int current, int max)
    {
        GetUI<UI_ProgressBar>((int)(Progress.UserMPBar)).SetProgressBar(current, max);
    }

    public void OnUpdateEXPBar(int ratio, int maxRatio)
    {
        GetUI<UI_ProgressBar>((int)(Progress.UserEXPBar)).SetProgressBar(ratio, maxRatio);
    }


    /// <summary>
    /// UI를 켤때 실행되는 함수.
    /// </summary>
    public override void OnShowUp()
    {
        base.OnShowUp();
        ShowStageInfo("ParkGo", 2.0f);
    }

    public void ShowStageInfo(string str, float time)
    {
        m_Title.StartStage(str, time);
    }

    public void AllInitSkillData(Dictionary<int, Skill> dic)
    {
        for(SkillIcon i = SkillIcon.SkillButton1; i < SkillIcon.END; i++)
        {
            InitSkillIconUI((int)i, null);
        }

        foreach (var s in dic)
        {
            InitSkillIconUI(s.Key, s.Value);
        }
    }

    public void InitSkillIconUI(int idx, Skill skill)
    {
        UI_SkillSlot icon = GetUI<UI_SkillSlot>(idx);

        if (icon == null) return;
        icon.InitSlot(skill);
    }

    public void InitCharacterHUD(int LV, int HP, int MP, int EXP ,Stat stat , Transform tr)
    {
        GetUI<UI_ProgressBar>((int)(Progress.UserEXPBar)).Init(EXP, stat.MaxEXP, TextBarStyle.PERCENT);
        GetUI<UI_ProgressBar>((int)(Progress.UserMPBar)).Init(MP, stat.MaxHP, TextBarStyle.DATA);
        GetUI<UI_ProgressBar>((int)(Progress.UserHPBar)).Init(HP, stat.MaxMP, TextBarStyle.DATA);

        GetUI<TMPro.TextMeshProUGUI>((int)(Text.LV_COUNT)).text = LV.ToString();
        GetUI<InputJoyStick>((int)JoyStick.JoyStick).Init(tr);
    }
}
