using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.UI;

public class Morse
{

  public static Dictionary<char, int> MORSE_CODE = new Dictionary<char, int>()
  {
    // A-Z
    {'A', 0b101}, {'B', 0b11000}, {'C', 0b11010}, {'D', 0b1100},
    {'E', 0b10}, {'F', 0b10010}, {'G', 0b1110}, {'H', 0b10000},
    {'I', 0b100}, {'J', 0b10111}, {'K', 0b1101}, {'L', 0b10100},
    {'M', 0b111}, {'N', 0b110}, {'O', 0b1111}, {'P', 0b10110},
    {'Q', 0b11101}, {'R', 0b1010}, {'S', 0b1000}, {'T', 0b11},
    {'U', 0b1001}, {'V', 0b10001}, {'W', 0b1011}, {'X', 0b11001},
    {'Y', 0b11011}, {'Z', 0b11100},

    // 0-9
    {'0', 0b111111},
    {'1', 0b101111},
    {'2', 0b100111},
    {'3', 0b100011},
    {'4', 0b100001},
    {'5', 0b100000},
    {'6', 0b110000},
    {'7', 0b111000},
    {'8', 0b111100},
    {'9', 0b111110},
  };
  private static float unit = 0.3f;

  private TextEditor editor;
  private WaitForSeconds ditLast;
  private WaitForSeconds dashLast;
  private WaitForSeconds delayByCode;
  private WaitForSeconds delayByLetter;
  private WaitForSeconds delayByWord;

  private string signal = "_";

  public Morse(TextEditor editor)
  {
    ditLast = new WaitForSeconds(unit);
    dashLast = new WaitForSeconds(unit * 3);
    delayByCode = new WaitForSeconds(unit);
    delayByLetter = new WaitForSeconds(unit * 3);
    delayByWord = new WaitForSeconds(unit * 7);
    this.editor = editor;
  }
  private void SetSignal()
  {
    editor.SetText(signal);
  }
  private void UnsetSignal()
  {
    editor.SetText("");
  }
  public IEnumerator MorseRoutine(string text)
  {
    while (true)
    {
      for (int i = 0; i < text.Length; i++)
      {
        char t = text[i];
        if (t == ' ')
        {
          yield return delayByWord;
          continue;
        }
        int code = MORSE_CODE[t];
        while (code != 1)
        {
          SetSignal();
          if ((code & 1) == 1)
          {
            yield return dashLast;
          }
          else
          {
            yield return ditLast;
          }
          UnsetSignal();
          code >>= 1;
          if (code == 1 && i == text.Length - 1)
          {
            yield return delayByWord;
          }
          else if (code == 1)
          {
            yield return delayByLetter;
          }
          else
          {
            yield return delayByCode;
          }
        }
      }
    }
  }
}