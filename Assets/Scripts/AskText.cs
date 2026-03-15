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

    [SerializeField] TextButton yesButton;
    [SerializeField] TextButton noButton;
    [SerializeField] TMP_Text widthTester;


    private string yes;
    private string no;

    UnityAction currentYesAction;
    UnityAction currentNoAction;

    private WaitForSeconds textDelay;

    void Awake()
    {
        ClearAsking();
        DisableAsking();
        textDelay = new WaitForSeconds(0.05f);
    }
    void Start()
    {
        yesSeq.AddTextEndListner(() => StartCoroutine(sepSeq.TextRoutine("/", textDelay, null, false)));
        sepSeq.AddTextEndListner(() => StartCoroutine(noSeq.TextRoutine(this.no, textDelay, null, false)));
        noSeq.AddTextEndListner(EnableAsking);
    }
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
        StartCoroutine(yesSeq.TextRoutine(yes, null, null, false));
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
