using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextButton : MonoBehaviour
{
  [SerializeField] Button button;

  public void EnableButton()
  {
    if (button != null) button.enabled = true;
  }
  public void DisableButton()
  {
    if (button != null) button.enabled = false;
  }
  public void AddButtonListener(UnityAction action)
  {
    if (button == null) return;
    button.onClick.AddListener(action);
  }
  public void RemoveButtonListener(UnityAction action)
  {
    if (button == null) return;
    button.onClick.RemoveListener(action);
  }
}