using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private Button powerButton;
    [SerializeField] private Button startButton;
    [SerializeField] CameraController cameraController;
    [SerializeField] Screen screen;

    void Awake()
    {
        screen.Off();
        powerButton.onClick.AddListener(screen.Toggle);
        startButton.onClick.AddListener(GameStart);
        cameraController.onZoomed.AddListener(ActiveRayCaster);
    }

    public void ActiveRayCaster()
    {
        raycaster.enabled = true;
    }
    public void GameStart()
    {
        raycaster.enabled = false;
        startButton.gameObject.SetActive(false);
        cameraController.GameStartButtonClicked = true;
    }
}
