using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using Proyecto26;
using Newtonsoft.Json;

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

    public List<Achievement> playerinfo = new List<Achievement>();




    private void Start()
    {
        string QuestionUrl = "https://quizguyz.firebaseio.com/Users.json";
        RestClient.Get(url: QuestionUrl).Then(onResolved: response =>
        {
            Dictionary<string, Achievement> entryDict = JsonConvert.DeserializeObject<Dictionary<string, Achievement>>(response.Text);
            playerinfo = entryDict.Select(x => x.Value).ToList();

            playerinfo.Sort(SortByScore);


            Rank1Player.text = playerinfo[playerinfo.Count - 1].username;
            Rank2Player.text = playerinfo[playerinfo.Count - 2].username;
            Rank3Player.text = playerinfo[playerinfo.Count - 3].username;
            Rank4Player.text = playerinfo[playerinfo.Count - 4].username;
            Rank5Player.text = playerinfo[playerinfo.Count - 5].username;
            Rank6Player.text = playerinfo[playerinfo.Count - 6].username;
            Rank7Player.text = playerinfo[playerinfo.Count - 7].username;
            Rank8Player.text = playerinfo[playerinfo.Count - 8].username;

            Rank1PlayerScore.text = playerinfo[playerinfo.Count - 1].achievementPoints.ToString();
            Rank2PlayerScore.text = playerinfo[playerinfo.Count - 2].achievementPoints.ToString();
            Rank3PlayerScore.text = playerinfo[playerinfo.Count - 3].achievementPoints.ToString();
            Rank4PlayerScore.text = playerinfo[playerinfo.Count - 4].achievementPoints.ToString();
            Rank5PlayerScore.text = playerinfo[playerinfo.Count - 5].achievementPoints.ToString();
            Rank6PlayerScore.text = playerinfo[playerinfo.Count - 6].achievementPoints.ToString();
            Rank7PlayerScore.text = playerinfo[playerinfo.Count - 7].achievementPoints.ToString();
            Rank8PlayerScore.text = playerinfo[playerinfo.Count - 8].achievementPoints.ToString();
        });

    }

    static int SortByScore(Achievement p1, Achievement p2)
    {
        return p1.achievementPoints.CompareTo(p2.achievementPoints);
    }

}
