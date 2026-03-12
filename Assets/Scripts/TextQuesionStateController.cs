using Unity.VisualScripting;

public enum TextQuestionState
{
  ReadQuestionSentence,
  ReadingQuestionSentence,
  ReadLastQuestionSentence,
  ReadAnswerMessage,
  ReadingAnswerMessage,
  ReadLastAnswerMessage
}

public interface ISentenceState
{
  public void OnReadingSentence();
  public void OnReadSentence(bool isLast);
}
public interface IYesNoState
{
  public void OnReadingAnswer();
  public void OnReadAnswer(bool isLast);
}

public class TextQuestionStateController : ISentenceState, IYesNoState
{
  private TextQuestionState state = TextQuestionState.ReadQuestionSentence;
  public TextQuestionState State { get => state; }
  public bool IsBusy =>
    state == TextQuestionState.ReadingQuestionSentence ||
    state == TextQuestionState.ReadLastQuestionSentence ||
    state == TextQuestionState.ReadingAnswerMessage;

  public bool CanReadSentence => state == TextQuestionState.ReadQuestionSentence;
  public bool CanReadAnswer => state == TextQuestionState.ReadAnswerMessage;
  public bool CanExecuteNext => state == TextQuestionState.ReadLastAnswerMessage;
  public bool CanReadYesNo => state == TextQuestionState.ReadLastQuestionSentence;

  public bool IsReadingQuestion => state == TextQuestionState.ReadingQuestionSentence;
  public bool IsReadingAnswer => state == TextQuestionState.ReadingAnswerMessage;

  public TextQuestionStateController()
  {

  }

  public void OnReadingSentence()
  {
    state = TextQuestionState.ReadingQuestionSentence;

  }
  public void OnReadSentence(bool isLast)
  {
    state = isLast ? TextQuestionState.ReadLastQuestionSentence : TextQuestionState.ReadQuestionSentence;
  }
  public void OnReadingAnswer()
  {
    state = TextQuestionState.ReadingAnswerMessage;
  }
  public void OnReadAnswer(bool isLast)
  {
    state = isLast ? TextQuestionState.ReadLastAnswerMessage : TextQuestionState.ReadAnswerMessage;
  }
}