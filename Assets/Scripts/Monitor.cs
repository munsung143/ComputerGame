using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IMonitorEffectProvider
{
    public void RemovePowerButtonListener();
}

public class Monitor : MonoBehaviour, IMonitorEffectProvider
{
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private Button powerButton;
    [SerializeField] private Button startButton;
    [SerializeField] CameraController cameraController;
    [SerializeField] RectTransform canvasTransform;
    [SerializeField] Screen screen;

    void Awake()
    {
        AskingEventRegistry.monitor = this;
        powerButton.onClick.AddListener(screen.Toggle);
        startButton.onClick.AddListener(GameStart);
        cameraController.onZoomed.AddListener(ActiveRayCaster);
        cameraController.onZoomed.AddListener(SendDistanceToScreen);
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
    private void SendDistanceToScreen()
    {
        screen.SetCursorVariables(cameraController.DistanceBetweenOrigin + canvasTransform.position.z);
        
    }
    public void RemovePowerButtonListener()
    {
        powerButton.onClick.RemoveListener(screen.Toggle);
    }
}
