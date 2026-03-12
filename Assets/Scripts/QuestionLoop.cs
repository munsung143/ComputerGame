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
public class QuestionLoop : IQuestionLoop
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
  public void SetNextQuestion()
  {
    AddQuestionIndex();
    SetCurrentQuestion();
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
        this);
        //TODO : 여기서 스크린 넥스트버튼 기존 리스너 제거 기능 필요할 듯

      screen.AddNextButtonListener(questionReadable.ReadQuestion);
      questionReadable.ReadQuestion();
    }
  }


}