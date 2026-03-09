using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ScreenViewingState
{
  None,
  ReadingSentence,
  ReadSentence,
  ReadingAfterMessage,
  ReadAfterMessage,
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

  public Question currentQuestion;
  public ISentencePrintable CurrentSentencePrintable
  {
    get
    {
      if (currentQuestion is ISentencePrintable i) return i;
      else return null;
    }
  }
  public string[] currentSentences;
  public string CurrentSentence => currentSentences[currentSentenceIndex];
  public bool IsLastSentence => currentSentenceIndex == currentSentences.Length - 1;
  public bool NoMoreQuestion => currentQuestionIndex >= questionList.clearQuestionCount;
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
    currentQuestion = questionList.selectedQuestions[currentQuestionIndex];
  }
  private void HideAsk()
  {
    ask.ClearAsking();
    ask.DisableAsking();
  }
  public void ReadQuestionByState()
  {
    if (state != ScreenViewingState.None) return;
    ReadCurrentQuestion();

  }
  private void ReadSentenceByState()
  {
    if (state == ScreenViewingState.ReadSentence)
    {
      state = ScreenViewingState.ReadingSentence;
      ReadSentence();
    }
    else if (state == ScreenViewingState.ReadAfterMessage)
    {
      state = ScreenViewingState.ReadingAfterMessage;
      ReadAfterMessage();
    }
  }


  // 질문의 시작점
  private void ReadCurrentQuestion()
  {
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
  private void ReadSentenceQuestion()
  {
    currentSentenceIndex = 0;
    currentSentences = CurrentSentencePrintable.Sentence;
    // 이때부터 다음 버튼을 누르면, 다음 질문이 아닌 다음 문장을 읽는다.
    nextButton.onClick.RemoveListener(ReadQuestionByState);
    nextButton.onClick.AddListener(ReadSentenceByState);
    state = ScreenViewingState.ReadingSentence;
    ReadSentence();
  }
  // 문장을 순차적으로 읽는 부분
  private void ReadSentence()
  {
    HideAsk();
    PrintText(CurrentSentence);
    if (IsLastSentence)
    {
      if (CurrentYesNo == null) return;
      ask.SetWidthTesterText(CurrentYes);
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
      nextButton.onClick.RemoveListener(ReadSentenceByState);
      nextButton.onClick.AddListener(PlayEventListener);
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
    ask.SetYesPositon();
    ask.ReadAsking(CurrentYes, CurrentNo);
    ask.AddYesButtonOnceListener(OnYes);
    ask.AddNoButtonOnceListener(OnNo);
  }
  private void SetReadSentenceListener()
  {
    sentenceSeq.RemoveTextEndListener(SetReadSentenceListener);
    state = ScreenViewingState.ReadSentence;
  }
  private void SetReadAfterMessageListener()
  {
    sentenceSeq.RemoveTextEndListener(SetReadAfterMessageListener);
    state = ScreenViewingState.ReadAfterMessage;
  }
  private void SetNoneListener()
  {
    sentenceSeq.RemoveTextEndListener(SetNoneListener);
    state = ScreenViewingState.None;
  }
  private void PlayEventListener()
  {
    if (state != ScreenViewingState.None) return;
    nextButton.onClick.RemoveListener(PlayEventListener);
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
    yesClicked = true;
    if (CurrentYesNo.YesMessage.Length != 0)
    {
      currentSentences = CurrentYesNo.YesMessage;
      currentSentenceIndex = 0;
      state = ScreenViewingState.ReadingAfterMessage;
      ReadAfterMessage();
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
      currentSentences = CurrentYesNo.NoMessage;
      currentSentenceIndex = 0;
      state = ScreenViewingState.ReadingAfterMessage;
      ReadAfterMessage();
    }
    else
    {
      CurrentNoEvent.Invoke();
    }
  }



  private void Next()
  {
    currentQuestionIndex++;
    currentQuestion = questionList.selectedQuestions[currentQuestionIndex];
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
    currentQuestion = questionList.codedQuestions[currentQuestion.followingQuestionCode];
    ReadCurrentQuestion();
  }
  private void GameEnd()
  {

  }
}