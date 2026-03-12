using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AskText : MonoBehaviour
{
    [SerializeField] TextSequence yesSeq;
    [SerializeField] TextSequence sepSeq;
    [SerializeField] TextSequence noSeq;
    [SerializeField] TMP_Text widthTester;


    public void ClearYes() => yesSeq.ClearText();
    public void ClearSep() => sepSeq.ClearText();
    public void ClearNo() => noSeq.ClearText();
    public void DisableYes() => yesSeq.DisableButton();
    public void DisableNo() => noSeq.DisableButton();


    private string yes;
    private string no;

    UnityAction currentYesAction;
    UnityAction currentNoAction;

    void Awake()
    {
        ClearAsking();
        DisableAsking();
    }
    void Start()
    {
        yesSeq.AddTextEndListner(() => StartCoroutine(sepSeq.TextRoutine("/", null, null, false)));
        sepSeq.AddTextEndListner(() => StartCoroutine(noSeq.TextRoutine(this.no, null, null, false)));
        noSeq.AddTextEndListner(EnableAsking);
    }
    public void EnableAsking()
    {
        yesSeq.EnableButton();
        noSeq.EnableButton();
    }
    public void DisableAsking()
    {
        yesSeq.DisableButton();
        noSeq.DisableButton();
    }
    public void ClearAsking()
    {
        yesSeq.ClearText();
        sepSeq.ClearText();
        noSeq.ClearText();
    }

    public void ReadAsking(string yes, string no)
    {
        this.yes = yes;
        this.no = no;
        StartCoroutine(ReadAskingRoutine());
    }

    private IEnumerator ReadAskingRoutine()
    {
        widthTester.text = yes;
        yield return null;
        float width = widthTester.rectTransform.rect.width;
        yesSeq.SetCorrectPosition(width);
        StartCoroutine(yesSeq.TextRoutine(yes, null, null, false));
    }
    public void AddYesButtonOnceListener(UnityAction action)
    {
        currentYesAction = action;
        yesSeq.AddButtonListener(YesListener);
    }
    public void AddNoButtonOnceListener(UnityAction action)
    {
        currentNoAction = action;
        noSeq.AddButtonListener(NoListener);
    }

    public void AddYesButtonListener(UnityAction action) => yesSeq.AddButtonListener(action);
    public void AddNoButtonListener(UnityAction action) => noSeq.AddButtonListener(action);
    public void RemoveYesButtonListener(UnityAction action) => yesSeq.RemoveButtonListener(action);
    public void RemoveNoButtonListener(UnityAction action) => noSeq.RemoveButtonListener(action);
    private void YesListener()
    {
        yesSeq.RemoveButtonListener(YesListener);
        currentYesAction?.Invoke();
        currentYesAction = null;
    }
    private void NoListener()
    {
        noSeq.RemoveButtonListener(NoListener);
        currentNoAction?.Invoke();
        currentNoAction = null;
    }
}
