using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewTextQuestion", menuName = "ScriptableObjects/TextQuestion", order = 1)]
public class TextQuestion : Question, IYesNO, ISentencePrintable
{
    public string yestText;
    public string noText;

    public string yesEvent;
    public string noEvent;
    public string[] yesMessage;
    public string[] noMessage;
    public string[] sentence;

    public string YesText { get => yestText; }
    public string NoText { get => noText; }
    public string YesEvent {get=>yesEvent;}
    public string NoEvent {get=>noEvent;}
    public string[] YesMessage {get=>yesMessage;}
    public string[] NoMessage {get=>noMessage;}
    public string[] Sentence {get=>sentence;}
}
