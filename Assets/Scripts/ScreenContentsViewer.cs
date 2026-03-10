using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ScreenQuestionState
{
  None,
  ReadingSentence,
  ReadSentence,
  ReadingAfterMessage,
  ReadAfterMessage,
}

public class ScreenQuestionViewer
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
  public string[] targetSentences;
  public string CurrentSentence => targetSentences[currentSentenceIndex];
  public bool IsLastSentence => currentSentenceIndex == targetSentences.Length - 1;
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
  public ScreenQuestionState state = ScreenQuestionState.None;
  public ScreenQuestionViewer(
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
    if (state != ScreenQuestionState.None) return;
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
    targetSentences = CurrentSentencePrintable.Sentence;
    // 이때부터 다음 버튼을 누르면, 다음 질문이 아닌 다음 문장을 읽는다.
    SetNextButtonToReadTargetSentence();
    state = ScreenQuestionState.ReadSentence;
    ReadTargetSentence();
  }
  //
  private void ReadTargetSentence()
  {
    if (state == ScreenQuestionState.ReadSentence)
    {
      state = ScreenQuestionState.ReadingSentence;
      ReadSentence();
    }
    else if (state == ScreenQuestionState.ReadAfterMessage)
    {
      state = ScreenQuestionState.ReadingAfterMessage;
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
    ask.SetYesPositon();
    ask.ReadAsking(CurrentYes, CurrentNo);
    ask.AddYesButtonOnceListener(OnYes);
    ask.AddNoButtonOnceListener(OnNo);
  }
  private void SetReadSentenceListener()
  {
    sentenceSeq.RemoveTextEndListener(SetReadSentenceListener);
    state = ScreenQuestionState.ReadSentence;
  }
  private void SetReadAfterMessageListener()
  {
    sentenceSeq.RemoveTextEndListener(SetReadAfterMessageListener);
    state = ScreenQuestionState.ReadAfterMessage;
  }
  private void SetNoneListener()
  {
    sentenceSeq.RemoveTextEndListener(SetNoneListener);
    state = ScreenQuestionState.None;
  }
  private void PlayEvent()
  {
    if (state != ScreenQuestionState.None) return;
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
      state = ScreenQuestionState.ReadAfterMessage;
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
      state = ScreenQuestionState.ReadAfterMessage;
      ReadTargetSentence();
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
    state = ScreenQuestionState.None;
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
    currentQuestion = questionList.codedQuestions[currentQuestion.followingQuestionCode];
    state = ScreenQuestionState.None;
    ReadQuestion();
  }
  private void GameEnd()
  {

  }
}