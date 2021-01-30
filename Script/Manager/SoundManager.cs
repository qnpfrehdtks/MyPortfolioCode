﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    Dictionary<E_SFX, AudioClip> m_dicSound = new Dictionary<E_SFX, AudioClip>();
    Dictionary<E_BGM, AudioClip> m_dicBGM = new Dictionary<E_BGM, AudioClip>();

    AudioSource m_AudioSource;

    E_BGM m_currentPlayBGM = E_BGM.NONE;


    public override void InitializeManager()
    {
        m_AudioSource = Common.GetOrAddComponent<AudioSource>(gameObject);

        AudioClip[] bgms = ResourceManager.Instance.LoadAll<AudioClip>("Sounds/BGM");

        for(int i = 0; i < bgms.Length; i++)
        {
            string soundName = bgms[i].name;
            E_BGM bgm = (E_BGM)Enum.Parse(typeof(E_BGM), soundName);
#if UNITY_EDITOR
            Debug.Log(bgms[i].name + "Load Success");
#endif
            m_dicBGM.Add(bgm, bgms[i]);
        }

        AudioClip[] sounds = ResourceManager.Instance.LoadAll<AudioClip>("Sounds/SFX");

        for (int i = 0; i < sounds.Length; i++)
        {
            string soundName = sounds[i].name;
            E_SFX sfx = (E_SFX)Enum.Parse(typeof(E_SFX), soundName);
#if UNITY_EDITOR
            Debug.Log(sounds[i].name + "Load Success");
#endif
            m_dicSound.Add(sfx, sounds[i]);
        }

        Settings.OnSound = PlayerPrefsManager.Instance.GetKeyBool("SOUND_ON", Settings.OnSound);
        if (Settings.OnSound) SetSoundVolume(0.7f);
        else SetSoundVolume(0.0f);

        return;
    }

    public void SetSoundVolume(float volume)
    {
        m_AudioSource.volume = volume;
    }

    public void StopBGM()
    {
        m_AudioSource.Stop();
        m_currentPlayBGM = E_BGM.NONE;
    }

    public void PlayBGM(E_BGM _bgm)
    {
        if (Settings.OnSound) SetSoundVolume(0.7f);
        else SetSoundVolume(0.0f);

        if (m_currentPlayBGM == _bgm)
            return;

        AudioClip clip;
        if (m_dicBGM.TryGetValue(_bgm, out clip))
        {
            m_AudioSource.loop = true;
            m_AudioSource.clip = clip;
            m_AudioSource.Play();
            m_currentPlayBGM = _bgm;
        }
        else
        {
            StopBGM();
        }
    }

    public void PlaySFX(E_SFX _sfx)
    {
        if (Settings.OnSound) SetSoundVolume(0.7f);
        else SetSoundVolume(0.0f);

        AudioClip clip;
        if(m_dicSound.TryGetValue(_sfx, out clip))
        {
            m_AudioSource.PlayOneShot(clip);
        }
    }
}
