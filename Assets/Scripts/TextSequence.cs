using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextSequence
{

    private static int COMPOSITE_KOREAN_START_AT = 0xAC00;
    private static int SINGLE_KOREAN_START_AT = 0x3130;
    private static int[] SINGLE_KOREAN_TABLE = { 1, 2, 4, 7, 8, 9, 17, 18, 19, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
    private UnityEvent onTextEnd;
    private TextEditor editor;
    public TextSequence(TextEditor editor)
    {
        onTextEnd = new UnityEvent();
        this.editor = editor;
    }
    public void AddTextEndListner(UnityAction action)
    {
        onTextEnd.AddListener(action);
    }
    public void RemoveTextEndListener(UnityAction action)
    {
        onTextEnd.RemoveListener(action);
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
            sb.Append(' ');
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
        StringBuilder result = new StringBuilder(initial);
        editor.SetText(result.ToString());
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
                result.Append(letter);
                editor.SetText(result.ToString());

                yield return delay;
                first = first * 21 * 28;
                letter = (char)(first + middle + COMPOSITE_KOREAN_START_AT);
                result.Remove(result.Length - 1, 1);
                result.Append(letter);
                editor.SetText(result.ToString());

                if (last != 0)
                {
                    yield return delay;
                    letter = (char)(first + middle + last + COMPOSITE_KOREAN_START_AT);
                    result.Remove(result.Length - 1, 1);
                    result.Append(letter);
                    editor.SetText(result.ToString());
                }
            }
            else
            {
                result.Append(c);
                editor.SetText(result.ToString());
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
                result.Remove(result.Length - 2, 2);
            }
            else
            {
                result.Append(" _");
            }
            underlined = !underlined;
            editor.SetText(result.ToString());
        }
    }
}
