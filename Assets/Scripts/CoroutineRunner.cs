using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
  public static MonoBehaviour instance;

  void Awake()
  {
    instance = this;
  }
}