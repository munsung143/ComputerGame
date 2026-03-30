using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URPTester : MonoBehaviour
{
    public Material mt;
    public Renderer rd;

  void Start()
  {
    //mt.SetFloat("_MetallicHandler", 1);
    rd.material.SetFloat("_MetallicHandler", 1);
  }
}
