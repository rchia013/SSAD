using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class HighscoreTable : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    float templateHeight = 55f;

    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);
    }

    public void endGameUpdateTable(List<Record> records)
    {
        print("UpdateEnd");

        List<Record> SortedList = records.OrderBy(o => o.Points).ToList();

        for (int i = 0; i < SortedList.Count; i++)
        {
            print("Entry"+(i+1));

            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector3(50, -templateHeight * i - 20, 0);
            entryTransform.gameObject.SetActive(true);

            print(SortedList[i].playerName);

            entryTransform.Find("Rank").GetComponent<TextMeshProUGUI>().text = (i+1).ToString();
            entryTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = SortedList[i].playerName;
            entryTransform.Find("Points").GetComponent<TextMeshProUGUI>().text = SortedList[i].Points.ToString();
        }
    }

    //PhotonNetwork.LeaveRoom();
}
