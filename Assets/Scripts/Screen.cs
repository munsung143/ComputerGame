using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;

public class Screen : MonoBehaviour
{
    [SerializeField] TMP_Text screenText;
    [SerializeField] GameObject screenOffPanel;

    [SerializeField] float textDelay;
    public UnityEvent onScreenOn;

    [SerializeField] string testText;

    private bool isOn;

    private WaitForSeconds delay;

    private int compositeKoreanStartAt = 0xAC00;

    private int[] koreanFirstTable = { 1, 2, 4, 7, 8,9,17,18,19,21,22,23,24,25,26,27,28,29,30 }; 

    void Awake()
    {
        onScreenOn.AddListener(StartTextRoutine);
        onScreenOn.AddListener(onScreenOn.RemoveAllListeners);

        delay = new WaitForSeconds(textDelay);
    }

    void StartTextRoutine()
    {
        StartCoroutine(TextCoroutine(testText));
    }

    public void Toggle()
    {
        if (isOn) Off();
        else On();
    }
    public void On()
    {
        screenOffPanel.SetActive(false);
        isOn = true;
        onScreenOn.Invoke();
    }
    public void Off()
    {
        screenOffPanel.SetActive(true);
        isOn = false;
    }

    IEnumerator TextCoroutine(string input)
    {
        // char는 해당 문자의 유니코드 값만을 저장한다.
        // UTF-8 인코딩 방식으로 저장하지 않는다.
        // 한글 문자 형성 공식 {(초성×28x21)+(중성×28)+종성}+44032 
        // (종성 0~27, 중성 0~20)
        string resultText = "";
        foreach (char c in input)
        {
            if (c >= compositeKoreanStartAt)
            {
                int composite = c - compositeKoreanStartAt;
                int last = composite % 28;
                composite /= 28;
                int middle = composite % 21 * 28; 
                composite /= 21;
                int first = composite;

                char letter = (char)(koreanFirstTable[first] + 0x3130);
                resultText = $"{resultText}{letter}";
                screenText.text = resultText;
                yield return delay;

                resultText = resultText.Remove(resultText.Length - 1);
                first = first * 21 * 28;
                letter = (char)(first + middle + compositeKoreanStartAt);
                resultText = $"{resultText}{letter}";
                screenText.text = resultText;
                yield return delay;

                if (last != 0)
                {
                    resultText = resultText.Remove(resultText.Length - 1);
                    letter = (char)(first + middle + last + compositeKoreanStartAt);
                    resultText = $"{resultText}{letter}";
                    screenText.text = resultText;
                    yield return delay;
                }

            }
            else
            {
                resultText = $"{resultText}{c}";
                screenText.text = resultText;
                yield return delay;
            }
        }
    }


}
