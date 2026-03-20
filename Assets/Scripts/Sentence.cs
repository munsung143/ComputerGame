using TMPro;
using UnityEngine;

public class Sentence : MonoBehaviour
{
  [SerializeField] TMP_Text tmpText;
  private Morse morse;
  private TextSequence textSequence;
  private TextEditor textEditor;
  private SentenceSequenceViewer sequenceViewer;
  private Coroutine currentRoutine;
  private bool isBinary;
  public void Awake()
  {
    textEditor = new TextEditor(tmpText);
    textSequence = new TextSequence(textEditor);
    morse = new Morse(textEditor);
    sequenceViewer = new SentenceSequenceViewer(textSequence);
  }

}