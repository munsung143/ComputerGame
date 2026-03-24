using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Sentence : MonoBehaviour
{
  [SerializeField] TMP_Text tmpText;
  private Morse morse;
  private TextSeq textSequence;
  private TextEditor textEditor;
  private Coroutine currentRoutine;
  public ScreenTextEffectData effectData;

  private float textDelay = 0.03f;
  private float underbarDelay = 0.3f;

  private float initialFontSize;
  public void Awake()
  {
    textEditor = new TextEditor(tmpText);
    textSequence = new TextSeq(tmpText);
    morse = new Morse(textEditor);
    initialFontSize = tmpText.fontSize;
  }
  void Start()
  {
    effectData.onFontMultSet += SetFontSize;
  }

  public void AddSentenceEndListener(UnityAction action)
  {
    textSequence.AddTextEndListner(action);
  }
  public void RemoveSentenceEndListener(UnityAction action)
  {
    textSequence.RemoveTextEndListener(action);
  }
  public void SetFontSize(float mult)
  {
    tmpText.fontSize = mult * initialFontSize;
  }

  public void PrintAnswer(string text)
  {
    text = effectData.GetFormattedText(text);
    PrintTextRaw(text, "");
  }
  public void PrintSentence(string text, int index, bool isFirst)
  {
    text = effectData.GetFormattedText(text);
    if (index == 0)
    {
      PrintTextRaw(text, "");
      return;
    }
    if (isFirst)
    {
      PrintTextRaw($"{index}. {text}", "");
    }
    else
    {
      PrintTextRaw(text, $"{index}. ");
    }
  }

  private void PrintTextRaw(string text, string initial)
  {
    if (currentRoutine != null) StopCoroutine(currentRoutine);
    currentRoutine = StartCoroutine(textSequence.TextRoutine(
      text,
      WaitForSecondsPool.Get(effectData.textSpeedMult * textDelay),
      initial,
      WaitForSecondsPool.Get(effectData.textSpeedMult * underbarDelay)));
  }

  public void PrintMorse(string text)
  {
    if (currentRoutine != null) StopCoroutine(currentRoutine);
    currentRoutine = StartCoroutine(morse.MorseRoutine(text));
  }

}