using TMPro;
using UnityEngine;

public class TextEditor
{
  private TMP_Text tmpText;
  private float initialFontSize;
  private Color initialColor;
  public TextEditor(TMP_Text targetText)
  {
    this.tmpText = targetText;
    initialFontSize = tmpText.fontSize;
    initialColor = tmpText.color;
  }
  public void SetText(string text)
  {
    tmpText.text = text;
  }
  public void ClearText()
  {
    tmpText.text = "";
  }
  public void SetFontSize(float size)
  {
    tmpText.fontSize = size;
  }
  public void ResetFontSize()
  {
    tmpText.fontSize = initialFontSize;
  }
  public void SetColor(Color color)
  {
    tmpText.color = color;
  }
  public void ResetColor()
  {
    tmpText.color = initialColor;
  }
}