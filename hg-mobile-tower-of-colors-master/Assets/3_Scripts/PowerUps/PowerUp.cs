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
    [SerializeField] private float cooldown;
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

    public void Lock(bool lockPowerUp)
    {
        if (!lockPowerUp)
        {
            if (!CanUnLock())
            {
                lockPowerUp = true;
            }
        }
        isLock = lockPowerUp;
        if (button == null) // because we can lock without awake calling
        {
            button = gameObject.GetComponent<Button>();
        }
        button.interactable = !isLock;
    }

    private bool CanUnLock()
    {
        return SaveData.PowerUpLeftUses(index) != 0;
    }


    public virtual void Launch()
    {
        Lock(true); // instead we could do a "using lock" with a different ui
        GameManager.Instance.PauseTimerCounter(true);
        SaveData.SetPowerUpLeftUsesDown(index);
        int usesLeftcount = SaveData.PowerUpLeftUses(index);
        usesLeft.text = usesLeftcount.ToString();
        if (usesLeftcount == 0)
        {
            Lock(true);
        }
    }

    protected void Finish()
    {
        GameManager.Instance.PauseTimerCounter(false);
        Lock(false);
    }
    
}
