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

    private void Start()
    {
        foreach (Transform child in References.ButtonsGroup.transform)
        {
            powerUps.Add(child.gameObject.GetComponentInChildren<PowerUp>());
        }

        InitLockPowerUp();
    }
    
    public void InitLockPowerUp()
    {
        bool isEnabled = false;
        for (int index = 0; index < powerUps.Count; index++)
        {
            if (RemoteConfig.powerUpsEnabled.TryGetValue("POWER_UP_" + GetIntToString(index+1) + "_ENABLED", out isEnabled))
            {
                powerUps[index].Lock(isEnabled);
            }
        }
    }

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
}
