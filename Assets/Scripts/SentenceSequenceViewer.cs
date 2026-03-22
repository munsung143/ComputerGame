using System;
using System.Collections;
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
public class SentenceSequenceViewer
{
  private TextSequence sequence;
  private WaitForSeconds initialTextDelay;
  private WaitForSeconds initialUnderbarDelay;
  private WaitForSeconds currentTextDelay;

  public SentenceSequenceViewer(TextSequence sequence)
  {
    this.sequence = sequence;
    initialTextDelay = new WaitForSeconds(0.03f);
    initialUnderbarDelay = new WaitForSeconds(0.3f);
    currentTextDelay = initialTextDelay;
  }
  public void SetTextDelay(float delay) => currentTextDelay = new WaitForSeconds(delay);

  public IEnumerator PrintTextRaw(string text, string initial)
  {
    return sequence.TextRoutine(
    text,
    currentTextDelay,
    initial,
    initialUnderbarDelay);
  }
}