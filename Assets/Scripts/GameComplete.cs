using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using Newtonsoft.Json;
using System.Linq;
using System;

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

    private List<PlayerController> players = new List<PlayerController>();
    private List<Record> records = new List<Record>();

    private QuestionManager QM;

    public GameObject ResultsPage;
    public bool rankProcessed = false;

    // Start is called before the first frame update
    void Start()
    {
        //HARD CODED:
        localID = Login.localid;

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
        if (rankProcessed)
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
                    players.Add(curPlayer.GetComponent<PlayerController>());
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
                players[i].GetComponent<PlayerController>().moveable = false;
          
            }
        
    }

    void createRecords()
    {
        List<PlayerController> rankedPlayers = players.OrderByDescending(o => o.points).ToList();

        string dateTime = System.DateTime.Now.ToString("MM\\/dd\\/yyyy h\\:mm tt");

        for (int i = 0; i < rankedPlayers.Count; i++)
        {
            Record cur = new Record(dateTime,
                QM.Difficulty, QM.Category,
                rankedPlayers[i].GetComponent<PlayerController>().playerName,
                rankedPlayers[i].GetComponent<PlayerController>().getPoints(),
                QM.getResponses(i));

            records.Add(cur);

            if (Login.currentUser.username == rankedPlayers[i].GetComponent<PlayerController>().playerName)
            {
                uploadRecord(cur, i);
            }
        }

        rankProcessed = true;
    }

    void uploadRecord(Record record, int rank)
    {
        string urlRecord = "https://quizguyz.firebaseio.com/Users/" + localID+"/Records.json";
        RestClient.Post(url: urlRecord, JsonConvert.SerializeObject(record));
        print("POSTED!!!!");

        //ACHIEVEMENT POINTS CALCULATION:

        int pointsAwarded = 0;

        switch (rank)
        {
            case 0:
                pointsAwarded = 10;
                break;
            case 1:
                pointsAwarded = 7;
                break;
            case 2:
                pointsAwarded = 4;
                break;
            case 3:
                pointsAwarded = 1;
                break;
        }

        pointsAwarded *= QM.Difficulty;

        updateAchievementPoints(pointsAwarded);

    }

    void updateAchievementPoints(int achievementPoints)
    {
        Achievement playerinfo = new Achievement();
        string playerurl = "https://quizguyz.firebaseio.com/Users/" + localID;
        RestClient.Get(url: playerurl + ".json").Then(onResolved: response =>
        {
            playerinfo = JsonConvert.DeserializeObject<Achievement>(response.Text);
            playerinfo.achievementPoints = playerinfo.achievementPoints + achievementPoints;
            RestClient.Put(url: playerurl + "/achievementPoints.json", JsonConvert.SerializeObject(playerinfo.achievementPoints));
        });
        
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

