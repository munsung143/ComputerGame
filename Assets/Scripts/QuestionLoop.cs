using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
  public TextQuestionController textQuestionController;

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
  }

  public void AddQuestionIndex()
  {
    currentQuestionIndex++;
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
  public void PlayCurrentQuestion()
  {
    if (CurrentTextQuestion != null)
    {
      state = QuestionState.TextQuestion;
      //textQuestionController = new TextQuestionController();
    }
  }


}