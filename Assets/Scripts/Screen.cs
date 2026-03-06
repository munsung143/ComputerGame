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
    // screenContentsViewer 제공
    private ScreenContentsViewer viewer;
    [SerializeField] TextSequence sentenceSeq;
    [SerializeField] AskText ask;
    [SerializeField] float defaultTextDelay;
    [SerializeField] float defaultUnderbarDelay;
    [SerializeField] QuestionList questionList;
    [SerializeField] Button nextButton;

    // 사용
    [SerializeField] GameObject screenOffPanel;
    [SerializeField] CursorController cursor;
    [SerializeField] TimeText timer;
    public UnityEvent onScreenOn;
    private bool isOn;

    void Awake()
    {
        viewer = new ScreenContentsViewer(
            ask, 
            sentenceSeq, 
            nextButton, 
            questionList, 
            defaultTextDelay, 
            defaultUnderbarDelay);
        Off();
        onScreenOn.AddListener(FirstReader);
    }

    public void SetCursorVariables(float Depth)
    {
        cursor.SetValues((RectTransform)transform, Depth);
    }

    public void FirstReader()
    {
        onScreenOn.RemoveListener(FirstReader);
        //viewer.ReadNext();
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
}
