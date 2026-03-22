using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public static class AskingEventRegistry
{
  public static IRedScreenEffectProvider redScreen;
  public static IMonitorEffectProvider monitor;
  public static IQuestionLoopEffectProvider questionLoop;
  public static ITextEffectPrivider sentenceUiViewer;

  public static ITextEffectPrivider askText;


  public static void PlayEvent(AskingEvent type)
  {
    switch (type)
    {
      case AskingEvent.Next: questionLoop.Next(); break;
      case AskingEvent.ForceStop: questionLoop.ForcedStop(); break;
      case AskingEvent.FollowQuestion: questionLoop.Following(); break;
      case AskingEvent.Reset: Reset(); break;
      case AskingEvent.Reverse: questionLoop.ReverseNext(); break;
      case AskingEvent.RedScreen: RedScreenEvent(); break;
      case AskingEvent.GoldenBall: GoldenBallEvent(); break;
      case AskingEvent.Binary: Binary(); break;
      case AskingEvent.HalfReset: HalfReset(); break;
    }
  }

  private static void Reset()
  {
    sentenceUiViewer.ResetBinaryState();
    askText.ResetBinaryState();
    askText.ResetSubject();
    askText.ResetFontSize();
    askText.ResetBinaryState();
    sentenceUiViewer.ResetSubject();
    sentenceUiViewer.ResetFontSize();
    sentenceUiViewer.ResetTextDelay();
    questionLoop.Reset();
  }

  private static void RedScreenEvent()
  {
    monitor.RemovePowerButtonListener();
    redScreen.OnRedScreen();
  }
  private static void GoldenBallEvent()
  {
    sentenceUiViewer.SetGoldenBallSubject();
    askText.SetGoldenBallSubject();
    questionLoop.Next();
  }
  private static void Binary()
  {
    sentenceUiViewer.SetBinaryState();
    askText.SetBinaryState();
    sentenceUiViewer.SetFontSize(0.7f);
    askText.SetFontSize(1);
    sentenceUiViewer.SetTextDelay(0.01f);
    questionLoop.Next();
  }
  private static void HalfReset()
  {
    float r = Random.Range(0, 2);
    if (r == 0)
    {
      Reset();
    }
    else
    {
      questionLoop.Next();
    }
  }
}

public enum AskingEvent
{
  Next,
  ForceStop,
  FollowQuestion,
  Reset,
  RedScreen,
  Reverse,
  GoldenBall,
  Binary,
  HalfReset,
  RedCursor,
  BlurCursor,
  FlipScreen,
  ReverseCursor
}
