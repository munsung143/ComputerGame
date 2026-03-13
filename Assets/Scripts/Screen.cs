using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IScreen
{
    public void AddNextButtonListener(UnityAction action);
    public void RemoveNextButtonListener(UnityAction action);
}

public class Screen : MonoBehaviour, IScreen
{
    // 제공
    private QuestionLoop loop;
    [SerializeField] TextSequence sentenceSeq;
    [SerializeField] AskText ask;
    [SerializeField] QuestionList questionList;
    [SerializeField] Button nextButton;

    // 사용
    [SerializeField] GameObject screenOffPanel;
    [SerializeField] CursorController cursor;
    [SerializeField] TimeText timer;
    [SerializeField] RedScreen redScreen;
    public UnityEvent onScreenOn;
    private bool isOn;

    void Awake()
    {
        loop = new QuestionLoop(
            questionList,
            this,
            new SentenceUIViewer(sentenceSeq),
            ask);
        Off();
        onScreenOn.AddListener(ReadQustionListener);
        AskingEventHelper.AddEvent(AskingEvent.RedScreen, redScreen.ScreenOn);
    }

    public void SetCursorVariables(float Depth)
    {
        cursor.SetValues((RectTransform)transform, Depth);
    }

    public void ReadQustionListener()
    {
        onScreenOn.RemoveListener(ReadQustionListener);
        loop.PlayCurrentQuestion();
        timer.TimerOn = true;
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
        cursor.GetScreenState(isOn);
    }
    public void Off()
    {
        screenOffPanel.SetActive(true);
        isOn = false;
        cursor.GetScreenState(isOn);
    }
    public void AddNextButtonListener(UnityAction action)
    {
        nextButton.onClick.AddListener(action);
    }
    public void RemoveNextButtonListener(UnityAction action)
    {
        nextButton.onClick.RemoveListener(action);
    }
}
