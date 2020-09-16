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

        //print("Before Question");

        //RestClient.Get<Question>(url: "https://quizguyz.firebaseio.com/Questions/0/1/1.json").Then(onResolved: response =>
        //{

        //    print("Adding Question");

        //    questions.Add(response);

        //    print("Added Question");
        //});

        Question test = new Question(1, 1, 1, "2 + 2 = ",
        new Dictionary<string, bool>(){
            {"4",true },
            {"5", false },
            {"6", false },
            {"7", false } });

        Question test2 = new Question(2, 1, 1, "2 + 2 = ",
        new Dictionary<string, bool>(){
            {"4",true },
            {"5", false },
            {"6", false },
            {"7", false } });

        Question test3 = new Question(3, 1, 1, "2 + 2 = ",
        new Dictionary<string, bool>(){
            {"4",true },
            {"5", false },
            {"6", false },
            {"7", false } });

        Question test4 = new Question(4, 1, 1, "2 + 2 = ",
        new Dictionary<string, bool>(){
            {"4",true },
            {"5", false },
            {"6", false },
            {"7", false } });

        questions.Add(test);
        questions.Add(test2);
        questions.Add(test3);
        questions.Add(test4);

        print(questions.Count);

    }

    public Question getRandomQuestion(int playerIndex)
    {
        Dictionary<int, int> cur = null;
        switch (playerIndex)
        {
            case 0:
                cur = P1;
                break;

            case 1:
                cur = P2;
                break;

            case 2:
                cur = P3;
                break;

            case 3:
                cur = P4;
                break;
        }

        int temp = -1;

        while (temp == -1 || cur.ContainsKey(temp)) {
            temp = Random.Range(0, questions.Count);
        }

        print(temp);

        return questions[temp];
    }

    public void recordResponse(int playerIndex, int questionNum, int response)
    {
        switch (playerIndex)
        {
            case 0:
                P1.Add(questionNum, response);
                break;

            case 1:
                P2.Add(questionNum, response);
                break;

            case 2:
                P3.Add(questionNum, response);
                break;

            case 3:
                P4.Add(questionNum, response);
                break;
        }
    }

    public Dictionary<int, int> getResponses(int playerIndex)
    {
        switch (playerIndex)
        {
            case 0:
                return P1;
                break;

            case 1:
                return P2;
                break;

            case 2:
                return P3;
                break;

            case 3:
                return P4;
                break;
        }
        return null;
    }

}
