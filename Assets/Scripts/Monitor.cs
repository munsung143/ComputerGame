using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private Button powerButton;

    [SerializeField] Screen screen;

    void Awake()
    {
        raycaster.enabled = false;
        screen.Off();
        powerButton.onClick.AddListener(screen.Toggle);
    }

    public void ActiveRayCaster()
    {
        raycaster.enabled = true;
    }
}
