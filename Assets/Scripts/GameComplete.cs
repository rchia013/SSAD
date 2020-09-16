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

    private 

    private QuestionManager QM;

    private GameObject ResultsPage;

    // Start is called before the first frame update
    void onEnable()
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
                Record cur = new Record(DateTime, SessionID,
                    QM.Difficulty, QM.Category,
                    players[i].GetComponent<Movement>().playerID,
                    players[i].GetComponent<Movement>().points,
                    QM.getResponses(i));

                records.Add(cur);
            }
        }
    }

    void displayResults()
    {
        //
    }

    void storeRecords()
    {
        for (int i = 0; i < records.Count; i++)
        {
            saveSingleRecord(records[i]);
            print(records[i]);
        }
    }

    void saveSingleRecord(Record record)
    {
        //
    }
}

