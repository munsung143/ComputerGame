public class SentenceController
{
  private int currentSentenceIndex;
  public ISentencePrintable currentSentencePrintable;
  public string[] sentences => currentSentencePrintable.Sentence;
  public string CurrentSentence => sentences[currentSentenceIndex];
  public bool IsLastSentence => currentSentenceIndex == sentences.Length - 1;
  private ISentenceState stateController;
  private Sentence sentence;

  public SentenceController(ISentencePrintable sentencePrintable, ISentenceState stateController, Sentence sentence)
  {
    currentSentencePrintable = sentencePrintable;
    this.stateController = stateController;
    this.sentence = sentence;
  }
  public void ReadSentence(int index)
  {
    stateController.OnReadingSentence();
    sentence.PrintSentence(CurrentSentence, index, currentSentenceIndex == 0);
  }
  public void OnRead()
  {
    stateController.OnReadSentence(IsLastSentence);
    if (!IsLastSentence) currentSentenceIndex++;
  }

}