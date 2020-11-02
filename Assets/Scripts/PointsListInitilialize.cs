using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsListInitilialize : MonoBehaviour
{

    public GameObject[] borders;
    public GameObject[] playerBoxes;

    public Material blue;
    public Material pink;
    public Material green;
    public Material yellow;
    public Material purple;
    public Material brown;

    public Dictionary<string, int> playerList;

    // Start is called before the first frame update
    void Start()
    {
        playerList = LobbySetUp.LS.playerList;

        int i = 0;
        foreach (KeyValuePair<string, int> player in playerList)
        {

            setUpBoxColor(i, player.Value);

            if (player.Key == Login.currentUser.username)
            {
                setUpHighlight(i);
            }

            i++;
        }
    }

    void setUpBoxColor(int index, int choice)
    {
        playerBoxes[index].SetActive(true);

        int color = choice % 10;
        Color chosen = Color.white;

        switch (color)
        {
            case 1:
                chosen = blue.color;
                break;

            case 2:
                chosen = pink.color;
                break;

            case 3:
                chosen = green.color;
                break;

            case 4:
                chosen = yellow.color;
                break;

            case 5:
                chosen = purple.color;
                break;

            case 6:
                chosen = brown.color;
                break;
        }

        playerBoxes[index].GetComponent<Image>().color = chosen;


    }

    void setUpHighlight(int index)
    {
        borders[index].SetActive(true);
    }
}
