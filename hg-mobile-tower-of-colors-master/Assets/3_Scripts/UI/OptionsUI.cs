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
            colorblindToggle.SetEnabled(SaveData.CurrentColorList == 1);
            colorblindToggle.SetToogleEvent(OnColorblindClick);
        }
        foreach (CustomToggle vibrationToggle in vibrationToggles)
        {
            vibrationToggle.SetEnabled(SaveData.VibrationEnabled == 1);
            vibrationToggle.SetToogleEvent(OnVibrationClick);
        }
    }

    public void OnColorblindClick(bool value)
    {
        if (SaveData.CurrentColorList == 1 != value) {
            SaveData.CurrentColorList = value ? 1 : 0;
            TileColorManager.Instance.SetColorList(SaveData.CurrentColorList);
        }
    }

    public void OnVibrationClick(bool value)
    {
        if (SaveData.VibrationEnabled == 1 != value) {
            SaveData.VibrationEnabled = value ? 1 : 0;
            if (value)
                Handheld.Vibrate();
        }
    }
}
