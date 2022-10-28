using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    private bool isLock;
    private Button button;

    public void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    public void Lock(bool isEnabled)
    {
        isLock = !isEnabled;
        button.interactable = !isLock;
    }
}
