using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenViewer : MonoBehaviour
{

  private object _locker = new object();
  private bool isPingTurn = true;


  // ping pong ping pong ping pong ping pong ping pong
  public void Start()
  {
    Thread pingth = new Thread(Ping);
    Thread pongth = new Thread(Pong);
    pingth.Start();
    pongth.Start();
  }

  // 0. ping 스레드가 먼저 lock을 얻는다고 가정.
  private void Ping()
  {
    lock (_locker)
    {
      for (int i = 0; i < 5; i++)
      {
        while (!isPingTurn)
        {
          // 2. 첫 ping호출 후 락을 해제함과 동시에 현 스레드를 대기 큐에 넣어 대기한다.
          // 락이 해제되었으므로, 대기 중이던 pong스레드가 호출되기 시작한다.

          // 5. pong 스레드에서 isPingTurn을 true로 하였으므로 루프에서 나가게 된다.
          // 이후 동일한 작업을 반복한다.
          Monitor.Wait(_locker);
        }
        // 1. 첫 반복에서 ping을 출력, 이후 isPingTurn을 false로 전환하고
        // Pulse를 통해 대기 큐의 스레드를 준비 큐로 옮긴다.
        // 없을 경우 무시됨
        Debug.Log("ping");
        isPingTurn = false;
        Monitor.Pulse(_locker);
      }
    }
  }
  private void Pong()
  {
    lock (_locker)
    {
      for (int i = 0; i < 5; i++)
      {
        while (isPingTurn)
        {
          // 4. 락을 해제하고 자신을 대기 큐에 넣어 대기한다.
          // 직후 준비 큐의 ping 스레드가 락을 받게 된다.
          Monitor.Wait(_locker);
        }
        // 3. pong 출력 후, 대기 큐를 펄스 시킨다.
        // 위에서 대기 큐에 들어갔던 ping 스레드가 락이 해제되고 제어권을 얻을 때 까지 준비 큐에서 대기하게 된다.
        Debug.Log("pong");
        isPingTurn = true;
        Monitor.Pulse(_locker);
      }
    }
  }

  public void Temp1()
  {
    Debug.Log(1);
    Thread tr2 = new Thread(Temp2);
    tr2.Start();
    tr2.Join();
    Debug.Log(3);
  }
  public void Temp2() => Debug.Log(2);
}