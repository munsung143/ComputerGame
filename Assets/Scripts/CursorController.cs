using UnityEngine;
using UnityEngine.UI;

public interface ICursorEffectProvider
{
    public void SetColor(Color color);
    public void Reverse(bool val);
    public void Reset();

}

public class CursorController : MonoBehaviour, ICursorEffectProvider
{
    [SerializeField] RawImage image;
    RectTransform screen;
    Camera cam;
    float cameraDepth;
    float halfWidth;
    float halfHelght;
    bool screenOn;

    bool reverse;
    void Awake()
    {
        cam = Camera.main;
        AskingEventRegistry.cursor = this;
    }
    void Update()
    {
        if (screen == null) return;
        Vector3 input = Input.mousePosition;
        input.z = cameraDepth;
        Vector3 i = screen.InverseTransformPoint(cam.ScreenToWorldPoint(input));
        if (i.x > halfWidth || i.x < -halfWidth || i.y > halfHelght || i.y < -halfHelght || !screenOn)
        {
            Cursor.visible = true;
            return;
        }
        Cursor.visible = false;
        if (reverse)
        {
            i.x = -i.x;
            i.y = -i.y;
        }
        transform.localPosition = i;

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

    public void SetColor(Color color)
    {
        image.color = color;
    }
    public void Reverse(bool val)
    {
        reverse = val;
    }
    public void Reset()
    {
        reverse =  false;
        image.color = Color.white;
    }
}
