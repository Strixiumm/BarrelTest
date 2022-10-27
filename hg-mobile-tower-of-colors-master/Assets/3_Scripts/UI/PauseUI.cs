using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameObject pausePopUp;

    public void OnPauseClick()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        GameManager.Instance.PauseGame(!gameObject.activeSelf);
    }
    
}
