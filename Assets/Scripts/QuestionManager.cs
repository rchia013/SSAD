using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager QM;

    public int Category;
    public int Difficulty;

    Dictionary<int, int> P1 = new Dictionary<int, int>();
    Dictionary<int, int> P2 = new Dictionary<int, int>();
    Dictionary<int, int> P3 = new Dictionary<int, int>();
    Dictionary<int, int> P4 = new Dictionary<int, int>();

    public List<Question> questions = new List<Question>();

    // Start is called before the first frame update
    void Start()
    {
        // Initialize settings:

        // PseudoCode: Get Category and Difficulty from Whichever manager;
        // PseudoCode: Create URL from below to collect correct collection of questions:

        print("Before Question");

        RestClient.Get<Question>(url: "https://quizguyz.firebaseio.com/Questions/0/1/1.json").Then(onResolved: response =>
        {

            print("Adding Question");

            questions.Add(response);

            print("Added Question");
        });
    }

    public Question getRandomQuestion(int playerIndex)
    {
        Dictionary<int, int> cur = null;
        switch (playerIndex)
        {
            case 1:
                cur = P1;
                break;

            case 2:
                cur = P2;
                break;

            case 3:
                cur = P3;
                break;

            case 4:
                cur = P4;
                break;
        }

        int temp = -1;

        while (temp == -1 && cur.ContainsKey(temp)) {
            temp = Random.Range(0, questions.Count);
        }

        return questions[temp];
    }

    public void recordResponse(int playerIndex, int questionNum, int response)
    {
        switch (playerIndex)
        {
            case 1:
                P1.Add(questionNum, response);
                break;

            case 2:
                P2.Add(questionNum, response);
                break;

            case 3:
                P3.Add(questionNum, response);
                break;

            case 4:
                P4.Add(questionNum, response);
                break;
        }
    }

    public Dictionary<int, int> getResponses(int playerIndex)
    {
        switch (playerIndex)
        {
            case 1:
                return P1;
                break;

            case 2:
                return P2;
                break;

            case 3:
                return P3;
                break;

            case 4:
                return P4;
                break;
        }
        return null;
    }

}
