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

    void Awake()
    {
        ClearAsking();
        DisableAsking();
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
    public void AddYesButtonListener(UnityAction action)
    {
        yesSeq.AddButtonListener(action);
    }
    public void AddNoButtonListener(UnityAction action)
    {
        noSeq.AddButtonListener(action);
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


    public void ReadAsking(string yes, string no)
    {
        CoroutineHelper.Start(yesSeq.TextRoutine(yes, null, null, false));
        yesSeq.onTextEnd.AddListener(() =>
        {
            yesSeq.onTextEnd.RemoveAllListeners();
            CoroutineHelper.Start(sepSeq.TextRoutine("/", null, null, false));
        });
        sepSeq.onTextEnd.AddListener(() =>
        {
            sepSeq.onTextEnd.RemoveAllListeners();
            CoroutineHelper.Start(noSeq.TextRoutine(no, null, null, false));
        });
    }
}
