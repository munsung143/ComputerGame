using UnityEngine;
using UnityEngine.iOS;
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

    int screenWidth;
    int screenHeight;
    float ratio;
    void Awake()
    {
        cam = Camera.main;
        AskingEventRegistry.cursor = this;
        screenWidth = UnityEngine.Device.Screen.width;
        screenHeight = UnityEngine.Device.Screen.height;
        ratio = (float)screenHeight/screenWidth;
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
            float yaw = (input.x / screenWidth - 0.5f) * 20 * ratio;
            float pitch = (input.y / screenHeight - 0.5f) * -20 * ratio;
            //cam.transform.rotation = Quaternion.Euler(new Vector3(pitch, yaw, 0));
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(new Vector3(pitch, yaw, 0)), 0.1f);
        }
        else
        {
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.identity, 0.1f);
            Cursor.visible = false;
            if (reverse)
            {
                i.x = -i.x;
                i.y = -i.y;
            }
            i.z = 0;
            transform.localPosition = i;
        }

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
        reverse = false;
        image.color = Color.white;
    }
}
