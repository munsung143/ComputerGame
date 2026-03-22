using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IQuestionLoopEffectProvider
{
  public void Reset();
  public void ReverseNext();
  public void Next();
  public void ForcedStop();
  public void Following();
}

public enum QuestionState
{
  None,
  TextQuestion,
  MinigameQuestion
}
public class QuestionLoop : IQuestionLoopEffectProvider
{
  private Sentence sentence;
  private AskText askText;
  private QuestionList questionList;
  private int currentQuestionIndex;
  private Question[] questions;
  public Question currentQuestion;
  private bool reversed;
  public bool NoMoreQuestion => currentQuestionIndex >= questionList.clearQuestionCount;
  private IScreen screen;
  private ScreenEffectData effectData;

  public TextQuestion CurrentTextQuestion
  {
    get
    {
      if (currentQuestion is TextQuestion i) return i;
      else return null;
    }
  }
  public MorseQuestion CurrentMorseQuestion
  {
    get
    {
      if (currentQuestion is MorseQuestion i) return i;
      else return null;
    }
  }
  public IQuestionReadable questionReadable;

  public QuestionLoop(
    QuestionList questionList,
    IScreen screen,
    Sentence sentenceUIViewer,
    AskText askText,
    ScreenEffectData effectData)
  {
    AskingEventRegistry.questionLoop = this;
    this.questionList = questionList;
    this.screen = screen;
    this.sentence = sentenceUIViewer;
    this.askText = askText;
    this.effectData = effectData;
    SetNewQuestionArray();
    currentQuestion = questions[currentQuestionIndex];
  }

  public void SetNextIndex()
  {
    // 첫번째 질문일 경우도달 시 리버스 상태 해제 및 셔플
    if (reversed && currentQuestionIndex == 1)
    {
      SetNewQuestionArray();
      reversed = false;
      effectData.ResetSubject();
    }
    if (reversed) currentQuestionIndex--;
    else currentQuestionIndex++;
  }
  public void SetNewQuestionArray()
  {
    questions = questionList.GetRandomQuestionArray();
  }
  // 질문을 불러오고, 읽도록 세팅하는 기능
  public void PlayCurrentQuestion()
  {
    if (CurrentTextQuestion != null)
    {
      questionReadable = new TextQuestionController(
        askText,
        sentence,
        CurrentTextQuestion,
        currentQuestionIndex);
      questionReadable.ReadQuestion();
    }
    else if (CurrentMorseQuestion != null)
    {
      questionReadable = new MorseQuestionController(
        CurrentMorseQuestion,
        sentence,
        askText);
      questionReadable.ReadQuestion();
    }
    screen.AddNextButtonListener(questionReadable.ReadQuestion);
  }

  private void ResetNextButtonListener()
  {
    screen.RemoveNextButtonListener(questionReadable.ReadQuestion);
  }

  public void Reset()
  {
    ResetNextButtonListener();
    SetNewQuestionArray();
    currentQuestionIndex = 0;
    currentQuestion = questions[currentQuestionIndex];
    PlayCurrentQuestion();
  }

  public void ReverseNext()
  {
    reversed = true;
    Next();
  }

  public void Next()
  {
    ResetNextButtonListener();
    SetNextIndex();
    if (NoMoreQuestion)
    {
      GameEnd();
      return;
    }
    currentQuestion = questions[currentQuestionIndex];
    PlayCurrentQuestion();
  }
  public void ForcedStop()
  {
    Application.Quit();
  }
  public void Following()
  {
    ResetNextButtonListener();
    currentQuestion = questionList.codedQuestions[currentQuestion.followingQuestionCode];
    PlayCurrentQuestion();
  }

  private void GameEnd()
  {
    sentence.PrintAnswer("게임 끝");
  }


}