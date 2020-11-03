using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using Newtonsoft.Json;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

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
    bool highscoreDisplayUpdated = false;


    //UI:

    public Image avatar;
    public TextMeshProUGUI rankDisplay;
    public TextMeshProUGUI prevPoints;
    public TextMeshProUGUI pointInc;
    public TextMeshProUGUI afterPoints;

    public GameObject Bronze;
    public GameObject Silver;
    public GameObject Gold;

    public GameObject prevBar;
    public GameObject afterBar;

    //


    // Start is called before the first frame update
    void Start()
    {
        localID = Login.localid;

        // End Gameplay

        initializePlayers();
        stopMoving();

        QM = gameObject.GetComponent<QuestionManager>();
        QM.setEnded();

        // Create Records

        createRecords();

        //display User Avatar:
        AvatarController AC = new AvatarController();

        AC.displayAvatar(avatar, LobbySetUp.LS.playerList[PhotonNetwork.LocalPlayer.NickName]);

    }

    private void Update()
    {
        if (rankProcessed && !highscoreDisplayUpdated)
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

        bool tie = false;
        int prevPoints = -1;
        int offset = 0;
        int rank;

        for (int i = 0; i < rankedPlayers.Count; i++)
        {
            if (i > 0 && rankedPlayers[i].GetComponent<PlayerController>().getPoints() == prevPoints)
            {
                tie = true;
                offset++;
            }

            else
            {
                tie = false;
                offset = 0;
            }

            if (tie)
            {
                rank = i - offset + 1;
            }
            else
            {
                rank = i + 1;
            }

            Record cur = new Record(dateTime,
                QM.Difficulty, QM.Category,
                rankedPlayers[i].GetComponent<PlayerController>().playerName,
                rankedPlayers[i].GetComponent<PlayerController>().getPoints(),
                rank);

            records.Add(cur);

            if (Login.currentUser.username == rankedPlayers[i].GetComponent<PlayerController>().playerName)
            {
                cur.attachResponses(QM.getResponses());

                uploadRecord(cur, rank);
            }

            prevPoints = rankedPlayers[i].GetComponent<PlayerController>().getPoints();
            tie = false;
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

        rankDisplay.text = (rank+1).ToString();

        switch (rank)
        {
            case 1:
                pointsAwarded = 10;
                break;
            case 2:
                pointsAwarded = 7;
                break;
            case 3:
                pointsAwarded = 4;
                break;
            case 4:
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
            pointInc.text = "+" + achievementPoints.ToString();

            //Get
            playerinfo = JsonConvert.DeserializeObject<Achievement>(response.Text);
            prevPoints.text = playerinfo.achievementPoints.ToString();
            prevBar.GetComponent<Image>().fillAmount = ((float)playerinfo.achievementPoints / 300);

            //Update
            playerinfo.achievementPoints = playerinfo.achievementPoints + achievementPoints;
            afterPoints.text = playerinfo.achievementPoints.ToString();
            afterBar.GetComponent<Image>().fillAmount = ((float)playerinfo.achievementPoints / 300);

            //Upload
            RestClient.Put(url: playerurl + "/achievementPoints.json", JsonConvert.SerializeObject(playerinfo.achievementPoints));

            if (playerinfo.achievementPoints >= 100)
            {
                Bronze.SetActive(true);
            }
            if (playerinfo.achievementPoints >= 200)
            {
                Silver.SetActive(true);
            }
            if (playerinfo.achievementPoints >= 300)
            {
                Gold.SetActive(true);
            }
        });
        
    }

    void displayResults()
    {
        highscoreDisplayUpdated = true;

        ResultsPage.GetComponent<HighscoreTable>().enabled = true;

        ResultsPage.GetComponent<HighscoreTable>().endGameUpdateTable(records);

        ResultsPage.SetActive(true);
    }
}

