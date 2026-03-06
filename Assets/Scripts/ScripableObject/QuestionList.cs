using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionList : ScriptableObject
{
    // 환영 인사 질문을 포함한 총 개수
    int maxQuestionCount = 31;
    int minTrapCount = 7;
    public List<Question> allQuestions;
    public List<Question> normalQuestions;
    public List<Question> trapQuestions;
    public List<Question> fixedQuestions;
    public Dictionary<string, Question> codedQuestions;

    public Question[] selectedQuestions;

    void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        normalQuestions = new();
        codedQuestions = new();
        trapQuestions = new();
        fixedQuestions = new();
        foreach (Question q in allQuestions)
        {
            if (q.code != "")
            {
                codedQuestions.Add(q.code, q);
                return;
            }
            if (q.fixedIndex != -1)
            {
                fixedQuestions.Add(q);
                return;
            }
            if (q.isTrap)
            {
                trapQuestions.Add(q);
            }
            else
            {
                normalQuestions.Add(q);
            }

        }
    }

    public void SetQuestionList()
    {
        selectedQuestions = new Question[maxQuestionCount];
        // 최대 질문 개수에서 고정된 질문을 제외한 남은 개수
        int remain = maxQuestionCount;

        //고정된 질문을 미리 질문 배열에 확률에 따라 추가함.
        foreach (Question q in fixedQuestions)
        {
            float r = UnityEngine.Random.Range(0, 1);
            if (q.fixChance >= r)
            {
                selectedQuestions[q.fixedIndex] = q;
                remain--;
            }
        }
        // 남은 개수 중 함정 질문의 개수와 일반 질문의 개수
        int trapCount = UnityEngine.Random.Range(minTrapCount, remain + 1);
        int normalCount = remain - trapCount;

        // 일반 및 함정 질문 리스트에서 중복 없이 위에서 정해진 수 만큼 선정 후 임시 리스트에 담음.
        // 이후 해당 임시 리스트 전체 셔플
        List<Question> selected = new();
        for (int i = normalQuestions.Count - 1; i > normalQuestions.Count - 1 - normalCount; i--)
        {
            int s = UnityEngine.Random.Range(0, i + 1);
            selected.Add(normalQuestions[s]);
            Question temp = normalQuestions[i];
            normalQuestions[i] = normalQuestions[s];
            normalQuestions[s] = temp;
        }
        for (int i = trapQuestions.Count - 1; i > trapQuestions.Count - 1 - trapCount; i--)
        {
            int s = UnityEngine.Random.Range(0, i + 1);
            selected.Add(trapQuestions[s]);
            Question temp = trapQuestions[i];
            trapQuestions[i] = trapQuestions[s];
            trapQuestions[s] = temp;
        }
        for (int i = selected.Count - 1; i > -1; i--)
        {
            int s = UnityEngine.Random.Range(0, i + 1);
            Question temp = selected[i];
            selected[i] = selected[s];
            selected[s] = temp;
        }

        // 선택 질문 배열을 순회하며, 임시 리스트의 요소들을 차례로 넣음. 이때
        // 질문의 요구 개수가 현재 인덱스 이상이라면, 그보다 1이상 인덱스에 질문을 위치시켜야 함.
        for (int i = 0, j = 0; i < selectedQuestions.Length;)
        {
            if (selectedQuestions[i] != null)
            {
                i++;
                continue;
            }
            if (selected[j].requiredQuestionCount <= i)
            {
                int r = UnityEngine.Random.Range(selected[j].requiredQuestionCount + 1, selectedQuestions.Length);
                selectedQuestions[r] = selected[j];
            }
            else
            {
                selectedQuestions[i] = selected[j];
                i++;
            }
            j++;
        }
    }
}
