using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using System;
using System.Linq;
using Newtonsoft.Json;

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

        RestClient.Get(url: "https://quizguyz.firebaseio.com/Questions/0/1.json").Then(onResolved: response =>
        {
            print("Adding Question");
            questions = JsonConvert.DeserializeObject<List<Question>>(response.Text);

            print("Added Question");
        });

        print("QUESTIONS count");
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

        if (cur.Count == questions.Count)
        {
            print("NO MORE QUESTIONS!");

            return null;
        }

        int temp = -1;
        Dictionary<int, int>.KeyCollection key = cur.Keys;

        while (temp == -1 || cur.ContainsKey(temp)) {
            if (cur.ContainsKey(temp))
            {
                Dictionary<int, int>.KeyCollection keys = cur.Keys;
            }
            temp = UnityEngine.Random.Range(0, questions.Count);
            print(temp);
        }

        print(temp);

        return questions[temp];
    }

    public void recordResponse(int playerIndex, int questionNum, int response)
    {
        switch (playerIndex)
        {
            case 0:
                P1.Add(questionNum-1, response);
                break;

            case 1:
                P2.Add(questionNum-1, response);
                break;

            case 2:
                P3.Add(questionNum-1, response);
                break;

            case 3:
                P4.Add(questionNum-1, response);
                break;
        }
    }

    public Dictionary<int, int> getResponses(int playerIndex)
    {
        switch (playerIndex)
        {
            case 0:
                return P1;

            case 1:
                return P2;

            case 2:
                return P3;

            case 3:
                return P4;
        }
        return null;
    }

}
