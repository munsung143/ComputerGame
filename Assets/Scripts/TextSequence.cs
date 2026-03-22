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
    private TMP_Text tmp;
    public TextSequence(TMP_Text tmp)
    {
        onTextEnd = new UnityEvent();
        this.tmp = tmp;
    }
    public void AddTextEndListner(UnityAction action)
    {
        onTextEnd.AddListener(action);
    }
    public void RemoveTextEndListener(UnityAction action)
    {
        onTextEnd.RemoveListener(action);
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
        tmp.SetText(result.ToString());
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
                tmp.SetText(result.ToString());

                yield return delay;
                first = first * 21 * 28;
                letter = (char)(first + middle + COMPOSITE_KOREAN_START_AT);
                result.Remove(result.Length - 1, 1);
                result.Append(letter);
                tmp.SetText(result.ToString());

                if (last != 0)
                {
                    yield return delay;
                    letter = (char)(first + middle + last + COMPOSITE_KOREAN_START_AT);
                    result.Remove(result.Length - 1, 1);
                    result.Append(letter);
                    tmp.SetText(result.ToString());
                }
            }
            else
            {
                result.Append(c);
                tmp.SetText(result.ToString());
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
            tmp.SetText(result.ToString());
        }
    }
}
