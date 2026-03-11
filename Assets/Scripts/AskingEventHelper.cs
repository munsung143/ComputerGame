using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class AskingEventHelper
{
  private static Dictionary<AskingEvent, UnityAction> events = new();


  public static void AddEvent(AskingEvent type, UnityAction action)
  {
    events.Add(type, action);
  }
  public static UnityAction GetEvent(AskingEvent type)
  {
    return events.ContainsKey(type) ? events[type] : null;
  }
  public static void InvokeEvent(AskingEvent type)
  {
    GetEvent(type)?.Invoke();
  }
}