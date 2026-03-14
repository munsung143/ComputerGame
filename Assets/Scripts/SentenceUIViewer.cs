using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public interface ISentenceUiViewerEffectPrivider
{
  public void SetGoldenBall();
}
public class SentenceUIViewer : ISentenceUiViewerEffectPrivider
{
  private TextSequence sequence;

  private Coroutine currentSentenceRoutine;
  private WaitForSeconds defaultTextDelayWfs;
  private WaitForSeconds defaultUnderbarDelayWfs;

  private string subject = "";
  private string postpositions = "";

  private bool isbinary;

  public void SetSubject(string subject, string postpositions)
  {
    this.subject = subject;
    this.postpositions = postpositions;
  }

  public SentenceUIViewer(TextSequence sequence)
  {
    this.sequence = sequence;
    defaultTextDelayWfs = new WaitForSeconds(0.05f);
    defaultUnderbarDelayWfs = new WaitForSeconds(0.3f);
    AskingEventRegistry.sentenceUiViewer = this;
  }

  public void SetGoldenBall()
  {
    subject = "금구슬";
    postpositions = "은이을과";
  }
  public void PrintText(string text)
  {
    PrintTextRaw(text, "");
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
    PrintTextRaw(text, $"{index}. ");
  }

  public void PrintTextRaw(string text, string initial)
  {
    string result = "";
    if (subject == "")
    {
      result = text;
    }
    else
    {
      bool sharp = false;
      for (int i = 0; i < text.Length; i++)
      {
        if (text[i] == '#')
        {
          sharp = !sharp;
          if (sharp) result = $"{result}{subject}";
          else
          {
            char p = text[++i];
            if (p == '은' || p == '는') result.Append(postpositions[0]);
            else if (p == '이' || p == '가') result.Append(postpositions[1]);
            else if (p == '을' || p == '를') result.Append(postpositions[2]);
            else if (p == '와' || p == '과') result.Append(postpositions[3]);
            else i--;
          }
          continue;
        }
        if (sharp) continue;
        result.Append(text[i]);
      }
    }
    if (currentSentenceRoutine != null) CoroutineHelper.Stop(currentSentenceRoutine);
    currentSentenceRoutine = CoroutineHelper.Start(sequence.TextRoutine(
    result,
    defaultTextDelayWfs,
    defaultUnderbarDelayWfs,
    true,
    initial));
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