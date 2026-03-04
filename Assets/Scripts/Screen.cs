using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Screen : MonoBehaviour
{
    [SerializeField] TextSequence sentenceSeq;
    [SerializeField] TextSequence yesSeq;
    [SerializeField] TextSequence sepSeq;
    [SerializeField] TextSequence noSeq;

    [SerializeField] TMP_Text widthTester;
    [SerializeField] GameObject screenOffPanel;
    [SerializeField] Button nextButton;
    [SerializeField] float textDelay;
    [SerializeField] float underbarDelay;
    [SerializeField] QuestionListSO questionList;
    public UnityEvent onScreenOn;
    private bool isOn;
    private WaitForSeconds textDelayWfs;
    private WaitForSeconds underbarDelayWfs;
    private int currentQuestionIndex;
    private int currentSentenceIndex;
    private Coroutine currentSentenceRoutine;

    void Awake()
    {
        onScreenOn.AddListener(ReadQuestion);
        onScreenOn.AddListener(() => onScreenOn.RemoveListener(ReadQuestion));
        nextButton.onClick.AddListener(ReadQuestion);
        textDelayWfs = new WaitForSeconds(textDelay);
        underbarDelayWfs = new WaitForSeconds(underbarDelay);

        noSeq.onTextEnd.AddListener(() =>
        {
            yesSeq.EnableButton();
            noSeq.EnableButton();
        });
        yesSeq.AddButtonListener(ReadQuestion);
        noSeq.AddButtonListener(ReadQuestion);
    }
    public void ReadQuestion()
    {
        DisableNext();
        yesSeq.ClearText();
        sepSeq.ClearText();
        noSeq.ClearText();
        yesSeq.DisableButton();
        noSeq.DisableButton();
        if (currentSentenceRoutine != null) StopCoroutine(currentSentenceRoutine);
        if (currentQuestionIndex >= questionList.list.Count) return;
        Question question = questionList.list[currentQuestionIndex];
        string sentence = question.sentences[currentSentenceIndex];
        bool last = currentSentenceIndex == question.sentences.Length - 1;
        currentSentenceRoutine = StartCoroutine(sentenceSeq.TextRoutine(sentence, textDelayWfs, underbarDelayWfs));
        if (last)
        {
            widthTester.text = question.yesString == "" ? "YES" : question.yesString;
            sentenceSeq.onTextEnd.AddListener(ReadAsking);
            sentenceSeq.onTextEnd.AddListener(() => sentenceSeq.onTextEnd.RemoveListener(ReadAsking));
            currentSentenceIndex = 0;
        }
        else
        {
            sentenceSeq.onTextEnd.AddListener(EnableNext);
            sentenceSeq.onTextEnd.AddListener(() => sentenceSeq.onTextEnd.RemoveListener(EnableNext));
            currentSentenceIndex++;
        }
    }
    public void ReadAsking()
    {
        Question question = questionList.list[currentQuestionIndex];
        string yes = question.yesString == "" ? "YES" : question.yesString;
        string no = question.noString == "" ? "NO" : question.noString;

        float width = widthTester.rectTransform.rect.width;
        yesSeq.SetCorrectPosition(width);
        StartCoroutine(yesSeq.TextRoutine(yes, null, null, false));
        yesSeq.onTextEnd.AddListener(() =>
        {
            StartCoroutine(sepSeq.TextRoutine("/", null, null, false));
            yesSeq.onTextEnd.RemoveAllListeners();
        });
        sepSeq.onTextEnd.AddListener(() =>
        {
            StartCoroutine(noSeq.TextRoutine(no, null, null, false));
            sepSeq.onTextEnd.RemoveAllListeners();
        });
        currentQuestionIndex++;
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
        onScreenOn?.Invoke();
    }
    public void Off()
    {
        screenOffPanel.SetActive(true);
        isOn = false;
    }

    public void EnableNext()
    {
        nextButton.enabled = true;
    }
    public void DisableNext()
    {
        nextButton.enabled = false;
    }


}
