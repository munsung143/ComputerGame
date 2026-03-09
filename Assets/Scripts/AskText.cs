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


    private string yes;
    private string no;

    void Awake()
    {
        ClearAsking();
        DisableAsking();
    }
    void Start()
    {
        //NO 출력 후 항상 버튼 활성화 설정
        AddNoEndListner(EnableAsking);
    }

    public void ClearYes() => yesSeq.ClearText();
    public void ClearSep() => sepSeq.ClearText();
    public void ClearNo() => noSeq.ClearText();
    public void DisableYes() => yesSeq.DisableButton();
    public void DisableNo() => noSeq.DisableButton();

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

    public void AddNoEndListner(UnityAction action)
    {
        noSeq.AddTextEndListner(action);
    }
    UnityAction currentYesAction;
    UnityAction currentNoAction;
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
    public void YesListener()
    {
        yesSeq.RemoveButtonListener(YesListener);
        currentYesAction?.Invoke();
        currentYesAction = null;
    }
    public void NoListener()
    {
        noSeq.RemoveButtonListener(NoListener);
        currentNoAction?.Invoke();
        currentNoAction = null;
    }
    public void RemoveYesButtonListener(UnityAction action)
    {
        yesSeq.RemoveButtonListener(action);
    }
    public void RemoveNoButtonListener(UnityAction action)
    {
        noSeq.RemoveButtonListener(action);
    }
    public IEnumerator YesRoutine(string yes)
    {
        return yesSeq.TextRoutine(yes, null, null, false);
    }


    // 텍스트 설정 후, Content size fitter가 크기를 재조정하기까지 시간이 걸리기에, 분리하여 사용.
    public void SetWidthTesterText(string yes)
    {
        widthTester.text = yes;
    }

    public void SetYesPositon()
    {
        float width = widthTester.rectTransform.rect.width;
        yesSeq.SetCorrectPosition(width);
    }

    private void ReadSepAfterYes()
    {
        yesSeq.RemoveTextEndListener(ReadSepAfterYes);
        StartCoroutine(sepSeq.TextRoutine("/", null, null, false));
    }
    private void ReadNoAfterSep()
    {
        sepSeq.RemoveTextEndListener(ReadNoAfterSep);
        StartCoroutine(noSeq.TextRoutine(this.no, null, null, false));
    }
    public void ReadAsking(string yes, string no)
    {
        this.yes = yes;
        this.no = no;
        StartCoroutine(yesSeq.TextRoutine(yes, null, null, false));
        yesSeq.AddTextEndListner(ReadSepAfterYes);
        sepSeq.AddTextEndListner(ReadNoAfterSep);
    }
}
