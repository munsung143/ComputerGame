using System.Collections.Generic;
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
  }
}