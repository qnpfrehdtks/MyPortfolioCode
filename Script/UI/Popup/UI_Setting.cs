using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Setting : UI_Popup
{
    enum Buttons
    {
        SOUND_ON,
        VIB_ON,
        EXIT
    }

    private void Awake()
    {
        BindUI<Button>(typeof(Buttons));
        AddUIEvent(GetButton((int)Buttons.SOUND_ON).gameObject, ClickOnOffSound, E_UIEVENT.CLICK);
        AddUIEvent(GetButton((int)Buttons.VIB_ON).gameObject, ClickOnOffVibe, E_UIEVENT.CLICK);
    }

    public override void InitPopUp(string noticeName, string str, System.Action<PointerEventData> okAction, System.Action<PointerEventData> cancleAction)
    {
        // 진동
        bool vibe = PlayerPrefsManager.Instance.GetKeyBool(Buttons.VIB_ON.ToString(), Settings.OnVibe);
        if (vibe) GetButton((int)Buttons.VIB_ON).image.sprite = ResourceManager.Instance.GetSprite("OnCheck");
        else GetButton((int)Buttons.VIB_ON).image.sprite = ResourceManager.Instance.GetSprite("OffCheck");

        //사운드
        bool sound = PlayerPrefsManager.Instance.GetKeyBool(Buttons.SOUND_ON.ToString(), Settings.OnSound);
        if(sound) GetButton((int)Buttons.SOUND_ON).image.sprite = ResourceManager.Instance.GetSprite("OnCheck");
        else GetButton((int)Buttons.SOUND_ON).image.sprite = ResourceManager.Instance.GetSprite("OffCheck");

        //종료 버튼
        AddUIEvent(GetButton((int)Buttons.EXIT).gameObject, cancleAction, E_UIEVENT.DOWN);
    }

    void ClickOnOffVibe(PointerEventData data)
    {
        Settings.OnVibe = !Settings.OnVibe;
        PlayerPrefsManager.Instance.SaveKey_bool(Buttons.VIB_ON.ToString(), Settings.OnVibe);

        if (Settings.OnVibe) GetButton((int)Buttons.VIB_ON).image.sprite = ResourceManager.Instance.GetSprite("OnCheck");
        else GetButton((int)Buttons.VIB_ON).image.sprite = ResourceManager.Instance.GetSprite("OffCheck");
    }

    void ClickOnOffSound(PointerEventData data)
    {
        Settings.OnSound = !Settings.OnSound;
        PlayerPrefsManager.Instance.SaveKey_bool(Buttons.SOUND_ON.ToString(), Settings.OnSound);

        if (Settings.OnSound) GetButton((int)Buttons.SOUND_ON).image.sprite = ResourceManager.Instance.GetSprite("OnCheck");
        else GetButton((int)Buttons.SOUND_ON).image.sprite = ResourceManager.Instance.GetSprite("OffCheck");

        if (!Settings.OnSound)
        {
            SoundManager.Instance.SetSoundVolume(0.0f);
        }
        else 
        {
            SoundManager.Instance.SetSoundVolume(0.7f);
        }
    }


}
