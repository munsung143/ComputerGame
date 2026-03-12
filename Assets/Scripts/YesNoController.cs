using System;
using UnityEngine.Events;

public class YesNoController
{

  private int currentSentenceIndex;
  public string[] sentences;
  public string CurrentSentence => sentences[currentSentenceIndex];
  public bool IsLastSentence => currentSentenceIndex == sentences.Length - 1;
  public IYesNO currentYesNo;
  public string CurrentYes => currentYesNo.YesText == "" ? "YES" : currentYesNo.YesText;
  public string CurrentNo => currentYesNo.NoText == "" ? "NO" : currentYesNo.NoText;
  private bool yesClicked;
  public AskText askText;
  public UnityAction currentEvent;
  private SentenceUIViewer sentenceUIViewer;
  private IYesNoState stateController;
  public YesNoController(IYesNO yesNO, IYesNoState stateController, AskText askText, SentenceUIViewer sentenceUIViewer)
  {
    currentYesNo = yesNO;
    this.stateController = stateController;
    this.askText = askText;
    this.sentenceUIViewer = sentenceUIViewer;
  }

  public void ReadYesNo()
  {
    askText.ReadAsking(CurrentYes, CurrentNo);
  }

  public void ReadAnswer()
  {
    stateController.OnReadingAnswer();
    sentenceUIViewer.PrintText(CurrentSentence);
  }

  public void OnRead()
  {
    stateController.OnReadAnswer(IsLastSentence);
    if (!IsLastSentence) currentSentenceIndex++;
  }
  public void OnYes()
  {
    // 결과 메시지 출력부
    yesClicked = true;
    currentEvent = AskingEventHelper.GetEvent(currentYesNo.YesEvent);
    if (currentYesNo.YesMessage.Length != 0)
    {
      sentences = currentYesNo.YesMessage;
      ReadAnswer();
    }
    else
    {
      InvokeEvent();
    }
  }
  public void OnNo()
  {
    yesClicked = false;
    currentEvent = AskingEventHelper.GetEvent(currentYesNo.NoEvent);
    if (currentYesNo.NoMessage.Length != 0)
    {
      sentences = currentYesNo.NoMessage;
      ReadAnswer();
    }
    else
    {
      InvokeEvent();
    }
  }

  public void InvokeEvent()
  {
    askText.RemoveYesButtonListener(OnYes);
    askText.RemoveNoButtonListener(OnNo);
    currentEvent.Invoke();
  }



}