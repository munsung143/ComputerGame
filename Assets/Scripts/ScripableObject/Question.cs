using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AskingEvent
{
    Next,
    ForceStop,
    FollowQuestion,
    Reset,
    BlueScreen
}


public interface IYesNO
{
    public string YesText { get; }
    public string NoText { get; }
    public AskingEvent YesEvent { get; }
    public AskingEvent NoEvent { get; }
    public string[] YesMessage { get; }
    public string[] NoMessage { get; }
}
public interface ISentencePrintable
{
    public string[] Sentence { get; }
}

public class Question : ScriptableObject
{
    public string code;
    public int requiredQuestionCount = -1;
    public string followingQuestionCode;
    public int fixedIndex = -1;
    public float fixChance = 1;
    public bool isTrap;

}
