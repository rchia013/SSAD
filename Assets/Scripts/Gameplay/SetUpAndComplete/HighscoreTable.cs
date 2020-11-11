using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class HighscoreTable : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    float templateHeight = 30f;

    private void Awake()
    {
        //entryTemplate.gameObject.SetActive(false);
    }

    public void endGameUpdateTable(List<Record> records)
    {
        for (int i = 0; i < records.Count; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector3(-53, 10-(templateHeight * i), 0);
            entryTransform.gameObject.SetActive(true);

            entryTransform.Find("Rank").GetComponent<TextMeshProUGUI>().text = records[i].Rank.ToString();
            entryTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = records[i].playerName;
            entryTransform.Find("Points").GetComponent<TextMeshProUGUI>().text = records[i].Points.ToString();
        }
    }

    public void OnClickEnd()
    {
        SceneManager.LoadScene("Main Menu");
        //SceneManager.UnloadSceneAsync("SampleScene");

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }
}
