public class SentenceController
{
  private int currentSentenceIndex;
  public ISentencePrintable currentSentencePrintable;
  public string[] sentences => currentSentencePrintable.Sentence;
  public string CurrentSentence => sentences[currentSentenceIndex];
  public bool IsLastSentence => currentSentenceIndex == sentences.Length - 1;
  private ISentenceState stateController;
  private SentenceUIViewer sentenceUIViewer;

  public SentenceController(ISentencePrintable sentencePrintable, ISentenceState stateController, SentenceUIViewer sentenceUIViewer)
  {
    currentSentencePrintable = sentencePrintable;
    this.stateController = stateController;
    this.sentenceUIViewer = sentenceUIViewer;
  }
  public void ReadSentence(int index)
  {
    stateController.OnReadingSentence();
    if (currentSentenceIndex == 0)
    {
      sentenceUIViewer.PrintTextWithIndex(CurrentSentence, index);
    }
    else
    {
      sentenceUIViewer.PrintTextWithInitialIndex(CurrentSentence, index);
    }
  }
  public void OnRead()
  {
    stateController.OnReadSentence(IsLastSentence);
    if (!IsLastSentence) currentSentenceIndex++;
  }

}