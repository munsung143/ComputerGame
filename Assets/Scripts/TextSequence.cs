using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextSequence : MonoBehaviour
{

    private static int COMPOSITE_KOREAN_START_AT = 0xAC00;
    private static int SINGLE_KOREAN_START_AT = 0x3130;
    private static int[] SINGLE_KOREAN_TABLE = { 1, 2, 4, 7, 8, 9, 17, 18, 19, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
    [SerializeField] TMP_Text tmpText;
    private UnityEvent onTextEnd;

    private float initialFontSize;
    void Awake()
    {
        onTextEnd = new UnityEvent();
        initialFontSize = tmpText.fontSize;
    }
    public void AddTextEndListner(UnityAction action)
    {
        onTextEnd.AddListener(action);
    }
    public void RemoveTextEndListener(UnityAction action)
    {
        onTextEnd.RemoveListener(action);
    }
    public void ClearText()
    {
        tmpText.text = "";
    }
    public void SetFontSize(float size)
    {
        tmpText.fontSize = size;
    }
    public void ResetFontSize()
    {
        tmpText.fontSize = initialFontSize;
    }
    public void SetColor(Color color)
    {
        tmpText.color = color;
    }

    public string GetBinaryText(string text)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            for (int shift = (int)Math.Log(text[i], 2); shift >= 0; shift--)
            {
                sb.Append((text[i] >> shift) & 1);
            }
        }
        return sb.ToString();
    }



    public string GetSpecificSubjectedText(string text, string subject, string postpositions)
    {
        StringBuilder sb = new StringBuilder();
        bool inSharp = false;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '#')
            {
                if (subject == "")
                {
                    continue;
                }
                inSharp = !inSharp;
                if (inSharp) sb.Append(subject);
                else
                {
                    i++;
                    if (i >= text.Length) break;
                    char p = text[i];
                    if (p == '은' || p == '는') sb.Append(postpositions[0]);
                    else if (p == '이' || p == '가') sb.Append(postpositions[1]);
                    else if (p == '을' || p == '를') sb.Append(postpositions[2]);
                    else if (p == '와' || p == '과') sb.Append(postpositions[3]);
                    else i--;
                }
                continue;
            }
            if (inSharp) continue;
            sb.Append(text[i]);
        }
        return sb.ToString();
    }
    public IEnumerator TextRoutine(string input, WaitForSeconds delay)
    {
        return TextRoutine(input, delay, "", null);
    }
    public IEnumerator TextRoutine(string input, WaitForSeconds delay, string initial)
    {
        return TextRoutine(input, delay, initial, null);
    }
    public IEnumerator TextRoutine(
    string input,
    WaitForSeconds delay,
    string initial,
    WaitForSeconds underbarDelay)
    {
        // char는 해당 문자의 유니코드 값만을 저장한다.
        // UTF-8 인코딩 방식으로 저장하지 않는다.
        // 한글 문자 형성 공식 {(초성×28x21)+(중성×28)+종성}+44032 
        // (종성 0~27, 중성 0~20)
        string resultText = initial;
        tmpText.text = resultText;
        foreach (char c in input)
        {
            yield return delay;
            if (c >= COMPOSITE_KOREAN_START_AT)
            {
                int composite = c - COMPOSITE_KOREAN_START_AT;
                int last = composite % 28;
                composite /= 28;
                int middle = composite % 21 * 28;
                composite /= 21;
                int first = composite;

                char letter = (char)(SINGLE_KOREAN_TABLE[first] + SINGLE_KOREAN_START_AT);
                resultText = $"{resultText}{letter}";
                tmpText.text = resultText;

                yield return delay;
                resultText = resultText.Remove(resultText.Length - 1);
                first = first * 21 * 28;
                letter = (char)(first + middle + COMPOSITE_KOREAN_START_AT);
                resultText = $"{resultText}{letter}";
                tmpText.text = resultText;

                if (last != 0)
                {
                    yield return delay;
                    resultText = resultText.Remove(resultText.Length - 1);
                    letter = (char)(first + middle + last + COMPOSITE_KOREAN_START_AT);
                    resultText = $"{resultText}{letter}";
                    tmpText.text = resultText;
                }
            }
            else
            {
                resultText = $"{resultText}{c}";
                tmpText.text = resultText;
            }
        }
        onTextEnd?.Invoke();
        bool underlined = false;
        bool useUnderbar = underbarDelay != null;
        while (useUnderbar)
        {
            yield return underbarDelay;
            if (underlined)
            {
                resultText = resultText.Remove(resultText.Length - 2);
            }
            else
            {
                resultText = $"{resultText} _";
            }
            underlined = !underlined;
            tmpText.text = resultText;
        }
    }
}
