using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Proyecto26;
using Newtonsoft.Json;
using UnityEngine.UI;

public class Achievement_controller : MonoBehaviour
{
    public List<Achievement> playerinfo = new List<Achievement>();
    public GameObject Bronze;
    public GameObject Silver;
    public GameObject Gold;

    public GameObject firstBar;

    public TextMeshProUGUI points;
    public TextMeshProUGUI nameBox;
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
            nameBox.text = playerinfo.username;

            if (playerinfo.achievementPoints <= 300)
            {
                firstBar.GetComponent<Image>().fillAmount = ((float)playerinfo.achievementPoints / 300);
            }
            else
            {
                firstBar.GetComponent<Image>().fillAmount = 1;
            }
            

            if (playerinfo.achievementPoints >= 100)
            {
                Bronze.SetActive(true);
            }
            if (playerinfo.achievementPoints >= 200)
            {
                Silver.SetActive(true);
            }
            if (playerinfo.achievementPoints >= 300)
            {
                Gold.SetActive(true);
            }
        });

    }

}
