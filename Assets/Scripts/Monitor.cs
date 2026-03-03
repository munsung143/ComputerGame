using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button powerButton;

    [SerializeField] Screen screen;

    void Awake()
    {
        screen.Off();
        powerButton.onClick.AddListener(screen.Toggle);
    }
}
