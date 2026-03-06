using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewQuestionList", menuName = "ScriptableObjects/QuestionList", order = 1)]
public class QuestionList : ScriptableObject
{
    // 환영 인사 질문을 포함한 게임 클리어 까지의 총 선택 개수
    public int clearQuestionCount = 31;
    public int minTrapCount = 7;
    public List<Question> allQuestions;
    public List<Question> normalQuestions;
    public List<Question> trapQuestions;
    public List<Question> fixedQuestions;
    public Dictionary<string, Question> codedQuestions;

    public Question[] selectedQuestions;

    void OnEnable()
    {
        Init();
        SetQuestionList();
    }

    public void Init()
    {
        normalQuestions = new List<Question>();
        codedQuestions = new Dictionary<string, Question>();
        trapQuestions = new List<Question>();
        fixedQuestions = new List<Question>();
        foreach (Question q in allQuestions)
        {
            if (q.code != "")
            {
                codedQuestions.Add(q.code, q);
                continue;
            }
            if (q.fixedIndex != -1)
            {
                fixedQuestions.Add(q);
                continue;
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
        selectedQuestions = new Question[clearQuestionCount];
        // 최대 질문 개수에서 고정된 질문을 제외한 남은 개수
        int remain = clearQuestionCount;

        //고정된 질문을 미리 질문 배열에 확률에 따라 추가함.
        foreach (Question q in fixedQuestions)
        {
            float r = UnityEngine.Random.Range(0, 1f);
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
        Debug.Log(remain);
        Debug.Log(trapCount);
        Debug.Log(normalCount);
        Debug.Log(selected.Count);
        for (int i = 0, j = 0; i < selectedQuestions.Length || j < selected.Count;)
        {
            Debug.Log($"{i} {j}");
            if (selectedQuestions[i] != null)
            {
                i++;
                continue;
            }
            if (selected[j].requiredQuestionCount != -1 && selected[j].requiredQuestionCount <= i)
            {
                List<int> temp = new();
                for (int k = selected[j].requiredQuestionCount + 1; k < selectedQuestions.Length; k++)
                {
                    if (selectedQuestions[k] == null) temp.Add(k);
                }
                int r = UnityEngine.Random.Range(0, temp.Count);
                selectedQuestions[temp[r]] = selected[j];
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
