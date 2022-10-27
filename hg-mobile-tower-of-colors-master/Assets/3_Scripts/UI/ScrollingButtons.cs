using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingButtons : MonoBehaviour
{
    Animator animator;
    bool isOpen = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = 1.0f / Time.timeScale;
        isOpen = false;
        animator.SetBool("isOpen", isOpen);
        
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }
    
}
