using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    [SerializeField] TMP_Text minuteTmp;
    [SerializeField] TMP_Text colonTmp;
    [SerializeField] TMP_Text secondTmp;

    [SerializeField] float colonDelay;

    private WaitForSeconds colonDelayWfs;

    private float second;
    private int seconds;
    private int minutes;
    bool isColon = true;
    bool timerOn;
    public bool TimerOn{ set{timerOn = value;}}

    void Awake()
    {
        colonDelayWfs = new WaitForSeconds(colonDelay);
        minuteTmp.text = "00";
        secondTmp.text = "00";
    }
    void Update()
    {
        if (!timerOn) return;
        second += Time.deltaTime;
        if (second >= 1)
        {
            second -= 1;
            seconds++;
            isColon = !isColon;
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
                RenderTime();
            }
            RenderTime();
        }

    }

    void RenderTime()
    {
        minuteTmp.text = minutes.ToString("D2");
        secondTmp.text = seconds.ToString("D2");
        colonTmp.alpha = isColon ? 0f : 1f;
    }
}
