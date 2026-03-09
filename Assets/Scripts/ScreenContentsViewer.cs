using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ScreenViewingState
{
  None,
  Sentencing,
  SentenceEnd
  
}

public class ScreenContentsViewer
{

  private AskText ask;
  private TextSequence sentenceSeq;
  private QuestionList questionList;
  private Button nextButton;
  private int currentQuestionIndex;
  private int currentSentenceIndex;
  private Coroutine currentSentenceRoutine;
  private WaitForSeconds defaultTextDelayWfs;
  private WaitForSeconds defaultUnderbarDelayWfs;

  private Dictionary<AskingEvent, UnityAction> events;
  private Question[] questions;

  public Question CurrentQuestion => questions[currentQuestionIndex];
  public bool IsExceedQuestionCount => currentQuestionIndex >= questions.Length;
  public ISentencePrintable CurrentSentencePrintable
  {
    get
    {
      if (CurrentQuestion is ISentencePrintable i) return i;
      else return null;
    }
  }
  public string CurrentSentence => CurrentSentencePrintable.Sentence[currentSentenceIndex];
  public bool IsLastSentence => currentSentenceIndex == CurrentSentencePrintable.Sentence.Length - 1;
  public IYesNO CurrentYesNo
  {
    get
    {
      if (CurrentQuestion is IYesNO i) return i;
      else return null;
    }
  }
  public string CurrentYes => CurrentYesNo.YesText == "" ? "YES" : CurrentYesNo.YesText;
  public string CurrentNo => CurrentYesNo.NoText == "" ? "NO" : CurrentYesNo.NoText;
  public UnityAction CurrentYesEvent => events[CurrentYesNo.YesEvent];
  public UnityAction CurrentNoEvent => events[CurrentYesNo.NoEvent];
  public ScreenViewingState state;
  public ScreenContentsViewer(
    AskText ask,
    TextSequence sentenceSeq,
    Button nextButton,
    QuestionList questionList,
    float textDelay,
    float underbarDelay)
  {
    this.ask = ask;
    this.questionList = questionList;
    this.sentenceSeq = sentenceSeq;
    this.nextButton = nextButton;
    defaultTextDelayWfs = new WaitForSeconds(textDelay);
    defaultUnderbarDelayWfs = new WaitForSeconds(underbarDelay);
    Init();
  }

  public void HideAsk()
  {
    ask.ClearAsking();
    ask.DisableAsking();
  }
  public void EnableNextButton()
  {
    nextButton.enabled = true;
  }
  public void DisableNextButton()
  {
    nextButton.enabled = false;
  }
  public void Init()
  {
    nextButton.onClick.AddListener(ReadQuestion);
    questions = questionList.GetRandomQuestionArray();
    events = new Dictionary<AskingEvent, UnityAction>();
    events.Add(AskingEvent.Next, Next);
    events.Add(AskingEvent.ForceStop, ForceStop);
    events.Add(AskingEvent.FollowQuestion, FollowQuestion);
    events.Add(AskingEvent.Reset, Reset);
  }
  public void ReadQuestion()
  {
    if (CurrentSentencePrintable == null) return;
    DisableNextButton();
    HideAsk();
    PrintText(CurrentSentence);
    // 센텐스 출력 직후, 화면 버튼을 활성화 하거나, YES/NO를 출력한다.
    if (IsLastSentence)
    {
      currentSentenceIndex = 0;
      if (CurrentYesNo == null) return;
      ask.SetWidthTesterText(CurrentYes);
      sentenceSeq.AddTextEndListner(PrintAskingListener);
    }
    else
    {
      currentSentenceIndex++;
      sentenceSeq.AddTextEndListner(EnableNextButtonListener);
    }

  }

  private void PrintAskingListener()
  {
    sentenceSeq.RemoveTextEndListener(PrintAskingListener);
    ask.SetYesPositon();
    ask.ReadAsking(CurrentYes, CurrentNo);
    ask.AddYesButtonListener(CurrentYesEvent);
    ask.AddNoButtonListener(CurrentNoEvent);
  }
  private void EnableNextButtonListener()
  {
    sentenceSeq.RemoveTextEndListener(EnableNextButtonListener);
    EnableNextButton();
  }

  private void PrintText(string text)
  {
    if (currentSentenceRoutine != null) CoroutineHelper.Stop(currentSentenceRoutine);
    currentSentenceRoutine = CoroutineHelper.Start(sentenceSeq.TextRoutine(
    text,
    defaultTextDelayWfs,
    defaultUnderbarDelayWfs));
  }

  private void Next()
  {
    currentQuestionIndex++;
    Debug.Log(currentQuestionIndex);
    ReadQuestion();
  }
  private void ForceStop()
  {
    Application.Quit();
  }
  private void Reset()
  {
    
  }
  private void FollowQuestion()
  {
    
  }
}