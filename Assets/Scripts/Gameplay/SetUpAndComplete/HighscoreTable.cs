using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles the logic behind the highscore table UI at the end of every game.
/// </summary>
public class HighscoreTable : MonoBehaviour
{
    // Parameters for the entries in the highscore table
    public Transform entryContainer;
    public Transform entryTemplate;
    float templateHeight = 30f;

    /// <summary>
    /// Updates the table at the end of the game.
    /// </summary>
    /// <param name="records">The records.</param>
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

    /// <summary>
    /// Called when the player clicks 'Done' on the highscore table UI.
    /// </summary>
    public void OnClickEnd()
    {
        SceneManager.LoadScene("Main Menu");
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }
}
