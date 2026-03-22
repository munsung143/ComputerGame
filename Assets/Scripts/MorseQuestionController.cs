using Unity.VisualScripting;

public class MorseQuestionController : IQuestionReadable
{
  private AskText askText;
  private MorseQuestion currentQuestion;
  private YesNoController yesNoController;
  private Sentence sentence;

  private MorseQuestionStateController stateController;

  public MorseQuestionController(MorseQuestion currentQuestion, Sentence sentence, AskText askText)
  {
    this.currentQuestion = currentQuestion;
    this.sentence = sentence;
    this.askText = askText;
    stateController = new MorseQuestionStateController();
    yesNoController = new YesNoController(currentQuestion, stateController, askText, sentence);
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
      sentence.PrintMorse(currentQuestion.morseText);
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