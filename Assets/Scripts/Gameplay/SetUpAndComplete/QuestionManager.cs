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

    public Dictionary<string, int> responses = new Dictionary<string, int>();
    public List<Question> questions = new List<Question>();

    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize settings:

        Category = MapController.Category;
        Difficulty = MapController.Difficulty;

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
            questions = JsonConvert.DeserializeObject<List<Question>>(response.Text);
        });
    }

    public Question getRandomQuestion()
    {
        if (responses.Count == questions.Count)
        {

            return null;
        }

        int tempQid = -1;
        int temp = -1;

        while (tempQid == -1 || responses.ContainsKey(tempQid.ToString())) {
            temp = UnityEngine.Random.Range(0, questions.Count);
            tempQid = questions[temp].ID;
        }

        return questions[temp];
    }

    public void recordResponse(int questionNum, int resp)
    {
        responses.Add(questionNum.ToString(), resp);
    }

    public Dictionary<string, int> getResponses()
    {
        return responses;
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
