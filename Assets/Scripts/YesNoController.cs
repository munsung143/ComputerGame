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
    askText.ClearAsking();
    askText.DisableAsking();
    stateController.OnReadingAnswer();
    sentenceUIViewer.PrintText(CurrentSentence);
  }

  public void OnRead()
  {
    stateController.OnReadAnswer(IsLastSentence);
    if (!IsLastSentence) currentSentenceIndex++;
  }
  public bool OnYes()
  {
    currentEvent = AskingEventHelper.GetEvent(currentYesNo.YesEvent);
    sentences = currentYesNo.YesMessage;
    return sentences.Length != 0;

  }
  public bool OnNo()
  {
    currentEvent = AskingEventHelper.GetEvent(currentYesNo.NoEvent);
    sentences = currentYesNo.NoMessage;
    return sentences.Length != 0;
  }

  public void InvokeEvent()
  {
    currentEvent.Invoke();
  }



}