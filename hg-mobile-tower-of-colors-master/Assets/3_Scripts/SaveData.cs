using System;
using UnityEngine;
using System.Collections;

public static class SaveData
{
    public static int CurrentLevel
    {
        get { return PlayerPrefs.GetInt("CurrentLevel", 1); }
        set { PlayerPrefs.SetInt("CurrentLevel", value); }
    }

    public static float PreviousHighscore
    {
        get { return PlayerPrefs.GetFloat("PreviousHighscore", 0); }
        set { PlayerPrefs.SetFloat("PreviousHighscore", value); }
    }

    public static int CurrentColorList
    {
        get { return PlayerPrefs.GetInt("CurrentColorList", 0); }
        set { PlayerPrefs.SetInt("CurrentColorList", value); }
    }

    public static int VibrationEnabled
    {
        get { return PlayerPrefs.GetInt("VibrationEnabled", 1); }
        set { PlayerPrefs.SetInt("VibrationEnabled", value); }
    }

    public static void InitPowerUpLeftUses(string powerUseIndex, int powerUpLeftUses)
    {
        if (!PlayerPrefs.HasKey("PowerUpLeftUses" + powerUseIndex))
        {
            PlayerPrefs.SetInt("PowerUpLeftUses" + powerUseIndex, powerUpLeftUses);
        }
    }

    public static int PowerUpLeftUses(string powerUseIndex)
    {
        return PlayerPrefs.GetInt("PowerUpLeftUses" + powerUseIndex, 0);
    }

    public static void SetPowerUpLeftUsesDown(string powerUseIndex)
    {
        int currentPowerUpLeftUses = PowerUpLeftUses(powerUseIndex);
        PlayerPrefs.SetInt("PowerUpLeftUses" + powerUseIndex, currentPowerUpLeftUses-1);
    }
}
