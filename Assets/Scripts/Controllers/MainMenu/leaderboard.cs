using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using Proyecto26;
using Newtonsoft.Json;

/// <summary>
/// This class handles the leaderboard UI and logic displayed in the main menu 
/// </summary>
public class leaderboard : MonoBehaviour
{
    // Names of the top 8 players 
    public TextMeshProUGUI Rank1Player;
    public TextMeshProUGUI Rank2Player;
    public TextMeshProUGUI Rank3Player;
    public TextMeshProUGUI Rank4Player;
    public TextMeshProUGUI Rank5Player;
    public TextMeshProUGUI Rank6Player;
    public TextMeshProUGUI Rank7Player;
    public TextMeshProUGUI Rank8Player;

    // Points of the top 8 players
    public TextMeshProUGUI Rank1PlayerScore;
    public TextMeshProUGUI Rank2PlayerScore;
    public TextMeshProUGUI Rank3PlayerScore;
    public TextMeshProUGUI Rank4PlayerScore;
    public TextMeshProUGUI Rank5PlayerScore;
    public TextMeshProUGUI Rank6PlayerScore;
    public TextMeshProUGUI Rank7PlayerScore;
    public TextMeshProUGUI Rank8PlayerScore;

    // List of all the players information in the database
    public List<Achievement> playerinfo = new List<Achievement>();




    private void Start()
    {
        string QuestionUrl = "https://quizguyz.firebaseio.com/Users.json";

        // API to retrieve the players information
        RestClient.Get(url: QuestionUrl).Then(onResolved: response =>
        {
            // Retrieving all the players information
            Dictionary<string, Achievement> entryDict = JsonConvert.DeserializeObject<Dictionary<string, Achievement>>(response.Text);
            playerinfo = entryDict.Select(x => x.Value).ToList();

            // Sorting the scores of all the players in order
            playerinfo.Sort(SortByScore);

            // Displays the names of the top 8 players on the leaderboard UI
            Rank1Player.text = playerinfo[playerinfo.Count - 1].username;
            Rank2Player.text = playerinfo[playerinfo.Count - 2].username;
            Rank3Player.text = playerinfo[playerinfo.Count - 3].username;
            Rank4Player.text = playerinfo[playerinfo.Count - 4].username;
            Rank5Player.text = playerinfo[playerinfo.Count - 5].username;
            Rank6Player.text = playerinfo[playerinfo.Count - 6].username;
            Rank7Player.text = playerinfo[playerinfo.Count - 7].username;
            Rank8Player.text = playerinfo[playerinfo.Count - 8].username;

            // Displays the points of the top 8 players on the leaderboard UI
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
    /// <summary>
    /// Sorts the list by score.
    /// </summary>
    /// <param name="p1">The p1.</param>
    /// <param name="p2">The p2.</param>
    /// <returns> Returns a sorted list </returns>
    static int SortByScore(Achievement p1, Achievement p2)
    {
        return p1.achievementPoints.CompareTo(p2.achievementPoints);
    }

}
