using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Proyecto26;
using Newtonsoft.Json;

public class Achievement_controller : MonoBehaviour
{
    public List<Achievement> playerinfo = new List<Achievement>();
    public GameObject Bronze;
    public GameObject Silver;
    public GameObject Gold;
    public TextMeshProUGUI points;
    string localID = Login.localid;

    // Start is called before the first frame update
    void Start()
    {
        Achievement playerinfo = new Achievement();
        string playerurl = "https://quizguyz.firebaseio.com/Users/" + localID;
        RestClient.Get(url: playerurl + ".json").Then(onResolved: response =>
        {
            playerinfo = JsonConvert.DeserializeObject<Achievement>(response.Text);
            print("player points count = " + playerinfo.achievementPoints);

            points.text = playerinfo.achievementPoints.ToString();
            if (playerinfo.achievementPoints > 50 && playerinfo.achievementPoints < 100)
            {
                Bronze.SetActive(true);
            }
            
            else if (playerinfo.achievementPoints > 100 && playerinfo.achievementPoints < 150)
            {
                Bronze.SetActive(true);
                Silver.SetActive(true);
            }
            else if (playerinfo.achievementPoints > 150)
            {
                Bronze.SetActive(true);
                Silver.SetActive(true);
                Gold.SetActive(true);
            }


        });

    }

}
