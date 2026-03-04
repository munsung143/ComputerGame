using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QestionList", menuName = "ScriptableObjects/QuestionList", order = 1)]
public class QuestionListSO : ScriptableObject
{
    public List<Question> list;
}

[Serializable]
public struct Question
{
    public string[] sentences; // 질문 내용들
    public string yesString; // YES 대신 출력할 것
    public string noString; // NO 대신 출력할 것
    public int weight; // 가중치
    public int onlyAfter; // 최소 이 정도의 질문을 받은 후 출력
    public string yesEvent; // Yes 클릭 시 이벤트
    public string noEvent; // No 클릭 시 이벤트


}
