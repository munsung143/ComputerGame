using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenContentsViewer
{

  private AskText ask;
  private TextSequence sentenceSeq;
  private QuestionList questionList;

  private Button nextButton;
  private int currentQuestionIndex;
  private int currentSentenceIndex;
  public int CurrentQuestionIndex { get => currentQuestionIndex; }

  private Coroutine currentSentenceRoutine;

  private WaitForSeconds defaultTextDelayWfs;
  private WaitForSeconds defaultUnderbarDelayWfs;

  private Dictionary<string, UnityAction> events;

  private Question[] questions;
  public Question currentQuestion => questions[currentQuestionIndex];
  public bool IsExceedQuestionCount => currentQuestionIndex >= questions.Length;
  public ISentencePrintable currentSentencePrintable;
  public string CurrentSentence => currentSentencePrintable.Sentence[currentSentenceIndex];
  public bool IsLastSentence => currentSentenceIndex == CurrentSentence.Length - 1;
  public IYesNO currentYesNo;
  public string CurrentYes => currentYesNo.YesText == "" ? "YES" : currentYesNo.YesText;
  public string CurrentNo => currentYesNo.NoText == "" ? "NO" : currentYesNo.NoText;
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
    questions = questionList.GetRandomQuestionArray();
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
    nextButton.onClick.AddListener(ReadSentence);
    //임시 - YES NO 클릭 시 항상 다음으로 넘어감
    //ask.AddYesButtonListener(ReadNext);
    //ask.AddNoButtonListener(ReadNext);
  }

  public void SetNextQuestionAndReadSentence()
  {
    ReadSentence();
  }
  public void AddQuestionIndex()
  {
    currentQuestionIndex++;
  }

  public void SelectCurrentQuestion()
  {
    if (currentQuestion is ISentencePrintable i) currentSentencePrintable = i;
    if (currentQuestion is IYesNO j) currentYesNo = j;
  }
  private void ReadSentence()
  {
    PrintText(CurrentSentence);
    // 센텐스 출력 직후, 화면 버튼을 활성화 하거나, YES/NO를 출력한다.
    if (IsLastSentence)
    {
      currentSentenceIndex = 0;
      ask.SetWidthTesterText(CurrentYes);
      sentenceSeq.AddTextEndListner(PrintAskingAfterSentence);
    }
    else
    {
      currentSentenceIndex++;
      sentenceSeq.AddTextEndListner(EnableNextAfterSentence);
    }

  }

  private void PrintAskingAfterSentence()
  {
    sentenceSeq.RemoveTextEndListener(PrintAskingAfterSentence);
    ask.SetYesPositon();
    ask.ReadAsking(CurrentYes, CurrentNo);
    currentQuestionIndex++;
  }
  private void EnableNextAfterSentence()
  {
    sentenceSeq.RemoveTextEndListener(EnableNextAfterSentence);
    EnableNextButton();
  }

  public void PrintText(string text)
  {
    DisableNextButton();
    HideAsk();
    if (currentSentenceRoutine != null) CoroutineHelper.Stop(currentSentenceRoutine);
    currentSentenceRoutine = CoroutineHelper.Start(sentenceSeq.TextRoutine(
    text,
    defaultTextDelayWfs,
    defaultUnderbarDelayWfs));
  }
}