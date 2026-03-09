using UnityEngine;

public class CursorController : MonoBehaviour
{
    RectTransform screen;
    Camera cam;
    float cameraDepth;
    float halfWidth;
    float halfHelght;
    bool screenOn;
    void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (screen == null) return;
        Vector3 input = Input.mousePosition;
        input.z = cameraDepth;
        Vector3 s2w = cam.ScreenToWorldPoint(input);
        Vector3 i = screen.InverseTransformPoint(s2w);
        if (i.x > halfWidth || i.x < -halfWidth || i.y > halfHelght || i.y < -halfHelght || !screenOn)
        {
            Cursor.visible = true;
            return;
        }
        Cursor.visible = false;
        transform.position = s2w;

    }
    public void SetValues(RectTransform trs, float depth)
    {
        screen = trs;
        cameraDepth = depth;
        halfWidth = trs.rect.width / 2;
        halfHelght = trs.rect.height / 2;
    }
    public void GetScreenState(bool isOn)
    {
        this.screenOn = isOn;
    }
}
