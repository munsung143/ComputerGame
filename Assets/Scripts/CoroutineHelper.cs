using System.Collections;
using System.Diagnostics;
using UnityEngine;

public static class CoroutineHelper
{
  public static Coroutine Start(IEnumerator routine)
  {
    return CoroutineRunner.instance.StartCoroutine(routine);
  }
  public static void Stop(Coroutine coroutine)
  {
    CoroutineRunner.instance.StopCoroutine(coroutine);
  }
}