using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    float templateHeight = 55f;
    string rankString;

    public void endGameUpdateTable()
    {

    }

    private void Awake()
    {
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector3(50, -templateHeight * i - 20,0);
            entryTransform.gameObject.SetActive(true);

            int rank = i + 1;

            entryTransform.Find("Rank").GetComponent<Text>().text = rank.ToString();
            int score = Random.Range(0, 100);
            entryTransform.Find("Points").GetComponent<Text>().text = score.ToString();
            string playerName = "Player " + (i+1).ToString();
            entryTransform.Find("Name").GetComponent<Text>().text = playerName;
        }
    }
}
