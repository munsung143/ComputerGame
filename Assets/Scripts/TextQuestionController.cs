using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IQuestionReadable
{
  public void ReadQuestion();
}

public class TextQuestionController : IQuestionReadable
{

  private AskText askText;
  private SentenceUIViewer sentenceUIViewer;
  private TextQuestion currentQuestion;

  private SentenceController sentenceController;
  private YesNoController yesNoController;
  public TextQuestionStateController stateController;

  private int index;
  public TextQuestionController(
    AskText askText,
    SentenceUIViewer sentenceUIViewer,
    TextQuestion textQuestion,
    int index)
  {
    this.askText = askText;
    this.sentenceUIViewer = sentenceUIViewer;
    this.index = index;
    currentQuestion = textQuestion;
    stateController = new TextQuestionStateController();

    sentenceController = new SentenceController(currentQuestion, stateController, sentenceUIViewer);
    yesNoController = new YesNoController(currentQuestion, stateController, askText, sentenceUIViewer);

    sentenceUIViewer.AddSentenceEndListener(OnSentenceEnd);
    askText.AddYesButtonListener(OnYesClicked);
    askText.AddNoButtonListener(OnNoClicked);
  }
  private void HideAsk()
  {
    askText.ClearAsking();
    askText.DisableAsking();
  }

  // 질문의 시작점
  public void ReadQuestion()
  {
    if (stateController.IsBusy) return;
    if (stateController.CanReadSentence)
    {
      sentenceController.ReadSentence(index);
    }
    else if (stateController.CanReadAnswer)
    {
      yesNoController.ReadAnswer();
    }
    else if (stateController.CanExecuteNext)
    {
      InvokeEvent();
    }
  }

  private void OnSentenceEnd()
  {
    if (stateController.IsReadingQuestion)
    {
      sentenceController.OnRead();
      if (stateController.CanReadYesNo) yesNoController.ReadYesNo();
    }
    else if (stateController.IsReadingAnswer)
    {
      yesNoController.OnRead();
    }
  }
  private void InvokeEvent()
  {
    sentenceUIViewer.RemoveSentenceEndListener(OnSentenceEnd);
    askText.RemoveYesButtonListener(OnYesClicked);
    askText.RemoveNoButtonListener(OnNoClicked);
    yesNoController.InvokeEvent();
  }

  private void OnYesClicked()
  {
    bool CanReadAnswer = yesNoController.OnYes();
    if (CanReadAnswer) yesNoController.ReadAnswer();
    else InvokeEvent();
  }
  private void OnNoClicked()
  {
    bool CanReadAnswer = yesNoController.OnNo();
    if (CanReadAnswer) yesNoController.ReadAnswer();
    else InvokeEvent();
  }
}