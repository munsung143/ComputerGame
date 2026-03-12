using System;
using System.Collections.Generic;
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
  private IQuestionLoop questionLoop;
  private TextQuestion currentQuestion;

  private SentenceController sentenceController;
  private YesNoController yesNoController;
  public TextQuestionStateController stateController;
  public TextQuestionController(
    AskText askText,
    SentenceUIViewer sentenceUIViewer,
    TextQuestion textQuestion,
    IQuestionLoop questionLoop)
  {
    this.askText = askText;
    this.sentenceUIViewer = sentenceUIViewer;
    currentQuestion = textQuestion;
    this.questionLoop = questionLoop;
    stateController = new TextQuestionStateController();

    sentenceController = new SentenceController(currentQuestion, stateController, sentenceUIViewer);
    yesNoController = new YesNoController(currentQuestion, stateController, askText, sentenceUIViewer);

    sentenceUIViewer.AddSentenceEndListener(OnSentenceEnd);
    askText.AddYesButtonListener(yesNoController.OnYes);
    askText.AddNoButtonListener(yesNoController.OnNo);
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
    else if (stateController.CanReadSentence)
    {
      HideAsk();
      sentenceController.ReadSentence();
    }
    else if (stateController.CanReadAnswer)
    {
      HideAsk();
      yesNoController.ReadAnswer();
    }
    else if (stateController.CanExecuteNext)
    {
      yesNoController.InvokeEvent();
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
}