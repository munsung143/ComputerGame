using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class SentenceUIViewer
{
  private TextSequence sequence;

  private Coroutine currentSentenceRoutine;
  private WaitForSeconds defaultTextDelayWfs;
  private WaitForSeconds defaultUnderbarDelayWfs;

  public SentenceUIViewer(TextSequence sequence)
  {
    this.sequence = sequence;
    defaultTextDelayWfs = new WaitForSeconds(0.05f);
    defaultUnderbarDelayWfs = new WaitForSeconds(0.3f);
  }

  public void PrintText(string text)
  {
    if (currentSentenceRoutine != null) CoroutineHelper.Stop(currentSentenceRoutine);
    currentSentenceRoutine = CoroutineHelper.Start(sequence.TextRoutine(
    text,
    defaultTextDelayWfs,
    defaultUnderbarDelayWfs));
  }
  public void PrintTextWithIndex(string text, int index)
  {
    if (index == 0)
    {
      PrintText(text);
      return;
    }
    PrintText($"{index}. {text}");
  }

  public void PrintTextWithInitialIndex(string text, int index)
  {
    if (index == 0)
    {
      PrintText(text);
      return;
    }
    if (currentSentenceRoutine != null) CoroutineHelper.Stop(currentSentenceRoutine);
    currentSentenceRoutine = CoroutineHelper.Start(sequence.TextRoutine(
    text,
    defaultTextDelayWfs,
    defaultUnderbarDelayWfs,
    true,
    $"{index}. "));
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