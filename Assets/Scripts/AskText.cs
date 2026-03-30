using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class AskText : MonoBehaviour
{

    [SerializeField] TextButton yesButton;
    [SerializeField] TextButton noButton;


    [SerializeField] TMP_Text yesTmp;
    [SerializeField] TMP_Text sepTmp;
    [SerializeField] TMP_Text noTmp;
    [SerializeField] TMP_Text widthTester;

    private TextSeq yesSeq;
    private TextSeq sepSeq;
    private TextSeq noSeq;

    public ScreenTextEffectData effectData;

    private string yes;
    private string no;
    private float textDelay = 0.03f;
    private float initialFontSize;

    void Awake()
    {
        yesSeq = new TextSeq(yesTmp);
        sepSeq = new TextSeq(sepTmp);
        noSeq = new TextSeq(noTmp);
        initialFontSize = yesTmp.fontSize;
    }
    void Start()
    {
        ClearAsking();
        DisableAsking();
        yesSeq.AddTextEndListner(() => StartCoroutine(sepSeq.TextRoutine("/", WaitForSecondsPool.Get(effectData.textSpeedMult * textDelay))));
        sepSeq.AddTextEndListner(() => StartCoroutine(noSeq.TextRoutine(this.no, WaitForSecondsPool.Get(effectData.textSpeedMult * textDelay))));
        noSeq.AddTextEndListner(EnableAsking);
        //effectData.onFontMultSet += SetFontSize;
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
        yesTmp.text = "";
        sepTmp.text = "";
        noTmp.text = "";
    }
    public void SetFontSize()
    {
        yesTmp.fontSize = effectData.fontSizeMult * initialFontSize;
        sepTmp.fontSize = effectData.fontSizeMult * initialFontSize;
        noTmp.fontSize = effectData.fontSizeMult * initialFontSize;
        widthTester.fontSize = effectData.fontSizeMult * initialFontSize;
    }

    public void ReadAsking(string yes, string no)
    {
        SetFontSize();
        yes = effectData.GetFormattedText(yes);
        no = effectData.GetFormattedText(no);
        this.yes = yes;
        this.no = no;
        StartCoroutine(ReadAskingRoutine());
    }


    private IEnumerator ReadAskingRoutine()
    {
        widthTester.text = yes;
        yield return null;
        float width = widthTester.rectTransform.rect.width;
        RectTransform rectTransform = (RectTransform)yesTmp.transform;
        Vector3 pos = new Vector3(-1.5f - width, 0, 0);
        rectTransform.localPosition = pos;
        StartCoroutine(yesSeq.TextRoutine(yes, WaitForSecondsPool.Get(effectData.textSpeedMult * textDelay)));
    }

    public void AddYesButtonListener(UnityAction action) => yesButton.AddButtonListener(action);
    public void AddNoButtonListener(UnityAction action) => noButton.AddButtonListener(action);
    public void RemoveYesButtonListener(UnityAction action) => yesButton.RemoveButtonListener(action);
    public void RemoveNoButtonListener(UnityAction action) => noButton.RemoveButtonListener(action);
}
