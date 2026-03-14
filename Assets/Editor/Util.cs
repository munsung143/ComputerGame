using UnityEngine;
using System.IO;
using System;

public static class Util
{
  [UnityEditor.MenuItem("Utilities/Generate TextQuestion Assets")]
  public static void GetnerateTextQuestionAsset()
  {
    string[] allLines = File.ReadAllLines(Application.dataPath + "/CSV/TextQuestion.csv");
    for (int i = 1; i < allLines.Length; i++)
    {
      TextQuestion so = ScriptableObject.CreateInstance<TextQuestion>();
      string[] splitData = allLines[i].Split(',');
      so.sentence = splitData[0].Split('^');
      so.yestText = splitData[1] == "" ? "" : splitData[1];
      so.noText = splitData[2] == "" ? "" : splitData[2];
      so.yesEvent = splitData[3] == "" ? AskingEvent.Next : (AskingEvent)Enum.Parse(typeof(AskingEvent), splitData[3], true);
      so.noEvent = splitData[4] == "" ? AskingEvent.Next : (AskingEvent)Enum.Parse(typeof(AskingEvent), splitData[4], true);
      so.isTrap = splitData[5] == "" ? false : true;
      so.fixedIndex = splitData[6] == "" ? -1 : int.Parse(splitData[6]);
      so.fixChance = splitData[7] == "" ? 1 : float.Parse(splitData[7]);
      so.code = splitData[8];
      so.followingQuestionCode = splitData[9];
      so.requiredQuestionCount = splitData[10] == "" ? -1 : int.Parse(splitData[10]);
      if (splitData[11] != "") so.yesMessage = splitData[11].Split('^');
      if (splitData.Length > 12 && splitData[12] != "") so.noMessage = splitData[12].Split('^');
      UnityEditor.AssetDatabase.CreateAsset(so, $"Assets/Questions/question{i}.asset");
      UnityEditor.AssetDatabase.SaveAssets();
    }
  }
}
