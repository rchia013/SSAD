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
/*            switch (rank)
            {
                default:
                    rankString = rank + "th";break;
                case 1:
                    rankString = "1st"; break;
                case 2:
                    rankString = "2nd"; break;
                case 3:
                    rankString = "3rd"; break;
            }*/

            entryTransform.Find("Rank").GetComponent<Text>().text = rank.ToString();
            int score = Random.Range(0, 100);
            entryTransform.Find("Points").GetComponent<Text>().text = score.ToString();
            string playerName = "Player " + (i+1).ToString();
            entryTransform.Find("Name").GetComponent<Text>().text = playerName;
        }
    }
}
