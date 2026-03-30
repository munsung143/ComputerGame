using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public static class AskingEventRegistry
{
  public static IRedScreenEffectProvider redScreen;
  public static IMonitorEffectProvider monitor;
  public static IQuestionLoopEffectProvider questionLoop;
  public static IScreenTextEffectProvider screenText;
  public static IScreenEffectProvider screen;
  public static ICursorEffectProvider cursor;


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
      case AskingEvent.FlipScreen: FlipScreen(); break;
      case AskingEvent.CursorRed : SetCursorRed(); break;
      case AskingEvent.CursorBlue : SetCursorBlue(); break;
      case AskingEvent.TextDelay3x: TextDelay3x(); break;
      case AskingEvent.ReverseCursor: ReverseCursor(); break;
      case AskingEvent.Cake: questionLoop.Next(); break;
    }
  }

  private static void Reset()
  {
    screenText.Reset();
    screen.Rotation(0);
    cursor.Reset();
    questionLoop.Reset();
  }

  private static void RedScreenEvent()
  {
    monitor.RemovePowerButtonListener();
    redScreen.OnRedScreen();
  }
  private static void GoldenBallEvent()
  {
    //screenText.SetSubject("금구슬", "은이을과");
    screenText.SetSubject("당근", "은이을과");
    questionLoop.Next();
  }
  private static void Binary()
  {
    screenText.SetBinaryState(true);
    screenText.MultTextDelayConst(0.1f);
    screenText.SetFontMult(0.5f);
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
  private static void ReverseCursor()
  {
    cursor.Reverse(true);
    questionLoop.Next();
  }
  private static void FlipScreen()
  {
    screen.Rotation(180);
    questionLoop.Next();
  }
  private static void SetCursorRed()
  {
    cursor.SetColor(Color.red);
    questionLoop.Next();
  }
  private static void SetCursorBlue()
  {
    cursor.SetColor(Color.blue);
    questionLoop.Next();
  }
  private static void TextDelay3x()
  {
    screenText.MultTextDelayConst(3);
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
  GoldenBall,
  Binary,
  HalfReset,
  CursorRed,
  CursorBlue,
  FlipScreen,
  ReverseCursor,
  TextDelay3x,
  Cake
}
