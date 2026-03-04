using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements.Experimental;

public class CameraController : MonoBehaviour
{

    private Camera mainCamera;
    [SerializeField] private Vector3 endingPosition;
    [SerializeField] float moveSpeed;
    [SerializeField] float tolerance;

    public UnityEvent onZoomed;

    public bool GameStartButtonClicked {get; set;}

    void Awake()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        MoveToEndingPosition();
    }
    private void MoveToEndingPosition()
    {
        if (!GameStartButtonClicked) return;
        if (Vector3.Distance(mainCamera.transform.position, endingPosition) < tolerance)
        {
            mainCamera.transform.position = endingPosition;
            onZoomed.Invoke();
            this.enabled = false;
        }
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, endingPosition, moveSpeed * Time.deltaTime);
    }

}
