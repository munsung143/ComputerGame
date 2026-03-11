using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum TextQuestionState
{
  None,
  ReadingSentence,
  ReadSentence,
  ReadingAfterMessage,
  ReadAfterMessage,
}

public class TextQuestionController
{

  private AskText askText;
  private SentenceUIViewer sentenceUIViewer;


  private int currentSentenceIndex;
  private TextQuestion currentQuestion;
  public ISentencePrintable CurrentSentencePrintable
  {
    get
    {
      if (currentQuestion is ISentencePrintable i) return i;
      else return null;
    }
  }
  public string[] targetSentences;
  public string CurrentSentence => targetSentences[currentSentenceIndex];
  public bool IsLastSentence => currentSentenceIndex == targetSentences.Length - 1;
  public IYesNO CurrentYesNo
  {
    get
    {
      if (currentQuestion is IYesNO i) return i;
      else return null;
    }
  }
  public string CurrentYes => CurrentYesNo.YesText == "" ? "YES" : CurrentYesNo.YesText;
  public string CurrentNo => CurrentYesNo.NoText == "" ? "NO" : CurrentYesNo.NoText;
  public TextQuestionState state = TextQuestionState.None;

  private Action<UnityAction> addNextListener;
  private Action<UnityAction> removeNextListener;
  public TextQuestionController(
    AskText askText,
    Action<UnityAction> addNextListener,
    Action<UnityAction> removeNextListener,
    SentenceUIViewer sentenceUIViewer,
    TextQuestion textQuestion)
  {
    this.askText = askText;
    this.addNextListener = addNextListener;
    this.removeNextListener = removeNextListener;
    this.sentenceUIViewer = sentenceUIViewer;
    currentQuestion = textQuestion;
  }
  private void HideAsk()
  {
    askText.ClearAsking();
    askText.DisableAsking();
  }
  private void SetNextButtonToReadQuestion()
  {
    nextButton.onClick.RemoveListener(PlayEvent);
    nextButton.onClick.RemoveListener(ReadTargetSentence);
    nextButton.onClick.AddListener(ReadQuestion);
  }
  private void SetNextButtonToReadTargetSentence()
  {
    nextButton.onClick.RemoveListener(PlayEvent);
    nextButton.onClick.RemoveListener(ReadQuestion);
    nextButton.onClick.AddListener(ReadTargetSentence);
  }
  private void SetNextButtonToPlayEvent()
  {
    nextButton.onClick.RemoveListener(ReadQuestion);
    nextButton.onClick.RemoveListener(ReadTargetSentence);
    nextButton.onClick.AddListener(PlayEvent);
  }

  // 질문의 시작점
  public void ReadQuestion()
  {
    if (state != TextQuestionState.None) return;
    else if (CurrentSentencePrintable != null)
    {
      ReadSentenceQuestion();
    }
  }
  // 문장형 질문의 시작점 (항상 질문의 처음부터)
  private void ReadSentenceQuestion()
  {
    currentSentenceIndex = 0;
    targetSentences = CurrentSentencePrintable.Sentence;
    // 이때부터 다음 버튼을 누르면, 다음 질문이 아닌 다음 문장을 읽는다.
    SetNextButtonToReadTargetSentence();
    state = TextQuestionState.ReadSentence;
    ReadTargetSentence();
  }
  //
  private void ReadTargetSentence()
  {
    if (state == TextQuestionState.ReadSentence)
    {
      state = TextQuestionState.ReadingSentence;
      ReadSentence();
    }
    else if (state == TextQuestionState.ReadAfterMessage)
    {
      state = TextQuestionState.ReadingAfterMessage;
      ReadAfterMessage();
    }
  }
  // 문장을 순차적으로 읽는 부분
  private void ReadSentence()
  {
    HideAsk();
    PrintText(CurrentSentence);
    if (IsLastSentence)
    {
      if (CurrentYesNo == null) return;
      askText.SetWidthTesterText(CurrentYes);
      sentenceSeq.AddTextEndListner(PrintAskingListener);
    }
    else
    {
      currentSentenceIndex++;
      sentenceSeq.AddTextEndListner(SetReadSentenceListener);
    }
  }
  private void ReadAfterMessage()
  {
    HideAsk();
    PrintText(CurrentSentence);
    if (IsLastSentence)
    {
      SetNextButtonToPlayEvent();
      sentenceSeq.AddTextEndListner(SetNoneListener);
    }
    else
    {
      currentSentenceIndex++;
      sentenceSeq.AddTextEndListner(SetReadAfterMessageListener);
    }
  }

  private void PrintAskingListener()
  {
    sentenceSeq.RemoveTextEndListener(PrintAskingListener);
    askText.SetYesPositon();
    askText.ReadAsking(CurrentYes, CurrentNo);
    askText.AddYesButtonOnceListener(OnYes);
    askText.AddNoButtonOnceListener(OnNo);
  }
  private void SetReadSentenceListener()
  {
    sentenceSeq.RemoveTextEndListener(SetReadSentenceListener);
    state = TextQuestionState.ReadSentence;
  }
  private void SetReadAfterMessageListener()
  {
    sentenceSeq.RemoveTextEndListener(SetReadAfterMessageListener);
    state = TextQuestionState.ReadAfterMessage;
  }
  private void SetNoneListener()
  {
    sentenceSeq.RemoveTextEndListener(SetNoneListener);
    state = TextQuestionState.None;
  }
  private void PlayEvent()
  {
    if (state != TextQuestionState.None) return;
    if (yesClicked)
    {
      CurrentYesEvent.Invoke();
    }
    else
    {
      CurrentNoEvent.Invoke();
    }
  }

  private void PrintText(string text)
  {
    if (currentSentenceRoutine != null) CoroutineHelper.Stop(currentSentenceRoutine);
    currentSentenceRoutine = CoroutineHelper.Start(sentenceSeq.TextRoutine(
    text,
    defaultTextDelayWfs,
    defaultUnderbarDelayWfs));
  }

  private bool yesClicked;
  private void OnYes()
  {
    // 결과 메시지 출력부
    yesClicked = true;
    if (CurrentYesNo.YesMessage.Length != 0)
    {
      targetSentences = CurrentYesNo.YesMessage;
      currentSentenceIndex = 0;
      state = TextQuestionState.ReadAfterMessage;
      ReadTargetSentence();
    }
    else
    {
      CurrentYesEvent.Invoke();
    }
  }
  private void OnNo()
  {
    yesClicked = false;
    if (CurrentYesNo.NoMessage.Length != 0)
    {
      targetSentences = CurrentYesNo.NoMessage;
      currentSentenceIndex = 0;
      state = TextQuestionState.ReadAfterMessage;
      ReadTargetSentence();
    }
    else
    {
      CurrentNoEvent.Invoke();
    }
  }
}