using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Victory : Ui_Scene
{
    //enum Buttons
    //{
    //    RETRY
    //}

    //enum Rank_Texts
    //{
    //    PlayerName,
    //    Rank,
    //    Gold
    //}

    //enum Images
    //{
    //    First,
    //    BG
    //}

    //List<UI_Base> m_ListRankBar = new List<UI_Base>();

    //public override void Initialize(GameObject factory)
    //{
    //    base.Initialize(factory);

    //    //  BindUI<TMPro.TMP_InputField>(typeof(InputFields));
    //    BindUI<Button>(typeof(Buttons));
    //    BindUI<GridLayoutGroup>(typeof(GridLayOut));
    //    BindUI<UI_Base>(typeof(RankObject_UI));

    //    string[] str = System.Enum.GetNames(typeof(RankObject_UI));
    //    for (int i = 0; i < str.Length; i++)
    //    {
    //        m_ListRankBar.Add(GetUI<UI_Base>(i));
    //    }

    //    AddUIEvent(GetButton((int)Buttons.RETRY).gameObject, DownPlayBtn, E_UIEVENT.DOWN);

    //    CharacterManager.Instance.OnFinalCharacterState = null;
    //    CharacterManager.Instance.OnFinalCharacterState = InitResultUI;
    //}

    //void InitRankBar(UI_Base ui, CharacterBase character, int idx, int gold)
    //{
    //    ui.gameObject.SetActive(true);
    //    ui.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

    //    string rankStr = "";

    //    if (character.m_isMyCharacter)
    //    {
    //        ui.GetText((int)Rank_Texts.PlayerName).color = new Color(0.0f, 0.9f, 0.0f);
    //        ui.GetImage((int)Images.BG).color = new Color(0.0f, 0.9f, 0.0f);

    //        Animator ani = ui.GetComponent<Animator>();

    //        if (ani && ani.enabled && gameObject.activeSelf)
    //            ui.GetComponent<Animator>().Play("PlayerWin");
    //    }
    //    else
    //    {
    //        ui.GetText((int)Rank_Texts.PlayerName).color = new Color(1.0f, 1.0f, 1.0f);
    //        ui.GetImage((int)Images.BG).color = new Color(1.0f, 1.0f, 1.0f);

    //        Animator ani = ui.GetComponent<Animator>();

    //        if (ani && ani.enabled && gameObject.activeSelf)
    //            ui.GetComponent<Animator>().Play("Idle");
    //    }

    //    if (character.IsDead)
    //    {
    //        rankStr = "Dead";
    //        ui.GetText((int)Rank_Texts.Rank).color = Color.white;
    //        ui.GetImage((int)Images.First).gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        switch (idx)
    //        {
    //            case 0:
    //                rankStr = "1st";
    //                ui.GetText((int)Rank_Texts.Rank).color = new Color(1.0f, 0.9f, 0);
    //                ui.GetImage((int)Images.First).gameObject.SetActive(true);
    //                break;
    //            case 1:
    //                rankStr = "2nd";
    //                ui.GetText((int)Rank_Texts.Rank).color = new Color(0.78f, 0.78f, 0.78f);
    //                ui.GetImage((int)Images.First).gameObject.SetActive(false);
    //                break;
    //            case 2:
    //                rankStr = "3rd";
    //                ui.GetText((int)Rank_Texts.Rank).color = new Color(0.8f, 0.5f, 0.19f);
    //                ui.GetImage((int)Images.First).gameObject.SetActive(false);
    //                break;
    //            default:
    //                rankStr = (idx + 1) + "th";
    //                ui.GetText((int)Rank_Texts.Rank).color = Color.white;
    //                ui.GetImage((int)Images.First).gameObject.SetActive(false);
    //                break;
    //        }
    //    }

    //    ui.GetText((int)Rank_Texts.Rank).text = rankStr;
    //    ui.GetText((int)Rank_Texts.Gold).text = gold.ToString();
    //}

    //public void InitResultUI()
    //{
    //    List<CharacterBase> listResultCharacter = new List<CharacterBase>();


    //    for (int i = 0; i < CharacterManager.Instance.m_ListDeadCharacter.Count; i++)
    //    {
    //        listResultCharacter.Add(CharacterManager.Instance.m_ListDeadCharacter[i]);
    //    }

    //    for (int i = 0; i < m_ListRankBar.Count; i++)
    //    {
    //        if (i < listResultCharacter.Count)
    //        {
    //            if(!listResultCharacter[i].IsDead)
    //                InitRankBar(m_ListRankBar[i], listResultCharacter[i], i, 1000);
    //            else if (listResultCharacter[i].IsDead)
    //                InitRankBar(m_ListRankBar[i], listResultCharacter[i], i, 0);
    //        }
    //        else
    //        {
    //            m_ListRankBar[i].gameObject.SetActive(false);
    //        }
    //    }
    //}

    //public override void OnShowUp()
    //{
    //    base.OnShowUp();
    //    InitResultUI();

    //    SoundManager.Instance.StopBGM();
    //    SoundManager.Instance.PlaySFX(E_SFX.VICTORY);

    //}

    void DownPlayBtn(PointerEventData data)
    {
      //  GameMaster.Instance.ChangeToTitle();
    }
}
