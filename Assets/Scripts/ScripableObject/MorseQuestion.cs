using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewTextQuestion", menuName = "ScriptableObjects/MorseQuesion", order = 1)]
public class MorseQuestion : Question, IYesNO
{
    public string yestText;
    public string noText;

    public string morseText;
    public string MorseText {get=>morseText; }
    public AskingEvent yesEvent;
    public AskingEvent noEvent;
    public string[] yesMessage;
    public string[] noMessage;

    public string YesText { get => yestText; }
    public string NoText { get => noText; }
    public AskingEvent YesEvent {get=>yesEvent;}
    public AskingEvent NoEvent {get=>noEvent;}
    public string[] YesMessage {get=>yesMessage;}
    public string[] NoMessage {get=>noMessage;}
}
