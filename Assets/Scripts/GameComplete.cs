using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using Newtonsoft.Json;
using UnityEditor;

public class GameComplete : MonoBehaviour
{
    private string DateTime;
    private int SessionID;

    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;
    bool initialized = false;

    public static string localID;

    private List<GameObject> players = new List<GameObject>();
    private List<Record> records = new List<Record>();

    private QuestionManager QM;

    public GameObject ResultsPage;
    public bool display = false;

    // Start is called before the first frame update
    void Start()
    {
        //HARD CODED:
        localID = Login.localid;
        DateTime = null;
        SessionID = 1234;

        // End Gameplay

        initializePlayers();
        stopMoving();

        QM = gameObject.GetComponent<QuestionManager>();
        QM.setEnded();

        // Create Records

        createRecords();
    }

    private void Update()
    {
        if (display)
        {
            // Display Ranking UI

            displayResults();
        }
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

            print(cur.playerName);

            uploadRecord(players[i].GetComponent<Movement>().playerID, cur);
        }
    }

    void uploadRecord(string playerID, Record record)
    {

        print("localID = " + localID);
        string urlString = "https://quizguyz.firebaseio.com/Users/" + localID+"/Records.json";
        RestClient.Post(url: urlString, JsonConvert.SerializeObject(record));
        print("POSTED!!!!");
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

