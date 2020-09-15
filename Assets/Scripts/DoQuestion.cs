using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class DoQuestion : MonoBehaviour
{
    public TextMeshProUGUI description;

    Button[] buttons = new Button[4];

    public Button b1;
    public Button b2;
    public Button b3;
    public Button b4;

    public bool pointsAwardable;
    public bool answered = false; 
    public bool correct = false;

    Image background;
    Color originalColor;

    GameObject player;
    public string playerTag;
    int playerIndex;
    private PhotonView PV;

    private Question question;

    // Start is called before the first frame update
    void Start()
    {
        // Color Setup

        originalColor = new Color(0, 0, 0, (29 / 255));

        // Player Information

        // player = GameObject.FindWithTag(playerTag);
        player = GameSetUp.GS.player;
        playerIndex = GameSetUp.GS.playerIndex;
        PV = player.GetComponent<Movement>().PV;

        // Setup Question;

        question = getQuestion();

        // Setup Buttons

        buttons[0] = b1;
        buttons[1] = b2;
        buttons[2] = b3;
        buttons[3] = b4;

        setupUI(question);

        b1.onClick.AddListener(Correct);

        background = gameObject.GetComponent<Image>();
        background.color = originalColor;

    }

    private Question getQuestion()
    {
        //return QuestionManager.QM.getRandomQuestion(playerIndex);
        return null;
    }


    private void setupUI(Question question)
    {
        /*description.SetText(question.Description);

        Dictionary<string, bool> Options = question.Options;

        var OptionDescriptions = Options.Keys.ToList();

        var answer = Options.FirstOrDefault(x => x.Value == true).Key;

        for (int i = 0; i < 4; i++)
        {
            buttons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(OptionDescriptions[i]);

            if (OptionDescriptions[i] == answer)
            {
                buttons[i].onClick.AddListener(Correct);
            }
            else
            {
                buttons[i].onClick.AddListener(Wrong);
            }
        }*/
    }

    private void OnEnable()
    {
        background = gameObject.GetComponent<Image>();
        background.color = new Color(0, 0, 0, (29 / 255));
    }

    void Correct()
    {
        print("Correct");
        if (pointsAwardable)
        {
            Debug.Log("Points"+ playerIndex);
            PV.RPC("ChangePoints", RpcTarget.All, playerIndex, 3);
            // player.GetComponent<Movement>().ChangePoints(playerIndex, 3);
        }
        gameObject.SetActive(false);

        answered = true;
        correct = true;
    }

    void Wrong()
    {
        print("Wrong");

        if (pointsAwardable)
        {
            Debug.Log(playerIndex);
            PV.RPC("ChangePoints", RpcTarget.All, playerIndex, -3);
            // player.GetComponent<Movement>().ChangePoints(playerIndex, -3);
        }

        answered = true;
        correct = false;

        StartCoroutine("FailBGChange");
    }

    IEnumerator FailBGChange()
    {
        Color color = background.color;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1)
        {
            Color newColor = new Color(Mathf.Lerp(color.r, 0.965f, t), Mathf.Lerp(color.g, 0.275f,t), Mathf.Lerp(color.b, 0.196f,t), Mathf.Lerp(color.a, 0.396f, t));

            background.color = newColor;

            yield return null;
        }
    }
}
