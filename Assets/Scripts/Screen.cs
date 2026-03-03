using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour
{
    private Canvas canvas;
    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
    }
    public void Hide()
    {
        canvas.enabled = false;
    }
    public void Toggle()
    {
        canvas.enabled = !canvas.enabled;
    }
}
