using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    [SerializeField]
    List<CustomToggle> colorblindToggles;
    [SerializeField]
    List<CustomToggle> vibrationToggles;

    void Awake()
    {
        foreach (CustomToggle colorblindToggle in colorblindToggles)
        {
            colorblindToggle.SetToogleEvent(OnColorblindClick);
        }
        foreach (CustomToggle vibrationToggle in vibrationToggles)
        {
            vibrationToggle.SetToogleEvent(OnVibrationClick);
        }

        OnColorblindClick(SaveData.CurrentColorList == 1);
        OnVibrationClick(SaveData.VibrationEnabled == 1);
    }

    public void OnColorblindClick(bool value)
    {
        if (SaveData.CurrentColorList == 1 != value) {
            SaveData.CurrentColorList = value ? 1 : 0;
            TileColorManager.Instance.SetColorList(SaveData.CurrentColorList);
        }

        foreach (CustomToggle colorblindToggle in colorblindToggles)
        {
            colorblindToggle.SetDisplay(value); 
        }
    }

    public void OnVibrationClick(bool value)
    {
        if (SaveData.VibrationEnabled == 1 != value) {
            SaveData.VibrationEnabled = value ? 1 : 0;
            if (value)
                Handheld.Vibrate();
        }
        foreach (CustomToggle vibrationToggle in vibrationToggles)
        {
            vibrationToggle.SetDisplay(value); 
        }
    }
}
