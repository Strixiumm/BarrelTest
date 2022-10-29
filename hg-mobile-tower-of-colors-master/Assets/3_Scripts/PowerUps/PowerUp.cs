using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    public string index;
    [SerializeField] private TextMeshProUGUI usesLeft;
    private bool isLock;
    private Button button;

    public void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    private void Start()
    {
        usesLeft.text = SaveData.PowerUpLeftUses(index).ToString();
    }

    public void Lock(bool isEnabled)
    {
        if (button == null) // because we can lock without awake calling
        {
            button = gameObject.GetComponent<Button>();
        }
        isLock = !isEnabled;
        button.interactable = !isLock;
    }

    public virtual void Launch()
    {
        GameManager.Instance.PauseTimerCounter(true);
        SaveData.SetPowerUpLeftUsesDown(index);
    }

    private void Finish()
    {
        GameManager.Instance.PauseTimerCounter(false);
    }
    
}
