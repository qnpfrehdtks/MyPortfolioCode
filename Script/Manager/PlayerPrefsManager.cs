using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : Singleton<PlayerPrefsManager>
{
    public override void InitializeManager()
    {
       
    }

    public string GetKeyString(string key, string defaultValue = "")
    {
        if (!PlayerPrefs.HasKey(key))
        {
            SaveKey_string(key, defaultValue);
            return defaultValue;
        }
        else
        {
            return PlayerPrefs.GetString(key);
        }
    }

    public bool GetKeyBool(string key, bool defaultValue = false)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            SaveKey_bool(key, defaultValue);
            return defaultValue;
        }
        else
        {
            return PlayerPrefs.GetInt(key) == 1;
        }
    }

    public int GetKeyInt(string key, int defaultValue = 0)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            SaveKey_int(key, defaultValue);
            return defaultValue;
        }
        else
        {
            return PlayerPrefs.GetInt(key);
        }
    }

    public float GetKeyFloat(string key, float defaultValue = 0.0f)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            SaveKey_float(key, 0.0f);
            return 0.0f;
        }
        else
        {
            return PlayerPrefs.GetFloat(key);
        }
    }

    public void SaveKey_bool(string Key, bool value)
    {
        PlayerPrefs.SetInt(Key, value == true ? 1 : 0);
    }

    public void SaveKey_float(string Key, float value)
    {
        PlayerPrefs.SetFloat(Key, value);
    }
    public void SaveKey_int(string Key, int value)
    {
        PlayerPrefs.SetInt(Key, value);
    }
    public void SaveKey_string(string Key, string value)
    {
        PlayerPrefs.SetString(Key, value);
    }
}
