using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Proyecto26;
using Newtonsoft.Json;
using UnityEngine.UI;

/// <summary>
/// This class handles the logic behind the achievements of each user
/// </summary>
public class Achievement_controller : MonoBehaviour
{
    //List that stores the users information
    public List<Achievement> playerinfo = new List<Achievement>();

    //The Badges that the players will be able to get
    public GameObject Bronze;
    public GameObject Silver;
    public GameObject Gold;
    public GameObject firstBar;

    //Text that displays the user's points and username
    public TextMeshProUGUI points;
    public TextMeshProUGUI nameBox;
    string localID = Login.localid;

    // Start is called before the first frame update
    void Start()
    {

        Achievement playerinfo = new Achievement();
        string playerurl = "https://quizguyz.firebaseio.com/Users/" + localID;

        //API Call to database to retrieve user's information
        RestClient.Get(url: playerurl + ".json").Then(onResolved: response =>
        {
            playerinfo = JsonConvert.DeserializeObject<Achievement>(response.Text);
            points.text = playerinfo.achievementPoints.ToString();
            nameBox.text = playerinfo.username;

            //Handles the badges that the player has
            //E.g. If more than 750 achievement points, player will have the Gold badge
            if (playerinfo.achievementPoints <= 750)
            {
                firstBar.GetComponent<Image>().fillAmount = ((float)playerinfo.achievementPoints / 750);
            }
            else
            {
                firstBar.GetComponent<Image>().fillAmount = 1;
            }
            
            //Displays the badges that the player has on the UI
            if (playerinfo.achievementPoints >= 250)
            {
                Bronze.SetActive(true);
            }
            if (playerinfo.achievementPoints >= 500)
            {
                Silver.SetActive(true);
            }
            if (playerinfo.achievementPoints >= 750)
            {
                Gold.SetActive(true);
            }
        });

    }

}
