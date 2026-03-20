public enum MorseQuestionState
{
  None,
  Morsing,
  ReadAnswerMessage,
  ReadingAnswerMessage,
  ReadLastAnswerMessage
}

public class MorseQuestionStateController : IYesNoState
{
  private MorseQuestionState state = MorseQuestionState.None;

  public bool IsBusy => state == MorseQuestionState.Morsing || state == MorseQuestionState.ReadingAnswerMessage;
  public bool CanReadMorse => state == MorseQuestionState.None;
  public bool CanReadAnswer => state == MorseQuestionState.ReadAnswerMessage;
  public bool CanExecuteNext => state == MorseQuestionState.ReadLastAnswerMessage;
  public bool IsReadingAnswer => state == MorseQuestionState.ReadingAnswerMessage;

  public void OnReadingMorse()
  {
    state = MorseQuestionState.Morsing;

  }
  public void OnReadingAnswer()
  {
    state = MorseQuestionState.ReadingAnswerMessage;
  }
  public void OnReadAnswer(bool isLast)
  {
    state = isLast ? MorseQuestionState.ReadLastAnswerMessage : MorseQuestionState.ReadAnswerMessage;
  }
}