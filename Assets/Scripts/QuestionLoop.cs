using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IQuestionLoop
{
  public void AddQuestionIndex();
  public void ResetQuestionIndex();
  public void SetNewQuestionArray();
  public void SetCurrentQuestion();
  public void SetFollowingQuestion(string code);
  public void PlayCurrentQuestion();

}

public enum QuestionState
{
  None,
  TextQuestion,
  MinigameQuestion
}
public class QuestionLoop
{
  private SentenceUIViewer sentenceUIViewer;
  private AskText askText;
  private QuestionList questionList;
  private int currentQuestionIndex;
  private Question[] questions;
  public Question currentQuestion;
  private QuestionState state;
  public bool NoMoreQuestion => currentQuestionIndex >= questionList.clearQuestionCount;

  private IScreen screen;

  public TextQuestion CurrentTextQuestion
  {
    get
    {
      if (currentQuestion is TextQuestion i) return i;
      else return null;
    }
  }
  public IQuestionReadable questionReadable;

  public QuestionLoop(
    QuestionList questionList,
    IScreen screen,
    SentenceUIViewer sentenceUIViewer,
    AskText askText)
  {
    this.questionList = questionList;
    this.screen = screen;
    this.sentenceUIViewer = sentenceUIViewer;
    this.askText = askText;

    AskingEventHelper.AddEvent(AskingEvent.Next, Next);
    AskingEventHelper.AddEvent(AskingEvent.FollowQuestion, Following);
    AskingEventHelper.AddEvent(AskingEvent.ForceStop, ForcedStop);
    AskingEventHelper.AddEvent(AskingEvent.Reset, Reset);

    SetNewQuestionArray();
    currentQuestion = questions[currentQuestionIndex];
  }
  public void ResetQuestionIndex()
  {
    currentQuestionIndex = 1;
  }
  public void SetNewQuestionArray()
  {
    questions = questionList.GetRandomQuestionArray();
  }
  public void SetCurrentQuestion()
  {
    currentQuestion = questions[currentQuestionIndex];
  }
  public void SetFollowingQuestion(string code)
  {
    currentQuestion = questionList.codedQuestions[code];
  }
  // 질문을 불러오고, 읽도록 세팅하는 기능
  public void PlayCurrentQuestion()
  {
    if (CurrentTextQuestion != null)
    {
      state = QuestionState.TextQuestion;
      questionReadable = new TextQuestionController(
        askText,
        sentenceUIViewer,
        CurrentTextQuestion,
        currentQuestionIndex);
      screen.AddNextButtonListener(questionReadable.ReadQuestion);
      questionReadable.ReadQuestion();
    }
  }

  private void ResetScreen()
  {
    askText.ClearAsking();
    askText.DisableAsking();
    screen.RemoveNextButtonListener(questionReadable.ReadQuestion);
  }

  private void Reset()
  {
    ResetScreen();
    SetNewQuestionArray();
    currentQuestionIndex = 1;
    currentQuestion = questions[currentQuestionIndex];
    PlayCurrentQuestion();
  }

  private void Next()
  {
    ResetScreen();
    currentQuestionIndex++;
    if (NoMoreQuestion)
    {
      GameEnd();
      return;
    }
    currentQuestion = questions[currentQuestionIndex];
    PlayCurrentQuestion();
  }
  private void ForcedStop()
  {
    Application.Quit();
  }
  private void Following()
  {
    ResetScreen();
    currentQuestion = questionList.codedQuestions[currentQuestion.followingQuestionCode];
    PlayCurrentQuestion();
  }

  private void GameEnd()
  {
    sentenceUIViewer.PrintText("게임 끝");
  }


}