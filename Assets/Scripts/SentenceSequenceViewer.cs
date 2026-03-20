using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.LookDev;
using UnityEngine.UI;


public interface ITextEffectPrivider
{
  public void SetGoldenBallSubject();
  public void ResetSubject();
  public void SetBinaryState();
  public void ResetBinaryState();
  public void SetFontSize(float size);
  public void SetTextDelay(float delay);
  public void ResetFontSize();
  public void ResetTextDelay();
}
public class SentenceSequenceViewer : ITextEffectPrivider
{
  private TextSequence sequence;
  private Coroutine currentSentenceRoutine;
  private WaitForSeconds initialTextDelay;
  private WaitForSeconds initialUnderbarDelay;
  private WaitForSeconds currentTextDelay;

  private string subject = "";
  private string postpositions = "";
  private bool isBinary;

  public void SetSubject(string subject, string postpositions)
  {
    this.subject = subject;
    this.postpositions = postpositions;
  }

  public SentenceSequenceViewer(TextSequence sequence)
  {
    this.sequence = sequence;
    initialTextDelay = new WaitForSeconds(0.03f);
    initialUnderbarDelay = new WaitForSeconds(0.3f);
    currentTextDelay = initialTextDelay;
    AskingEventRegistry.sentenceUiViewer = this;
  }
  public void SetFontSize(float size) => sequence.SetFontSize(size);
  public void SetTextDelay(float delay) => currentTextDelay = new WaitForSeconds(delay);

  public void ResetFontSize() => sequence.ResetFontSize();
  public void ResetTextDelay() => currentTextDelay = initialTextDelay;


  public void SetBinaryState()
  {
    isBinary = true;
  }
  public void ResetBinaryState()
  {
    isBinary = false;
  }

  public void SetGoldenBallSubject()
  {
    subject = "금구슬";
    postpositions = "은이을과";
  }
  public void ResetSubject()
  {
    subject = "";
    postpositions = "";
  }
  private string GetTextFormat(string text)
  {
    text = sequence.GetSpecificSubjectedText(text, subject, postpositions);
    if (isBinary) text = sequence.GetBinaryText(text);
    return text;
  }
  public void PrintText(string text)
  {
    text = GetTextFormat(text);
    PrintTextRaw(text, "");
  }
  public void PrintTextWithIndex(string text, int index)
  {
    text = GetTextFormat(text);
    if (index == 0)
    {
      PrintTextRaw(text, "");
      return;
    }
    PrintTextRaw($"{index}. {text}", "");
  }

  public void PrintTextWithInitialIndex(string text, int index)
  {
    text = GetTextFormat(text);
    if (index == 0)
    {
      PrintTextRaw(text, "");
      return;
    }
    PrintTextRaw(text, $"{index}. ");
  }

  public void PrintTextRaw(string text, string initial)
  {
    if (currentSentenceRoutine != null) CoroutineHelper.Stop(currentSentenceRoutine);
    currentSentenceRoutine = CoroutineHelper.Start(sequence.TextRoutine(
    text,
    currentTextDelay,
    initial,
    initialUnderbarDelay));
  }

  public void AddSentenceEndListener(UnityAction action)
  {
    sequence.AddTextEndListner(action);
  }
  public void RemoveSentenceEndListener(UnityAction action)
  {
    sequence.RemoveTextEndListener(action);
  }
}