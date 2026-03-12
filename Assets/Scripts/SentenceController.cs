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
  // 문장을 순차적으로 읽는 부분
  public void ReadSentence()
  {
    stateController.OnReadingSentence();
    sentenceUIViewer.PrintText(CurrentSentence);
  }
  public void OnRead()
  {
    stateController.OnReadSentence(IsLastSentence);
    if (!IsLastSentence) currentSentenceIndex++;
  }

}