using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using Newtonsoft.Json;

public class GameComplete : MonoBehaviour
{
    private string DateTime;
    private int SessionID;

    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;
    bool initialized = false;

    private List<GameObject> players = new List<GameObject>();
    private List<Record> records = new List<Record>();

    private QuestionManager QM;

    public GameObject ResultsPage;

    // Start is called before the first frame update
    void Start()
    {
        //HARD CODED:

        DateTime = null;
        SessionID = 1234;

        // End Gameplay

        initializePlayers();
        stopMoving();

        QM = gameObject.GetComponent<QuestionManager>();

        // Create Records

        createRecords();

        // Display Ranking UI

        displayResults();
    }

    void initializePlayers()
    {
        if (!initialized)
        {
            for (int i = 1; i < 5; i++)
            {
                GameObject curPlayer = GameObject.FindWithTag("Player" + i.ToString());

                if (curPlayer != null)
                {
                    players.Add(curPlayer);
                }
                else
                {
                    break;
                }
            }

            print("Player List Size: " + players.Count);
            initialized = true;
        }
    }

    public void stopMoving()
    {
            initializePlayers();
            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<Movement>().moveable = false;
            }
        
    }

    void createRecords()
    {
        for (int i = 0; i < players.Count; i++)
        {
            //Record cur = new Record("now", 123,
            //    QM.Difficulty, QM.Category,
            //    players[i].GetComponent<Movement>().playerName,
            //    players[i].GetComponent<Movement>().points,
            //    QM.getResponses(i));

            Record cur = new Record("now", 123,
                0, 0,
                players[i].GetComponent<Movement>().playerName,
                players[i].GetComponent<Movement>().getPoints(),
                QM.getResponses(i));

            records.Add(cur);

            uploadRecord(players[i].GetComponent<Movement>().playerID, cur);
        }
    }

    void uploadRecord(int playerID, Record record)
    {
        // PSEUDOCODE:

        //// Retrive user entry
        //// Cast user entry to user object type?
        //// append record to user entry -> records
        //// upload user entry to update existing one

        //string urlString = "https://quizguyz.firebaseio.com/users/" + playerID.ToString();

        //RestClient.Post(url: urlString, JsonConvert.SerializeObject(record));
    }

    void displayResults()
    {
        print("display Results");
        print(records.Count);

        ResultsPage.GetComponent<HighscoreTable>().enabled = true;

        ResultsPage.GetComponent<HighscoreTable>().endGameUpdateTable(records);

        ResultsPage.SetActive(true);
    }
}

