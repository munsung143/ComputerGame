using System.Collections.Generic;
using UnityEngine;

public static class WaitForSecondsPool
{
  private static Dictionary<float, WaitForSeconds> pool = new();

  public static WaitForSeconds Get(float seconds)
  {
    pool.TryGetValue(seconds, out WaitForSeconds wfs);
    if (wfs == null)
    {
      wfs = new WaitForSeconds(seconds);
      pool.Add(seconds, wfs);
    }
    return wfs;
  } 
}