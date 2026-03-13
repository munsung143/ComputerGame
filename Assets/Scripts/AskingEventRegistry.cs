using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public static class AskingEventRegistry
{
  public static IRedScreenEffectProvider RedScreen;
  public static IMonitorEffectProvider Monitor;
  public static IQuestionLoopEffectProvider QuestionLoop;


  public static void PlayEvent(AskingEvent type)
  {
    switch (type)
    {
      case AskingEvent.Next: QuestionLoop.Next(); break;
      case AskingEvent.ForceStop: QuestionLoop.ForcedStop(); break;
      case AskingEvent.FollowQuestion: QuestionLoop.Following(); break;
      case AskingEvent.Reset: QuestionLoop.Reset(); break;
      case AskingEvent.Reverse: QuestionLoop.ReverseNext(); break;
      case AskingEvent.RedScreen: RedScreenEvent(); break;
    }
  }

  private static void RedScreenEvent()
  {
    Monitor.RemovePowerButtonListener();
    RedScreen.OnRedScreen();
  }
}

public enum AskingEvent
{
    Next,
    ForceStop,
    FollowQuestion,
    Reset,
    RedScreen,
    Reverse
}
