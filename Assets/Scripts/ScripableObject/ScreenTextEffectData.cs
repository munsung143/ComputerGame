using System;
using System.Text;
using TMPro;
using UnityEngine;

public interface IScreenTextEffectProvider
{
  public void SetSubject(string subject, string postpositions);
  public void SetBinaryState(bool b);
  public void SetFontMult(float size);
  public void SetTextDelayMult(float delay);
  public void Reset();
}
[CreateAssetMenu(fileName = "ScreenEffect", menuName = "ScriptableObjects/ScreenEffect", order = 1)]
public class ScreenTextEffectData : ScriptableObject, IScreenTextEffectProvider
{
  public string subject;
  public string postpositions;
  public bool isBinary;
  public float textSpeedMult;
  public float fontSizeMult;

 [NonSerialized] public Action<float> onFontMultSet;

  void OnEnable()
  {
    Reset();
    AskingEventRegistry.screenText = this;
  }
  public void Reset()
  {
    subject = "";
    postpositions = "";
    isBinary = false;
    textSpeedMult = 1;
    SetFontMult(1);
  }
  public void SetSubject(string subject, string postpositions)
  {
    this.subject = subject;
    this.postpositions = postpositions;
  }
  public void SetFontMult(float value)
  {
    this.fontSizeMult = value;
    onFontMultSet?.Invoke(fontSizeMult);
  }
  public void SetBinaryState(bool b)
  {
    isBinary = b;
  }
  public void SetTextDelayMult(float value)
  {
    textSpeedMult = value;
  }
  public string GetFormattedText(string text)
  {
    text = GetBinaryText(text);
    text = GetSpecificSubjectedText(text);
    return text;
  }

  public string GetBinaryText(string text)
  {
    if (!isBinary) return text;
    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < text.Length; i++)
    {
      for (int shift = (int)Math.Log(text[i], 2); shift >= 0; shift--)
      {
        sb.Append((text[i] >> shift) & 1);
      }
      sb.Append(' ');
    }
    return sb.ToString();
  }
  public string GetSpecificSubjectedText(string text)
  {
    StringBuilder sb = new StringBuilder();
    bool inSharp = false;
    for (int i = 0; i < text.Length; i++)
    {
      if (text[i] == '#')
      {
        if (subject == "")
        {
          continue;
        }
        inSharp = !inSharp;
        if (inSharp) sb.Append(subject);
        else
        {
          i++;
          if (i >= text.Length) break;
          char p = text[i];
          if (p == '은' || p == '는') sb.Append(postpositions[0]);
          else if (p == '이' || p == '가') sb.Append(postpositions[1]);
          else if (p == '을' || p == '를') sb.Append(postpositions[2]);
          else if (p == '와' || p == '과') sb.Append(postpositions[3]);
          else i--;
        }
        continue;
      }
      if (inSharp) continue;
      sb.Append(text[i]);
    }
    return sb.ToString();
  }
}