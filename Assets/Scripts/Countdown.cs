using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Countdown : MonoBehaviour
{

    float currentTime = 0f;
    float startingTime = 15.5f;
    float totalTime;
    float minutes;
    string min;
    string sec;
    float seconds;
    bool start;


    [SerializeField] TextMeshProUGUI countdown;

    private void Start()
    {

        currentTime = startingTime;
        bool start = false;
     
    }

    private void Update()
    {

        StartCoroutine(Delay());
        if (start)
        {
            if (currentTime >= 10)
            {
                currentTime -= 1 * Time.deltaTime;
                minutes = currentTime % 60;
                min = minutes.ToString();
                seconds = currentTime / 60;
                sec = seconds.ToString();
                
                countdown.text = min +_ sec;
            }
            else if (currentTime < 10 && currentTime > 0)
            {
                currentTime -= 1 * Time.deltaTime;
                countdown.text = currentTime.ToString("0.0");
                countdown.color = Color.red;
            }
            else if (currentTime <= 0)
            {
                currentTime = 0;
                countdown.color = Color.red;
            }

        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        start = true;
    }
}
