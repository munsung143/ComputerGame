using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] CameraController cameraController;

    void Awake()
    {
        startButton.onClick.AddListener(GameStart);
    }
    public void GameStart()
    {
        startButton.gameObject.SetActive(false);
        cameraController.GameStartButtonClicked = true;
    }
}
