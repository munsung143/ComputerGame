using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class AskText : MonoBehaviour, ITextEffectPrivider
{
    [SerializeField] TextSequence yesSeq;
    [SerializeField] TextSequence sepSeq;
    [SerializeField] TextSequence noSeq;

    [SerializeField] TextButton yesButton;
    [SerializeField] TextButton noButton;
    [SerializeField] TMP_Text widthTester;


    private string yes;
    private string no;

    private bool isBinary;
    private string subject = "";
    private string postpositions = "";

    UnityAction currentYesAction;
    UnityAction currentNoAction;

    private WaitForSeconds initialTextDelay;
    private WaitForSeconds currentTextDelay;

    void Awake()
    {
        initialTextDelay = new WaitForSeconds(0.05f);
        AskingEventRegistry.askText = this;
    }
    void Start()
    {
        ClearAsking();
        DisableAsking();
        yesSeq.AddTextEndListner(() => StartCoroutine(sepSeq.TextRoutine("/", initialTextDelay)));
        sepSeq.AddTextEndListner(() => StartCoroutine(noSeq.TextRoutine(this.no, initialTextDelay)));
        noSeq.AddTextEndListner(EnableAsking);
    }

    public void SetBinaryState()
    {
        isBinary = true;
    }
    public void ResetBinaryState()
    {
        isBinary = false;
    }

    public void SetGoldenBallSubject()
    {
        subject = "금구슬";
        postpositions = "은이을과";
    }
    public void ResetSubject()
    {
        subject = "";
        postpositions = "";
    }

    public void SetFontSize(float size)
    {
        yesSeq.SetFontSize(size);
        sepSeq.SetFontSize(size);
        noSeq.SetFontSize(size);
        widthTester.fontSize = size;

    }
    public void SetTextDelay(float delay) => currentTextDelay = new WaitForSeconds(delay);

    public void ResetFontSize()
    {
        yesSeq.ResetFontSize();
        sepSeq.ResetFontSize();
        noSeq.ResetFontSize();
        widthTester.fontSize = 2;
    }
    public void ResetTextDelay() => currentTextDelay = initialTextDelay;
    public void EnableAsking()
    {
        yesButton.EnableButton();
        noButton.EnableButton();
    }
    public void DisableAsking()
    {
        yesButton.DisableButton();
        noButton.DisableButton();
    }
    public void ClearAsking()
    {
        yesSeq.ClearText();
        sepSeq.ClearText();
        noSeq.ClearText();
    }

    public void ReadAsking(string yes, string no)
    {
        yes = yesSeq.GetSpecificSubjectedText(yes, subject, postpositions);
        no = yesSeq.GetSpecificSubjectedText(no, subject, postpositions);
        if (isBinary)
        {
            yes = yesSeq.GetBinaryText(yes);
            no = yesSeq.GetBinaryText(no);
        }
        this.yes = yes;
        this.no = no;
        StartCoroutine(ReadAskingRoutine());
    }


    private IEnumerator ReadAskingRoutine()
    {
        widthTester.text = yes;
        yield return null;
        float width = widthTester.rectTransform.rect.width;
        RectTransform rectTransform = (RectTransform)yesSeq.transform;
        Vector3 pos = new Vector3(-1.5f - width, 0, 0);
        rectTransform.localPosition = pos;
        StartCoroutine(yesSeq.TextRoutine(yes, initialTextDelay));
    }
    public void AddYesButtonOnceListener(UnityAction action)
    {
        currentYesAction = action;
        yesButton.AddButtonListener(YesListener);
    }
    public void AddNoButtonOnceListener(UnityAction action)
    {
        currentNoAction = action;
        noButton.AddButtonListener(NoListener);
    }

    public void AddYesButtonListener(UnityAction action) => yesButton.AddButtonListener(action);
    public void AddNoButtonListener(UnityAction action) => noButton.AddButtonListener(action);
    public void RemoveYesButtonListener(UnityAction action) => yesButton.RemoveButtonListener(action);
    public void RemoveNoButtonListener(UnityAction action) => noButton.RemoveButtonListener(action);
    private void YesListener()
    {
        yesButton.RemoveButtonListener(YesListener);
        currentYesAction?.Invoke();
        currentYesAction = null;
    }
    private void NoListener()
    {
        noButton.RemoveButtonListener(NoListener);
        currentNoAction?.Invoke();
        currentNoAction = null;
    }
}
