using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Camera mainCamera;
    [SerializeField] private Vector3 endingPosition;
    [SerializeField] float moveSpeed = 0.01f;
    [SerializeField] float tolerance = 0.1f;

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
        if (Vector3.Distance(mainCamera.transform.position, endingPosition) < tolerance)
        {
            return;
        }
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, endingPosition, moveSpeed);
    }

}
