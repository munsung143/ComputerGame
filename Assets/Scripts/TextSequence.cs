using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextSequence : MonoBehaviour
{

    private static int COMPOSITE_KOREAN_START_AT = 0xAC00;
    private static int[] FIRST_KOREAN_LETTER_TABLE = { 1, 2, 4, 7, 8, 9, 17, 18, 19, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
    [SerializeField] TMP_Text tmpText;
    [SerializeField] Button button;

    [SerializeField] float textDelay;
    [SerializeField] float underbarDelay;
    private UnityEvent onTextEnd;
    private WaitForSeconds textDelayWfs;
    private WaitForSeconds underbarDelayWfs;

    public void SetCorrectPosition(float width)
    {
        RectTransform rectTransform = (RectTransform)transform;
        Vector3 pos = new Vector3(-1.5f - width, 0, 0);
        rectTransform.localPosition = pos;
    }

    void Awake()
    {
        onTextEnd = new UnityEvent();
        textDelayWfs = new WaitForSeconds(textDelay);
        underbarDelayWfs = new WaitForSeconds(underbarDelay);
    }
    public void EnableButton()
    {
        if (button != null) button.enabled = true;
    }
    public void DisableButton()
    {
        if (button != null) button.enabled = false;
    }
    public void AddButtonListener(UnityAction action)
    {
        if (button == null) return;
        button.onClick.AddListener(action);
    }
    public void RemoveButtonListener(UnityAction action)
    {
        if (button == null) return;
        button.onClick.RemoveListener(action);
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

    public IEnumerator TextRoutine(string input, WaitForSeconds delay, WaitForSeconds underbarDelay, bool useUnderbar = true)
    {
        // char는 해당 문자의 유니코드 값만을 저장한다.
        // UTF-8 인코딩 방식으로 저장하지 않는다.
        // 한글 문자 형성 공식 {(초성×28x21)+(중성×28)+종성}+44032 
        // (종성 0~27, 중성 0~20)
        if (delay == null) delay = textDelayWfs;
        if (underbarDelay == null) underbarDelay = underbarDelayWfs;
        string resultText = "";
        tmpText.text = "";
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

                char letter = (char)(FIRST_KOREAN_LETTER_TABLE[first] + 0x3130);
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
        while (useUnderbar)
        {
            yield return underbarDelay;
            if (underlined)
            {
                resultText = resultText.Remove(resultText.Length - 1);
            }
            else
            {
                resultText = $"{resultText}_";
            }
            underlined = !underlined;
            tmpText.text = resultText;
        }
    }
}
