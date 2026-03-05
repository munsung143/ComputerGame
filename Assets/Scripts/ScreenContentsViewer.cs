using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenContentsViewer
{

  private AskText ask;
  private TextSequence sentenceSeq;
  private QuestionListSO questionList;

  private Button nextButton;
  private int currentQuestionIndex;
  private int currentSentenceIndex;
  public int CurrentQuestionIndex { get => currentQuestionIndex; }

  private Coroutine currentSentenceRoutine;

  private WaitForSeconds defaultTextDelayWfs;
  private WaitForSeconds defaultUnderbarDelayWfs;

  public int QuestionCount => questionList.list.Count;
  public Question CurrentQuestion => questionList.list[currentQuestionIndex];
  public string CurrentSentence => CurrentQuestion.sentences[currentSentenceIndex];
  public string CurrentYes => CurrentQuestion.yesString == "" ? "YES" : CurrentQuestion.yesString;
  public string CurrentNo => CurrentQuestion.noString == "" ? "NO" : CurrentQuestion.noString;
  public bool IsLastSentence => currentSentenceIndex == CurrentQuestion.sentences.Length - 1;
  public bool IsExceedQuestionCount => currentQuestionIndex >= QuestionCount;
  public ScreenContentsViewer(
    AskText ask, 
    TextSequence sentenceSeq, 
    Button nextButton, 
    QuestionListSO questionList, 
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
    //임시 - YES NO 클릭 시 항상 다음으로 넘어감
    ask.AddYesButtonListener(ReadQuestion);
    ask.AddNoButtonListener(ReadQuestion);
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


  public void ReadQuestion()
  {
    DisableNextButton();
    HideAsk();
    if (currentSentenceRoutine != null) CoroutineHelper.Stop(currentSentenceRoutine);
    if (IsExceedQuestionCount) return;
    currentSentenceRoutine = CoroutineHelper.Start(sentenceSeq.TextRoutine(
      CurrentSentence, 
      defaultTextDelayWfs, 
      defaultUnderbarDelayWfs));
    // 마지막 질문지 문장일 경우
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
}