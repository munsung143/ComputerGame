using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button powerButton;

    [SerializeField] GameObject screen;

    void Start()
    {
        OffScreen();
        powerButton.onClick.AddListener(ToggleScreen);
    }

    private void OffScreen()
    {
        screen.SetActive(false);
    }

    private void ToggleScreen()
    {
        screen.SetActive(!screen.activeSelf);
    }
}
