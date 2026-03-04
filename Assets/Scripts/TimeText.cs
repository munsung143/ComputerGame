using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    [SerializeField] TMP_Text hourTmp;
    [SerializeField] TMP_Text colonTmp;
    [SerializeField] TMP_Text minuteTmp;

    [SerializeField] float colonDelay;

    private WaitForSeconds colonDelayWfs;

    void Awake()
    {
        colonDelayWfs = new WaitForSeconds(colonDelay);
        StartCoroutine(TimeGetRoutine());
    }

    IEnumerator TimeGetRoutine()
    {
        bool isColon = false;
        while (true)
        {
            yield return colonDelayWfs;
            DateTime now = DateTime.Now;
            hourTmp.text = now.ToString("HH");
            minuteTmp.text = now.ToString("mm");
            colonTmp.alpha = isColon ? 0f : 1f;
            isColon = !isColon;

        }
    }
}
