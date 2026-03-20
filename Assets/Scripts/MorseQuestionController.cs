using Unity.VisualScripting;

public class MorseQuestionController : IQuestionReadable
{

  private Morse morse;
  private AskText askText;
  private MorseQuestion currentQuestion;
  private YesNoController yesNoController;
  private SentenceSequenceViewer sentenceUIViewer;

  private MorseQuestionStateController stateController;

  public MorseQuestionController(Morse morse, MorseQuestion currentQuestion, SentenceSequenceViewer sentenceUIViewer, AskText askText)
  {
    this.morse = morse;
    this.currentQuestion = currentQuestion;
    this.sentenceUIViewer = sentenceUIViewer;
    this.askText = askText;
    stateController = new MorseQuestionStateController();
    yesNoController = new YesNoController(currentQuestion, stateController, askText, sentenceUIViewer);
    askText.AddYesButtonListener(OnYesClicked);
    askText.AddNoButtonListener(OnNoClicked);

  }
  private void HideAsk()
  {
    askText.ClearAsking();
    askText.DisableAsking();
  }


  public void ReadQuestion()
  {
    if (stateController.IsBusy) return;
    if (stateController.CanReadMorse)
    {
      stateController.OnReadingMorse();
      morse.PrintMorse(currentQuestion.morseText);
      yesNoController.ReadYesNo();
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
  private void OnYesClicked()
  {
    HideAsk();
    bool CanReadAnswer = yesNoController.OnYes();
    if (CanReadAnswer) yesNoController.ReadAnswer();
    else InvokeEvent();
  }
  private void OnNoClicked()
  {
    HideAsk();
    bool CanReadAnswer = yesNoController.OnNo();
    if (CanReadAnswer) yesNoController.ReadAnswer();
    else InvokeEvent();
  }
  private void InvokeEvent()
  {
    askText.RemoveYesButtonListener(OnYesClicked);
    askText.RemoveNoButtonListener(OnNoClicked);
    yesNoController.InvokeEvent();
  }

}