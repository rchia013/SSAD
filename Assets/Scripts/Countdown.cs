using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Photon.Pun;
using System;
using Photon.Realtime;


public class Countdown : MonoBehaviour
{

    float currentTime = 0f;
    float startingTime = 50.49999f;
    float totalTime;
    double minutes;
    string min;
    string sec;
    float seconds;
    bool start;

    GameObject GameController;
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;


    [SerializeField] TextMeshProUGUI countdown;
    public GameObject highScoreTable;


    private void Start()
    {

        currentTime = startingTime;
        countdown.text = timeToString(currentTime);
        bool start = false;
        

        GameController = GameObject.FindWithTag("GameController");
    }

    private void Update()
    {

        StartCoroutine(Delay());
        if (start)
        {
            if (currentTime >= 10)
            {
                currentTime -= 1 * Time.deltaTime;

                countdown.text = timeToString(currentTime);
            }
            else if (currentTime < 10 && currentTime > 0)
            {
                currentTime -= 1 * Time.deltaTime;

                countdown.text = timeToString(currentTime);
                countdown.color = Color.red;

                countdown.fontSize = getFontSize(currentTime);
            }
            else if (currentTime <= 0)
            {
                countdown.fontSize = 60;
                countdown.text = "Game Over";
                countdown.color = Color.red;
                countdown.fontStyle = FontStyles.Bold;
                
                // End Game Sequence

                StartCoroutine(EndGame());
            }

        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        start = true;
    }



    IEnumerator EndGame()
    {
        stopMoving();

       
        yield return new WaitForSeconds(2);
        Destroy(countdown);
        highScoreTable.SetActive(true);

        GameController.GetComponent<GameComplete>().enabled = true;
        //PhotonNetwork.LeaveRoom();
    }

    string timeToString(float time)
    {
        if (currentTime > 60)
        {
            minutes = Math.Floor(currentTime / 60);
            min = minutes.ToString("0");
            seconds = currentTime % 60;
            sec = seconds.ToString("00");

            return (min + ":" + sec);
        }
        if (currentTime >= 10)
        {
            return currentTime.ToString("0");
        }
        else if (currentTime < 10 && currentTime > 0)
        {
            return currentTime.ToString("0.0");
        }
        else if (currentTime == 0)
        {
            return "GAME OVER";
        }
        return "GAME OVER";
    }

    int getFontSize(float time)
    {
        float perc = time % 1;

        return (int)Mathf.Lerp(40, 50, perc);
    }

    void stopMoving()
    {


        player1 = GameObject.FindWithTag("Player1");
        player2 = GameObject.FindWithTag("Player2");
        player3 = GameObject.FindWithTag("Player3");
        player4 = GameObject.FindWithTag("Player4");

        player1.GetComponent<Movement>().moveable = false;

/*      player2.GetComponent<Movement>().moveable = false;
        player3.GetComponent<Movement>().moveable = false;
        player4.GetComponent<Movement>().moveable = false;*/
    }   

    public void endGame()
    {
        highScoreTable.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }
}

  

