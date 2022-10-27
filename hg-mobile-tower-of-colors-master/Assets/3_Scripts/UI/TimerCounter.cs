using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCounter : MonoBehaviour
{
    [SerializeField]
    Gradient gradient;
    [SerializeField]
    Image percentFill;
    [SerializeField]
    Image border;
    [SerializeField]
    Slider slider;

    private float maxTime = RemoteConfig.FLOAT_LEVEL_TIMER_SECONDS;
    private IEnumerator timer;
    
    public void InitTimer(Color lvlColor)
    {
        SetBorderColor(lvlColor);
        slider.normalizedValue = 1f;
        SetFillColor();
    }
    
    public void StartTimer()
    {
        timer = ProgressTimer();
        StartCoroutine(timer);
    }
    
    private IEnumerator ProgressTimer()
    {
        float duration = maxTime;
        float normalizedTime = 1f;
        while(normalizedTime > 0f)
        {
            slider.normalizedValue = normalizedTime;
            normalizedTime -= Time.deltaTime / duration;
            SetFillColor();
            yield return null;
        }
        TimerComplete();
    }

    public void StopTimer()
    {
        StopCoroutine(timer);
    }
    
    private void TimerComplete()
    {
        GameManager.Instance.LoseGame();
    }
    
    private void SetBorderColor(Color color)
    {
        border.color = color;
    }
    
    private void SetFillColor()
    {
        percentFill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
