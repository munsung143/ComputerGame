using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewTextQuestion", menuName = "ScriptableObjects/TextQuestion", order = 1)]
public class TextQuestion : Question, IYesNO, ISentencePrintable
{
    public string yestText;
    public string noText;

    public AskingEvent yesEvent;
    public AskingEvent noEvent;
    public string[] yesMessage;
    public string[] noMessage;
    public string[] sentence;

    public string YesText { get => yestText; }
    public string NoText { get => noText; }
    public AskingEvent YesEvent {get=>yesEvent;}
    public AskingEvent NoEvent {get=>noEvent;}
    public string[] YesMessage {get=>yesMessage;}
    public string[] NoMessage {get=>noMessage;}
    public string[] Sentence {get=>sentence;}
}
