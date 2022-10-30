using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    // if you really want them to be scalable :`
    // the const POWER_UP_XXX_ENALBE have to be implement in PowerUp with "POWER_UP_ENALBE"
    
    public EditorReferences References;

    [Serializable]
    public class EditorReferences
    {
        public VerticalLayoutGroup ButtonsGroup;
    }
    
    private List<PowerUp> powerUps = new List<PowerUp>();

    private void Awake()
    {
        foreach (Transform child in References.ButtonsGroup.transform)
        {
            powerUps.Add(child.gameObject.GetComponentInChildren<PowerUp>());
        }
        InitPlayerPref();
    }

    private void Start()
    {
        InitLockPowerUp();
    }
    
    public void InitLockPowerUp()
    {
        bool isEnabled = false;
        foreach (PowerUp powerUp in powerUps)
        {
            if (RemoteConfig.powerUpsEnabled.TryGetValue("POWER_UP_" + powerUp.index + "_ENABLED", out isEnabled))
            {
                powerUp.Lock(!isEnabled);
            }
        }
    }

    // at begining I made powerup number based on index, but to be more efficient
    // and call function from the powerup itself, i put them in powerup element 
    private string GetIntToString(int intToConvert)
    {
        switch (intToConvert)
        {
            case 1: return "ONE";
            case 2: return "TWO";
            case 3: return "THREE";
            default: return "";
        }
    }

    private void InitPlayerPref()
    {
        int leftUses = 0;
        foreach (PowerUp powerUp in powerUps)
        {
            if (RemoteConfig.powerUpsUses.TryGetValue("POWER_UP_" + powerUp.index + "_USES", out leftUses))
            {
                SaveData.InitPowerUpLeftUses(powerUp.index, leftUses);
            }
        }
    }
}
