using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public static class SettingsManager
{
    public delegate void BoolGet(string key, bool value);
    public static event BoolGet BoolChange;

    public delegate void IntGet(string key, int value);
    public static event IntGet IntChange;

    public delegate void StringGet(string key, string value);
    public static event StringGet StringChange;

    public static bool MusicOn;


    public static void NewHatValue(int newValue)
    {
        PlayerPrefs.SetInt("hatsUnlocked", newValue); 
        IntChange("hatsUnlocked", GetHats());
    }

    public static int GetHats()
    {
        return PlayerPrefs.GetInt("hatsUnlocked", 2);
    }

    public static bool IsMusicOn()
    {
        int testInt = PlayerPrefs.GetInt("MusicOn");
        if (testInt == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsMusicOn(bool value)
    {
        int testInt = value ? 1 : 0;
        PlayerPrefs.SetInt("MusicOn", testInt);
        if (testInt == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool IsInvincible()
    {
        int testInt = PlayerPrefs.GetInt("Invincible");
        if (testInt == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsInvincible(bool value)
    {
        int testInt = value ? 1 : 0;
        PlayerPrefs.SetInt("Invincible", testInt);
        if (testInt == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool IsPluh()
    {
        int testInt = PlayerPrefs.GetInt("Pluh");
        if (testInt == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsPluh(bool value)
    {
        int testInt = value ? 1 : 0;
        PlayerPrefs.SetInt("Pluh", testInt);
        if (testInt == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsTutorial()
    {
        int testInt = PlayerPrefs.GetInt("IsTutorial", -1);
        if (testInt == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsTutorial(bool value)
    {
        int testInt = value ? 1 : 0;
        PlayerPrefs.SetInt("IsTutorial", testInt);
        if (testInt == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void CheckInitialization()
    {
        if (PlayerPrefs.GetInt("IsTutorial", -1) == -1)
        {
            PlayerPrefs.SetInt("IsTutorial", 1);
        }
        if (PlayerPrefs.GetInt("Pluh", -1) == -1)
        {
            PlayerPrefs.SetInt("Pluh", 0);
        }
        if (PlayerPrefs.GetInt("Invincible", -1) == -1)
        {
            PlayerPrefs.SetInt("Invincible", 0);
        }
        if (PlayerPrefs.GetInt("MusicOn", -1) == -1)
        {
            PlayerPrefs.SetInt("MusicOn", 1);
        }
    }
}


