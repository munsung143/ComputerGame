using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;
using System.Reflection;

public class ScreenViewer : MonoBehaviour
{

  private object _locker = new object();
  private bool isPingTurn = true;

  public void Start()
  {
    AppDomain appDomain = AppDomain.CurrentDomain;
    // 도메인 순회
    foreach (Assembly asm in appDomain.GetAssemblies())
    {
      // 어셈블리 순회
      // GetTypes()를 통해 바로 타입들을 불러올 수도 있음
      foreach(Module mod in asm.GetModules())
      {
        // 모듈 순회
        foreach (Type type in mod.GetTypes())
        {
          
        }
      }
    }
  }
}

public static class Intadb
{
  public static int AddTwo(this Int32 i, int a, int b)
  {
    return i + a + b;
  }
}