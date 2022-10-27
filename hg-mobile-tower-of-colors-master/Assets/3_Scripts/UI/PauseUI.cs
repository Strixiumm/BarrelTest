using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameObject pausePopUp;
    [SerializeField] GameObject pauseToggle;

    public void Start()
    {
        // because the pause button is under the config button, we don't need to do a cleaner "show" in the animtion
        // where the option panel is hide but it will be better
        if (!RemoteConfig.BOOL_PAUSE_BUTTON_ENABLED)
        {
            pauseToggle.SetActive(false);
        }
    }

    public void OnPauseClick()
    {
        pausePopUp.SetActive(!pausePopUp.activeSelf);
        GameManager.Instance.PauseGame(pausePopUp.activeSelf);
    }
    
}
