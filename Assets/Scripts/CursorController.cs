using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    [SerializeField] TMP_Text textText;
    [SerializeField] TMP_Text textText2;

    [SerializeField] RectTransform screen;
    Camera cam;
    void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        Vector3 input = Input.mousePosition;
        input.z =61 - 3.1f;
        Vector3 s2w = cam.ScreenToWorldPoint(input);
        Vector3 inverse = screen.InverseTransformPoint(s2w);
        float width2 = screen.rect.width / 2;
        float height2 = screen.rect.height / 2;
        float x = inverse.x;
        float y = inverse.y;
        if (x > width2 || x < -width2 || y > height2 || y < -height2) return;
        textText.text = s2w.ToString();
        transform.position = s2w;
        textText2.text = screen.InverseTransformPoint(s2w).ToString();

    }
}
