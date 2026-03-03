using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : MonoBehaviour
{
    private Screen screen;
    private Canvas canvas;
    private Button powerButton;
    void Awake()
    {
        screen = GetComponentInChildren<Screen>();
        canvas = GetComponentInChildren<Canvas>();
        powerButton = canvas.GetComponentInChildren<Button>();
    }

    void Start()
    {
        screen.Hide();
        powerButton.onClick.AddListener(screen.Toggle);
    }
}
