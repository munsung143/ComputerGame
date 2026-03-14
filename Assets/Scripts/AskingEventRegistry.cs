using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public static class AskingEventRegistry
{
  public static IRedScreenEffectProvider redScreen;
  public static IMonitorEffectProvider monitor;
  public static IQuestionLoopEffectProvider questionLoop;
  public static ISentenceUiViewerEffectPrivider sentenceUiViewer;


  public static void PlayEvent(AskingEvent type)
  {
    switch (type)
    {
      case AskingEvent.Next: questionLoop.Next(); break;
      case AskingEvent.ForceStop: questionLoop.ForcedStop(); break;
      case AskingEvent.FollowQuestion: questionLoop.Following(); break;
      case AskingEvent.Reset: questionLoop.Reset(); break;
      case AskingEvent.Reverse: questionLoop.ReverseNext(); break;
      case AskingEvent.RedScreen: RedScreenEvent(); break;
      case AskingEvent.GoldenBall: GoldenBallEvent(); break;

    }
  }

  private static void RedScreenEvent()
  {
    monitor.RemovePowerButtonListener();
    redScreen.OnRedScreen();
  }
  private static void GoldenBallEvent()
  {
    sentenceUiViewer.SetGoldenBall();
    questionLoop.Next();
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
  GoldenBall
}
