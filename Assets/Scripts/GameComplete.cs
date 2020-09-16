using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameComplete : MonoBehaviour
{
    private string DateTime;
    private int SessionID;

    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    private GameObject[] players = new GameObject[4];
    private List<Record> records = new List<Record>();

    private QuestionManager QM;

    private GameObject ResultsPage;

    // Start is called before the first frame update
    void Start()
    {
        DateTime = null;
        SessionID = 1234;

        player1 = GameObject.FindWithTag("Player1");
        player2 = GameObject.FindWithTag("Player2");
        player3 = GameObject.FindWithTag("Player3");
        player4 = GameObject.FindWithTag("Player4");

        players[0] = player1;
        players[1] = player2;
        players[2] = player3;
        players[3] = player4;

        QM = gameObject.GetComponent<QuestionManager>();

        ResultsPage = GameObject.FindWithTag("GameOver");

        // Create Records

        createRecords();

        // Display UI

        displayResults();

        // Store Records

        storeRecords();
    }

    void createRecords()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {

                //Record cur = new Record("now", 123,
                //    QM.Difficulty, QM.Category,
                //    players[i].GetComponent<Movement>().playerID,
                //    players[i].GetComponent<Movement>().points,
                //    QM.getResponses(i));

                Record cur = new Record("now", 123,
                    0, 0,
                    players[i].GetComponent<Movement>().playerID,
                    players[i].GetComponent<Movement>().getPoints(),
                    QM.getResponses(i));

                records.Add(cur);
            }
        }
    }

    void displayResults()
    {
        // INSERT REUBEN CODE
    }

    void storeRecords()
    {
        for (int i = 0; i < records.Count; i++)
        {
            saveSingleRecord(records[i]);
        }
    }

    void saveSingleRecord(Record record)
    {
        // WRITE TO FIREBASE
    }
}

