using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ScreenViewingState
{
  Ready, // 화면 클릭해서 다음으로 넘어갈 준비가 됨.
  DoSomething // 다음으로 넘어갈 준비가 되지 않음.
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
  public bool NoMoreQuestion => currentQuestionIndex >= questionList.clearQuestionCount;
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
  public void Init()
  {
    nextButton.onClick.AddListener(ReadQuestionByState);
    questions = questionList.GetRandomQuestionArray();
    events = new Dictionary<AskingEvent, UnityAction>();
    events.Add(AskingEvent.Next, Next);
    events.Add(AskingEvent.ForceStop, ForceStop);
    events.Add(AskingEvent.FollowQuestion, FollowQuestion);
    events.Add(AskingEvent.Reset, Reset);
  }
  private void HideAsk()
  {
    ask.ClearAsking();
    ask.DisableAsking();
  }
  public void ReadQuestionByState()
  {
    if (state != ScreenViewingState.Ready) return;
    ReadCurrentQuestion();

  }
  private void ReadSentenceByState()
  {
    if (state != ScreenViewingState.Ready) return;
    ReadSentence();
  }


  // 질문의 시작점
  private void ReadCurrentQuestion()
  {
    state = ScreenViewingState.DoSomething;
    if (NoMoreQuestion)
    {
      GameEnd();
    }
    else if (CurrentSentencePrintable != null)
    {
      ReadSentenceQuestion();
    }
  }

  // 문장형 질문의 시작점 (항상 질문의 처음부터)
  // 이때부터 다음 버튼을 누르면, 다음 질문이 아닌 다음 문장을 읽는다.
  private void ReadSentenceQuestion()
  {
    currentSentenceIndex = 0;
    nextButton.onClick.RemoveListener(ReadQuestionByState);
    nextButton.onClick.AddListener(ReadSentenceByState);
    ReadSentence();
  }
  // 질문의 문장을 순차적으로 읽는 부분
  private void ReadSentence()
  {
    state = ScreenViewingState.DoSomething;
    HideAsk();
    PrintText(CurrentSentence);
    if (IsLastSentence)
    {
      nextButton.onClick.RemoveListener(ReadSentenceByState);
      if (CurrentYesNo == null) return;
      ask.SetWidthTesterText(CurrentYes);
      sentenceSeq.AddTextEndListner(PrintAskingListener);
    }
    else
    {
      currentSentenceIndex++;
      sentenceSeq.AddTextEndListner(SetReadyListener);
    }
  }

  private void PrintAskingListener()
  {
    sentenceSeq.RemoveTextEndListener(PrintAskingListener);
    ask.SetYesPositon();
    ask.ReadAsking(CurrentYes, CurrentNo);
    ask.AddYesButtonOnceListener(CurrentYesEvent);
    ask.AddNoButtonOnceListener(CurrentNoEvent);
  }
  private void SetReadyListener()
  {
    sentenceSeq.RemoveTextEndListener(SetReadyListener);
    state = ScreenViewingState.Ready;
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
    ReadCurrentQuestion();
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
  private void GameEnd()
  {

  }
}