using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class leaderboard : MonoBehaviour
{
    public TextMeshProUGUI Rank1Player;
    public TextMeshProUGUI Rank2Player;
    public TextMeshProUGUI Rank3Player;
    public TextMeshProUGUI Rank4Player;
    public TextMeshProUGUI Rank5Player;
    public TextMeshProUGUI Rank6Player;
    public TextMeshProUGUI Rank7Player;
    public TextMeshProUGUI Rank8Player;

    public TextMeshProUGUI Rank1PlayerScore;
    public TextMeshProUGUI Rank2PlayerScore;
    public TextMeshProUGUI Rank3PlayerScore;
    public TextMeshProUGUI Rank4PlayerScore;
    public TextMeshProUGUI Rank5PlayerScore;
    public TextMeshProUGUI Rank6PlayerScore;
    public TextMeshProUGUI Rank7PlayerScore;
    public TextMeshProUGUI Rank8PlayerScore;


    private void Start()
    {
        Rank1Player.text = "Fuck you";
    }

}
