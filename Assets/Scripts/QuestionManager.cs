using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using System;
using System.Linq;
using Newtonsoft.Json;
using Photon.Pun;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager QM;

    public int Category;
    public int Difficulty;

    bool ended;

    public Dictionary<string, int> P1 = new Dictionary<string, int>();
    public Dictionary<string, int> P2 = new Dictionary<string, int>();
    public Dictionary<string, int> P3 = new Dictionary<string, int>();
    public Dictionary<string, int> P4 = new Dictionary<string, int>();

    public List<Question> questions = new List<Question>();

    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize settings:

        Category = LobbySetUp.LS.category;
        Difficulty = LobbySetUp.LS.difficulty;

        print("Cat: " + Category);
        print("Diff: " + Difficulty);

        ended = false;

        string QuestionUrl = "https://quizguyz.firebaseio.com/Questions/";

        switch (Category)
        {
            case 0:
                QuestionUrl += "Math/";
                break;
            case 1:
                QuestionUrl += "Science/";
                break;
            case 2:
                QuestionUrl += "Geography/";
                break;
            case 3:
                QuestionUrl += "General/";
                break;

        }

        QuestionUrl += (Difficulty).ToString();
        QuestionUrl += ".json";

        // PseudoCode: Get Category and Difficulty from Whichever manager;
        // PseudoCode: Create URL from below to collect correct collection of questions:

        RestClient.Get(url: QuestionUrl).Then(onResolved: response =>
        {
            print("Adding Question");
            questions = JsonConvert.DeserializeObject<List<Question>>(response.Text);

            print("Added Question");

            print("QUESTIONS count");
            print(questions.Count);
        });

        

    }

    public Question getRandomQuestion(int playerIndex)
    {
        Dictionary<string, int> cur = null;
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

        int tempQid = -1;
        int temp = -1;

        while (tempQid == -1 || cur.ContainsKey(tempQid.ToString())) {
            temp = UnityEngine.Random.Range(0, questions.Count);
            tempQid = questions[temp].ID;
        }

        return questions[temp];
    }

    public void recordResponse(int playerIndex, int questionNum, int resp)
    {
        print("Player:" + playerIndex);
        print("Question: " + questionNum);
        print("Response: ");
        print(resp);

        print("Finish Printing");

        switch (playerIndex)
        {
            case 0:
                P1.Add(questionNum.ToString(), resp);
                break;

            case 1:
                P2.Add(questionNum.ToString(), resp);
                break;

            case 2:
                P3.Add(questionNum.ToString(), resp);
                break;

            case 3:
                P4.Add(questionNum.ToString(), resp);
                break;
        }
    }

    public Dictionary<string, int> getResponses(int playerIndex)
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

    public float getTimeLimit()
    {
        switch (Difficulty)
        {
            case 1:
                return 5.0f;
            case 2:
                return 7.0f;
            case 3:
                return 9.0f;
        }
        return 0.0f;
    }

    public void setEnded()
    {
        ended = true;
    }

    public bool isEnded()
    {
        return ended;
    }

}
